using App.Core.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace App.Core.Business.Services
{
    public interface IDTOService<TDTO> where TDTO : class
    {
        IDTORepository<TDTO> DTORepository { get; }

        IQueryable<TDTO> GetDTO(IDictionary<string, string> paramList = null, params object[] paramArray);
        IQueryable<TDTO> GetDTO(params object[] paramArray);
    }
}

