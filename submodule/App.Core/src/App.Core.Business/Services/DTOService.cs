using App.Core.Data.Helpers;
using App.Core.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Core.Business.Services
{
    public class DTOService<TDTO>: IDTOService<TDTO> where TDTO : class
    {
        public IDTORepository<TDTO> DTORepository { get; }

        public DTOService(IDTORepository<TDTO> dtoRepository)
        {
            DTORepository = dtoRepository;
        }

        public IQueryable<TDTO> GetDTO(IDictionary<string, string> parameters = null, params object[] paramArray)
        {
            return DTORepository.GetDTO(parameters, paramArray);
        }

        public IQueryable<TDTO> GetDTO(params object[] paramArray)
        {
            return DTORepository.GetDTO(null, paramArray);
        }
    }
}
