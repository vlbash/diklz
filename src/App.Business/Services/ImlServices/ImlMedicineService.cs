using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Data.DTO.Common;
using App.Core.Data.Entities.Common;
using App.Core.Data.Enums;
using App.Core.Data.Helpers;
using App.Data.Contexts;
using App.Data.DTO.CRV;
using App.Data.DTO.IML;
using App.Data.Models.IML;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using OfficeOpenXml;
using Serilog;

namespace App.Business.Services.ImlServices
{
    public class ImlMedicineService
    {
        public readonly ICommonDataService DataService;
        private readonly IConfiguration _configuration;
        private readonly MigrationDbContext _context;
        private readonly UserInfo _userInfo;

        public ImlMedicineService(ICommonDataService dataService, IConfiguration configuration, MigrationDbContext context, IUserInfoService userInfoService)
        {

            DataService = dataService;
            _configuration = configuration;
            _context = context;
            _userInfo = userInfoService.GetCurrentUserInfo();
        }

        public async Task<ImlMedicineDetailDTO> GetEditModel(Guid? id, IDictionary<string, string> paramList)
        {
            if (id != null && id != Guid.Empty)
                return (await DataService.GetDtoAsync<ImlMedicineDetailDTO>(x => x.Id == id)).FirstOrDefault();

            paramList.TryGetValue("appId", out var strAppId);

            var medicine = new ImlMedicineDetailDTO();
            medicine.ApplicationId = new Guid(strAppId);
            return medicine;
        }

        public async Task<List<string>> UploadMedicine(Guid? appId, IFormFile file)
        {
            var countOfErrors = 0;
            var cellErrors = new List<string>();
            var medicines = new List<ImlMedicine>();

            using (var excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                var workbook = excelPackage.Workbook;
                var worksheet = workbook.Worksheets.First();
                var row = 9;
                var checkString = "";
                var limsRps = await DataService.GetDtoAsync<LimsRPMinDTO>(x => x.EndDate > DateTime.Now && x.OffOrderDate == null, extraParameters: new object[] { "" });
                do
                {
                    var imlMedicine = new ImlMedicine();
                    var checkOnError = false;
                    string cellNum = "";
                    for (var col = 1; col < 14; col++)
                    {
                        var cell = worksheet.Cells[row, col].First();
                        var valueObj = cell.Value;
                        if (valueObj != null)
                        {
                            var value = valueObj.ToString();
                            switch (col)
                            {
                                case 1:
                                    cellNum = value;
                                    break;
                                case 2:
                                    imlMedicine.MedicineName = value;
                                    checkString = value;
                                    break;
                                case 3:
                                    if (string.IsNullOrEmpty(value))
                                        checkOnError = true;
                                    imlMedicine.FormName = value;
                                    break;
                                case 4:
                                    if (string.IsNullOrEmpty(value))
                                        checkOnError = true;
                                    imlMedicine.DoseInUnit = value;
                                    break;
                                case 5:
                                    if (string.IsNullOrEmpty(value))
                                        checkOnError = true;
                                    imlMedicine.NumberOfUnits = value;
                                    break;
                                case 6:
                                    if (string.IsNullOrEmpty(value))
                                        checkOnError = true;
                                    imlMedicine.MedicineNameEng = value;
                                    break;
                                case 7:
                                    if (string.IsNullOrEmpty(value))
                                        checkOnError = true;
                                    var rplz = limsRps.FirstOrDefault(x => x.RegNum == value);
                                    if (rplz == null)
                                    {
                                        cellErrors.Add(cellNum);
                                        checkOnError = true;
                                        break;
                                    }
                                    else
                                        imlMedicine.LimsRpId = rplz.Id;

                                    imlMedicine.RegisterNumber = value;
                                    break;
                                case 8:
                                    if (string.IsNullOrEmpty(value))
                                        checkOnError = true;
                                    imlMedicine.AtcCode = value;
                                    break;
                                case 9:
                                    if (string.IsNullOrEmpty(value))
                                        checkOnError = true;
                                    imlMedicine.ProducerName = value;
                                    break;
                                case 10:
                                    if (string.IsNullOrEmpty(value))
                                        checkOnError = true;
                                    imlMedicine.ProducerCountry = value;
                                    break;
                                case 11:
                                    if (string.IsNullOrEmpty(value))
                                        checkOnError = true;
                                    imlMedicine.SupplierName = value;
                                    break;
                                case 12:
                                    if (string.IsNullOrEmpty(value))
                                        checkOnError = true;
                                    imlMedicine.SupplierCountry = value;
                                    break;
                                case 13:
                                    if (string.IsNullOrEmpty(value))
                                        checkOnError = true;
                                    imlMedicine.SupplierAddress = value;
                                    break;
                                case 14:
                                    if (string.IsNullOrEmpty(value))
                                        checkOnError = true;
                                    imlMedicine.Notes = value;
                                    break;
                            }
                        }
                        else
                        {

                            if (col == 2)
                            {
                                checkString = string.Empty;
                                checkOnError = true;
                                break;
                            }

                        }

                        //if (checkOnError)
                        //    break;
                    }

                    imlMedicine.ApplicationId = appId.Value;
                    imlMedicine.CreatedByJson = _userInfo.PersonId;

                    if (!checkOnError)
                    {
                        medicines.Add(imlMedicine);
                    }
                    //else
                    //    countOfErrors++;

                    row++;
                } while (!string.IsNullOrEmpty(checkString));

                await SaveMedicines(medicines);
            }
            var fileStore = new FileStoreDTO
            {
                FileType = FileType.Unknown,
                OrigFileName = file.Name + ".xlsx",
                FileSize = file.Length,
                Ock = false,
                EntityId = appId.Value,
                EntityName = nameof(ImlApplication),
                ContentType = ".xlsx",
                Description = "Medicines"
            };
            var dto = FileStoreHelper.SaveFile(_configuration, file, fileStore);
            DataService.Add<FileStore>(dto);
            await DataService.SaveChangesAsync();
            return cellErrors;
        }

        public async Task DeleteAll(Guid appId, bool isFromLicense = false)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                conn.Open();

                NpgsqlCommand cmdd = new NpgsqlCommand("call DeleteMedicines(@p1, @p2)", conn);
                cmdd.Parameters.AddWithValue("p1", appId);
                cmdd.Parameters.AddWithValue("p2", isFromLicense);

                await cmdd.ExecuteNonQueryAsync();
                conn.Close();
            }
            catch (NpgsqlException e)
            {
                Log.Error(e.Message);
                throw;
            }
        }
        
        public void UpdateStatus(Guid appId)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                conn.Open();

                NpgsqlCommand cmdd = new NpgsqlCommand("call UpdateStatusMedicines(@p1)", conn);
                cmdd.Parameters.AddWithValue("p1", appId);

                cmdd.ExecuteNonQuery();
                conn.Close();
            }
            catch (NpgsqlException e)
            {
                Log.Error(e.Message);
                throw;
            }
        }



        public async Task<bool> CheckExisting(Expression<Func<ImlMedicine, bool>> predicate)
        {
            return await _context.ImlMedicines.AnyAsync(predicate);
        }

        public async Task SaveMedicines(List<ImlMedicine> medicines)
        {
            var json = JsonConvert.SerializeObject(medicines);
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                conn.Open();

                NpgsqlCommand cmdd = new NpgsqlCommand("call InsertMedicines(@p1)", conn);
                cmdd.Parameters.AddWithValue("p1", json);

                await cmdd.ExecuteNonQueryAsync();
                conn.Close();
            }
            catch (NpgsqlException e)
            {
                Log.Error(e.Message);
                throw;
            }
        }
    }
}
