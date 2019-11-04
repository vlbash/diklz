using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Base;
using App.Core.Business.Services;
using App.Data.DTO.BRN;
using App.Data.Models.APP;
using App.Data.Models.DOC;
using App.Data.Models.PRL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Host.Areas.PRL.Controllers
{
    [Authorize(Policy = "Registered")]
    [Area("Prl")]
    public class PrlVerificationController: Controller
    {
        private readonly ICommonDataService _dataService;

        public PrlVerificationController(ICommonDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IActionResult> Index(Guid appId, bool jsonData)
        {
            var application = _dataService.GetEntity<PrlApplication>(x => x.Id == appId).FirstOrDefault();
            if (application == null)
                return await Task.Run(() => NotFound());

            var errorModel = new Dictionary<string, string>();
            StringBuilder error = new StringBuilder();
            StringBuilder code = new StringBuilder();

            var branchList = (await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId));
            var branchListIds = branchList.Select(dto => dto.Id);

            if (!branchList.Any())
            {
                error.Append("<p>").Append("Місця провадження діяльності").Append("</p>")
                    .Append("<p>")
                    .Append("Перевіряється наявність доданого до заяви хоча б одного місця провадження діяльності")
                    .Append("</p>");
                code.Append("До заяви не додано хоча б одне місце провадження діяльності.");
                errorModel.Add(error.ToString(), code.ToString());
                error.Clear();
                code.Clear();
            }
            else
            {
                var assigneeBranches =
                    _dataService.GetEntity<AppAssigneeBranch>(x => branchListIds.Contains(x.BranchId) && x.RecordState != RecordState.D);
                var assigneeBranchIds = assigneeBranches.Select(x => x.BranchId).Distinct();
                if (assigneeBranchIds.Count() != branchListIds.Count())
                {
                    error.Append("<p>").Append("Реєстр уповноважених осіб").Append("</p>")
                        .Append("<p>")
                        .Append("Перевіряється наявність хоча б одної уповноваженої особи для кожного введеного МПД")
                        .Append("</p>");

                    code.Append("Перелік зауважень по реєстру 'Уповноважені особи': ").Append("<br>");

                    foreach (var branchId in branchListIds)
                    {
                        if (!assigneeBranchIds.Contains(branchId))
                        {
                            code.Append("Для ");
                            code.Append("\"")
                                .Append(branchList.Where(x => x.Id == branchId).Select(y => y.Name).First())
                                .Append("\"");
                            code.Append(" не вказано уповноважену особу").Append("<br>");
                        }
                    }

                    errorModel.Add(error.ToString(), code.ToString());
                    error.Clear();
                    code.Clear();
                }

                var edocBranches = _dataService.GetEntity<BranchEDocument>(x => branchListIds.Contains(x.BranchId) && x.RecordState != RecordState.D);
                var edocBranchIds = edocBranches.Select(x => x.BranchId).Distinct();
                if (edocBranchIds.Count() != branchListIds.Count())
                {
                    error.Append("<p>").Append("Реєстр досьє з виробництва").Append("</p>")
                        .Append("<p>").Append("Перевіряється наявність досьє з виробництва для кожного введеного МПД")
                        .Append("</p>");

                    code.Append("Перелік зауважень по реєстру 'Реєстр досьє': ").Append("<br>");

                    foreach (var branchId in branchListIds)
                    {
                        if (!edocBranchIds.Contains(branchId))
                        {
                            code.Append("Для ");
                            code.Append("\"")
                                .Append(branchList.Where(x => x.Id == branchId).Select(y => y.Name).First())
                                .Append("\"");
                            code.Append(" не додано досьє з виробництва").Append("<br>");
                        }
                    }

                    errorModel.Add(error.ToString(), code.ToString());
                    error.Clear();
                    code.Clear();
                }
            }

            if (jsonData)
                return new JsonResult(errorModel);
            return PartialView(model: errorModel);
        }

        public async Task<IActionResult> AddBranchApplication(Guid appId, bool jsonData)
        {
            var application = _dataService.GetEntity<PrlApplication>(x => x.Id == appId).FirstOrDefault();
            if (application == null)
                return await Task.Run(() => NotFound());

            var errorModel = new Dictionary<string, string>();
            StringBuilder error = new StringBuilder();
            StringBuilder code = new StringBuilder();

            var branchList = (await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId));
            var branchListIds = branchList.Select(dto => dto.Id);

            if (!branchList.Any(x=>x.IsFromLicense != true))
            {
                error.Append("<p>").Append("Місця провадження діяльності").Append("</p>")
                    .Append("<p>")
                    .Append("Перевіряється наявність доданого до заяви хоча б одного місця провадження діяльності")
                    .Append("</p>");
                code.Append("До заяви не додано хоча б одне місце провадження діяльності.");
                errorModel.Add(error.ToString(), code.ToString());
                error.Clear();
                code.Clear();
            }
            else
            {
                var assigneeBranches =
                    _dataService.GetEntity<AppAssigneeBranch>(x => branchListIds.Contains(x.BranchId) && x.RecordState != RecordState.D);
                var assigneeBranchIds = assigneeBranches.Select(x => x.BranchId).Distinct();
                if (assigneeBranchIds.Count() != branchListIds.Count())
                {
                    error.Append("<p>").Append("Реєстр уповноважених осіб").Append("</p>")
                        .Append("<p>")
                        .Append("Перевіряється наявність хоча б одної уповноваженої особи для кожного введеного МПД")
                        .Append("</p>");

                    code.Append("Перелік зауважень по реєстру 'Уповноважені особи': ").Append("<br>");

                    foreach (var branchId in branchListIds)
                    {
                        if (!assigneeBranchIds.Contains(branchId))
                        {
                            code.Append("Для ");
                            code.Append("\"")
                                .Append(branchList.Where(x => x.Id == branchId).Select(y => y.Name).First())
                                .Append("\"");
                            code.Append(" не вказано уповноважену особу").Append("<br>");
                        }
                    }

                    errorModel.Add(error.ToString(), code.ToString());
                    error.Clear();
                    code.Clear();
                }

                var edocBranches = _dataService.GetEntity<BranchEDocument>(x => branchListIds.Contains(x.BranchId) && x.RecordState != RecordState.D);
                var edocBranchIds = edocBranches.Select(x => x.BranchId).Distinct();
                if (edocBranchIds.Count() != branchListIds.Count())
                {
                    error.Append("<p>").Append("Реєстр досьє з виробництва").Append("</p>")
                        .Append("<p>").Append("Перевіряється наявність досьє з виробництва для кожного введеного МПД")
                        .Append("</p>");

                    code.Append("Перелік зауважень по реєстру 'Реєстр досьє': ").Append("<br>");

                    foreach (var branchId in branchListIds)
                    {
                        if (!edocBranchIds.Contains(branchId))
                        {
                            code.Append("Для ");
                            code.Append("\"")
                                .Append(branchList.Where(x => x.Id == branchId).Select(y => y.Name).First())
                                .Append("\"");
                            code.Append(" не додано досьє з виробництва").Append("<br>");
                        }
                    }

                    errorModel.Add(error.ToString(), code.ToString());
                    error.Clear();
                    code.Clear();
                }
            }

            if (jsonData)
                return new JsonResult(errorModel);
            return PartialView("Index", errorModel);
        }

        public async Task<IActionResult> Verification_RemBranchApplication(Guid appId, bool jsonData)
        {
            var application = _dataService.GetEntity<PrlApplication>(x => x.Id == appId).FirstOrDefault();
            if (application == null)
                return await Task.Run(() => NotFound());

            var errorModel = new Dictionary<string, string>();
            StringBuilder error = new StringBuilder();
            StringBuilder code = new StringBuilder();

            var branchList = (await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId));
           

            if (!branchList.Any())
            {
                throw new Exception("Виникла помилка, зверніться до адміністатора");
            }
            else
            {
                if (!branchList.Any(x => x.LicenseDeleteCheck == true))
                {
                    error.Append("<p>").Append("Місця провадження діяльності").Append("</p>")
                        .Append("<p>")
                        .Append("Перевіряється наявність доданого до заяви хоча б одного місця провадження діяльності")
                        .Append("</p>");
                    code.Append("Ви вне вказали жодного МПД для внесення в Заяву про внесення змін до ЄДР в зв'язку з " +
                                "припинення діяльності за певним місцем провадження. Для подання заяви потрібно вказати хоча б одне МПД.");
                    errorModel.Add(error.ToString(), code.ToString());
                    error.Clear();
                    code.Clear();
                }

                if (branchList.All(x => x.LicenseDeleteCheck == true))
                {
                    error.Append("<p>").Append("Місця провадження діяльності").Append("</p>")
                        .Append("<p>")
                        .Append("Перевіряється наявність хоча б одного МПД, яке не буде видалене з ліцензії після розгляду поданої заяви.")
                        .Append("</p>");
                    code.Append("Ви вказали всі МПД для внесення в Заяву про внесення змін до ЄДР в зв'язку з" +
                                " припинення діяльності.Для того, щоб ліцензія вважалася дійсною - в ній повинно бути активним " +
                                "хоча б одне місце провадження діяльності (МПД).");
                    errorModel.Add(error.ToString(), code.ToString());
                    error.Clear();
                    code.Clear();
                }
            }
            if (jsonData)
                return new JsonResult(errorModel);
            return PartialView("Index", errorModel);
        }

        public async Task<IActionResult> Verification_ChangeBranchInfoApplication(Guid appId, bool jsonData)
        {
            var application = _dataService.GetEntity<PrlApplication>(x => x.Id == appId).FirstOrDefault();
            if (application == null)
                return await Task.Run(() => NotFound());

            var errorModel = new Dictionary<string, string>();
            StringBuilder error = new StringBuilder();
            StringBuilder code = new StringBuilder();

            var branchList = (await _dataService.GetDtoAsync<BranchAltListDTO>(x => x.ApplicationId == appId));


            if (!branchList.Any())
            {
                throw new Exception("Виникла помилка, зверніться до адміністатора");
            }
            else
            {
                if (branchList.All(x => string.IsNullOrEmpty(x.OperationListFormChanging)))
                {
                    error.Append("<p>").Append("Місця провадження діяльності").Append("</p>")
                        .Append("<p>")
                        .Append("Перевіряється наявність доданого до заяви хоча б одного місця провадження діяльності")
                        .Append("</p>");
                    code.Append("Ви не внесли змін до переліку виробничих операцій в жодному МПД. Заяву не може будети подано.");
                    errorModel.Add(error.ToString(), code.ToString());
                    error.Clear();
                    code.Clear();
                }
            }
            if (jsonData)
                return new JsonResult(errorModel);
            return PartialView("Index", errorModel);
        }

    }
}
