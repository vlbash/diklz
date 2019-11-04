using App.Core.Base;
using App.Core.Data.CustomAutoMapper;
using App.Core.Data.Interfaces;
using App.Core.Data.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace App.Core.Business.Services
{
    public class BaseService<TListDTO, TDetailDTO, TEntity>: BaseService<TDetailDTO, TEntity>, IBaseService<TListDTO, TDetailDTO, TEntity>
        where TListDTO : CoreDTO
        where TDetailDTO : CoreDTO
        where TEntity : CoreEntity
    {
        public IDTOService<TDetailDTO> DetailService => DTOService;
        public IDTOService<TListDTO> ListService { get; set; }

        public BaseService(IDTOService<TListDTO> listService,
                           IDTOService<TDetailDTO> detailService,
                           IEntityRepository<TEntity> entityRepository)
            : base(detailService, entityRepository)
        {
            ListService = listService;
        }
        
        public IQueryable<TListDTO> GetListDTO(IDictionary<string, string> paramList, params object[] paramArray)
        {
            return ListService.GetDTO(paramList, paramArray);
        }
        public IQueryable<TListDTO> GetListDTO(params object[] paramArray)
        {
            return ListService.GetDTO(null, paramArray);
        }

        public IQueryable<TDetailDTO> GetDetailDTO(IDictionary<string, string> paramList, params object[] paramArray)
        {
            return DetailService.GetDTO(paramList, paramArray);
        }
        public IQueryable<TDetailDTO> GetDetailDTO(params object[] paramArray)
        {
            return DetailService.GetDTO(null, paramArray);
        }
    }

    public class BaseService<TDTO, TEntity>: EntityService<TEntity>, IBaseService<TDTO, TEntity>
        where TDTO : CoreDTO
        where TEntity : CoreEntity
    {
        private static readonly IMapper _mapper;

        static BaseService()
        {
            var dtoType = typeof(TDTO);
            if (typeof(IMapped).IsAssignableFrom(dtoType)) {
                _mapper = (IMapper)dtoType.GetProperty("GetMapper", BindingFlags.Public | BindingFlags.Static).GetValue(null, null);
            } else {
                _mapper = new MapperConfiguration(cfg => cfg.CreateMap<TDTO, TEntity>().MapOnlyIfChanged(EntityName)).CreateMapper();
            }
        }

        public IDTOService<TDTO> DTOService { get; set; }

        public BaseService(IDTOService<TDTO> dtoService,
                           IEntityRepository<TEntity> entityRepository)
            : base(entityRepository)
        {
            DTOService = dtoService;
        }

        public IQueryable<TDTO> GetDTO(IDictionary<string, string> paramList, params object[] paramArray)
        {
            return DTOService.GetDTO(paramList, paramArray);
        }
        public IQueryable<TDTO> GetDTO(params object[] paramArray)
        {
            return DTOService.GetDTO(null, paramArray);
        }

        /// <summary>
        /// Map DTO to Entity and Add/Update.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Guid> SaveAsync(TDTO dto, bool transacted = false)
        {
            var id = await AttachAsync(dto);

            if (!transacted)
                await Repository.SaveAsync();

            return id;
        }

        public async Task<Guid> AttachAsync(TDTO dto)
        {
            TEntity entity = null;

            if (dto.Id == Guid.Empty)
            {
                //entity = _mapper.Map<TEntity>(dto, opt => opt.Items["rights"] = DTOService.DTORepository.UserInfo.Rights);
                entity = _mapper.Map<TEntity>(dto);
                await Repository.AddAsync(entity);
                dto.Id = entity.Id;
            }
            else
            {
                entity = Repository.GetEntity().FirstOrDefault(x => x.Id == dto.Id);
                if (entity == null)
                {
                    //entity = _mapper.Map<TEntity>(dto, opt => opt.Items["rights"] = DTOService.DTORepository.UserInfo.Rights);
                    entity = _mapper.Map<TEntity>(dto);
                    await Repository.AddAsync(entity);
                }
                else
                {
                    //_mapper.Map(dto, entity, opt => opt.Items["rights"] = DTOService.DTORepository.UserInfo.Rights);
                    _mapper.Map(dto, entity);
                    Repository.Update(entity);
                }
            }

            return entity.Id;
        }
    }
}
