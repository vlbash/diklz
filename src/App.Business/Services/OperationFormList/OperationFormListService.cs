using System;
using App.Data.DTO.HelperDTOs;
using Microsoft.Extensions.Caching.Memory;

namespace App.Business.Services.OperationFormList
{
    public class OperationFormListService : IOperationFormListService
    {
        private readonly IMemoryCache _memoryCache;

        public OperationFormListService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public OperationListDTO GetOperationListDTO()
        {
            if (!_memoryCache.TryGetValue("OperationListDTO", out OperationListDTO operationList))
            {
                operationList = new OperationListDTO().InitialiseObject();
                _memoryCache.Set("OperationListDTO", operationList, TimeSpan.FromHours(24));
            }
            return operationList;
        }
    }
}
