using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using App.Core.Data.Entities.Common;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Data.Helpers;
using App.Core.Data.Interfaces;
using App.Data.DTO.Common;
using App.Data.Models.DOS;
using Microsoft.AspNetCore.Authorization;
using ReflectionIT.Mvc.Paging;

namespace App.Host.Controllers
{
    [Authorize]
    public class FileStoreController: BaseController<FileStoreDTO, FileStore>
    {
        private IObjectMapper _objectMapper { get; }
        private ICommonDataService _commonDataService { get; }

        public FileStoreController(IObjectMapper objectMapper, ICommonDataService commonDataService) : base()
        {
            _objectMapper = objectMapper;
            _commonDataService = commonDataService;
        }

        public IActionResult UploadForm(Guid EntityId, string EntityName)
        {
            return PartialView(new Core.Data.DTO.Common.FileStoreDTO() { EntityId = EntityId, EntityName = EntityName });
        }

        public override async Task<IActionResult> List(IDictionary<string, string> paramList, ActionListOption<FileStoreDTO> options)
        {
            var entityId = paramList.FirstOrDefault(x => x.Key.Contains("EntityId"));
            var edocId = _commonDataService.GetEntity<EDocument>(p => p.Id == Guid.Parse(entityId.Value)).FirstOrDefault();
            var edocEntity = _commonDataService.GetEntity<EDocument>(p => (p.EntityId == Guid.Parse(entityId.Value)
                                                                           || p.EntityId == (edocId != null ? edocId.EntityId ?? Guid.Empty : Guid.Empty))
                                                                          && p.EDocumentType == "PaymentDocument").Select(p => p.Id).ToList();

            if (edocEntity.Count > 0)
            {
                var filePaymentList = _commonDataService.GetDto<FileStoreDTO>(p => edocEntity.Contains(p.EntityId), dtos => dtos.OrderByDescending(p => p.EdocumentCreatedOn));

                var pagingList = PagingList.Create(filePaymentList, PageRowCount.Value, options.pg_Page, "-EdocumentCreatedOn", "Id", x => (x as IPagingCounted)?.TotalRecordCount, "ListPayment", true);
                return PartialView("ListPayment", pagingList);
            }

            var view = paramList.FirstOrDefault(x => x.Key.Contains("pg_PartialViewName"));
            if (!view.Equals(default(KeyValuePair<string,string>)))
                options.pg_PartialViewName = paramList["amp;pg_PartialViewName"] ;
            return await base.List(paramList, options);
        }

        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public IActionResult Upload(FileStoreDTO upModel)
        {
            if (Request.Form != null && Request.Form.Files.Count > 0)
            {
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 1e+8) 
                    {
                        return Json(new { success = false, fileSize = formFile.Length, fileName = formFile.FileName });
                    }
                    //Костыль с кастомным FileStore. Когда будет новый core, нужно переписать
                    var coreModel = new Core.Data.DTO.Common.FileStoreDTO();
                    _objectMapper.Map(upModel, coreModel);
                    var dto = FileStoreHelper.SaveFile(Configuration, formFile, coreModel);
                    if (dto == null)
                    {
                        continue;
                    }

                    try
                    {
                        _objectMapper.Map(dto, upModel);
                        Service.SaveAsync(upModel).Wait();
                    }
                    catch
                    {
                        FileStoreHelper.DeleteFileIfExist(dto.FilePath);
                        throw;
                    }
                }
                return Ok(new { count = Request.Form.Files.Count });
            }
            else
            {
                return NotFound();
            }
        }


        public IActionResult Download(Guid fileId)
        {
            var fileStoreDTO = Service.GetDetailDTO().FirstOrDefault(x => x.Id == fileId);
            var coreModel = new Core.Data.DTO.Common.FileStoreDTO();
            _objectMapper.Map(fileStoreDTO, coreModel);
            if (FileStoreHelper.LoadFile(coreModel, out var stream, out var contentType))
            {
                return File(stream, contentType, fileStoreDTO.OrigFileName);
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> DeleteFile(Guid id)
        {
            OnDelete += entry =>
            {
                FileStoreHelper.DeleteFileIfExist(entry.FilePath);
            };

            return await base.Delete(id);
        }

        public async Task<IActionResult> GetMedicineTemplate()
        {
            var templatePath = Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Templates/DrugList/Template_DrugList.xlsx");
            var stream = new MemoryStream();
            using (var st = new FileStream(templatePath, FileMode.Open))
            {
                st.CopyTo(stream);
            }

            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Template_DrugList.xlsx");
        }
    }

}
