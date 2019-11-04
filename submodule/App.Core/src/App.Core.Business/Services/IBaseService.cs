using App.Core.Base;
using App.Core.Data.DTO.Common;
using App.Core.Data.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Core.Business.Services
{
    public interface IBaseService<TListDTO, TDetailDTO, TEntity>: IBaseService<TDetailDTO, TEntity>
        where TListDTO : CoreDTO
        where TDetailDTO : CoreDTO
        where TEntity : CoreEntity
    {
        IDTOService<TDetailDTO> DetailService { get; }
        IDTOService<TListDTO> ListService { get; set; }

        IQueryable<TListDTO> GetListDTO(IDictionary<string, string> paramList, params object[] paramArray);
        IQueryable<TListDTO> GetListDTO(params object[] paramArray);

        IQueryable<TDetailDTO> GetDetailDTO(IDictionary<string, string> paramList, params object[] paramArray);
        IQueryable<TDetailDTO> GetDetailDTO(params object[] paramArray);
    }


    public interface IBaseService<TDTO, TEntity>: IEntityService<TEntity>
        where TDTO : CoreDTO
        where TEntity : CoreEntity
    {
        IDTOService<TDTO> DTOService { get; set; }

        IQueryable<TDTO> GetDTO(IDictionary<string, string> paramList, params object[] paramArray);
        IQueryable<TDTO> GetDTO(params object[] paramArray);

        Task<Guid> SaveAsync(TDTO dto, bool transacted = false);
        Task<Guid> AttachAsync(TDTO dto);
    }
}
