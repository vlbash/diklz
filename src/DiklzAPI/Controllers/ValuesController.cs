//using System;
//using System.Threading.Tasks;
//using App.WebApi.Models;
//using App.WebApi.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace App.WebApi.Controllers
//{
//    [Route("")]
//    [ApiController]
//    public class ValuesController: ControllerBase
//    {
//        private readonly ILicenseJsonSerializeService _license;

//        public ValuesController(ILicenseJsonSerializeService license)
//        {
//            _license = license;
//        }

//        /// <summary>
//        /// Get license by EDRPOU
//        /// </summary>
//        /// <param name="edrpou">License EDRPOU</param>
//        /// <returns>License model</returns>
//        /// <response code="500">License is not found</response>
//        [HttpGet("GetLicenseByEdrpou/{edrpou}")]
//        [ProducesResponseType(typeof(SingleResponse<License>), 200)]
//        public async Task<IActionResult> GetLicenseByEdrpou(string edrpou)
//        {
//            var response = new SingleResponse<License> { };            
//            try
//            {
//                response.Model = new License {Id = Convert.ToInt32(edrpou)};
//                //TODO add database
//            }
//            catch (Exception e)
//            {
//                //TODO _logger.log(e.message);
//                response.DidError = true;
//                response.ErrorMessage = "There was an internal error, please contact to technical support.";
//            }

//            return response.ToHttpResponse();
//        }

//        /// <summary>
//        /// Get license by id
//        /// </summary>
//        /// <param name="id">License id</param>
//        /// <returns></returns>
//        [HttpGet("GetLicenseById/{id}")]
//        [ProducesResponseType(typeof(SingleResponse<License>), 200)]
//        public async Task<IActionResult> GetLicenseById(int id)
//        {
//            var response = new SingleResponse<License>() { Model = new License { Id = id } };

//            return response.ToHttpResponse();
//        }

//        /// <summary>
//        /// Get branch by id
//        /// </summary>
//        /// <param name="id">Branch id</param>
//        /// <returns></returns>
//        [HttpGet("GetBranchById/{id}")]
//        [ProducesResponseType(typeof(SingleResponse<Branch>), 200)]
//        public async Task<IActionResult> GetBranchByIdExample(int id)
//        {
//            var response = new SingleResponse<Branch>() { Model = new Branch { Id = id } };

//            return response.ToHttpResponse();
//        }
//    }
//}
