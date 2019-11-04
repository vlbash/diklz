using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Core.Base;

namespace App.Core.Business.Services
{
    public interface ICommonDataService
    {
        /// <summary>
        /// Gets the results from the database as Dto collection
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="predicate">Conditions applied to collection</param>
        /// <param name="orderBy">Sorting function that will be applied to collection</param>
        /// <param name="parameters">Specially formatted parametes that will be converted to 'where' conditions</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cacheResultDuration">How long results should be stored in the cache.
        /// Pay attention that result can be not cached at all even if duration is specified.</param>
        /// <param name="extraParameters">These parameters will be placed to {} placeholders in a text query, if any</param>
        /// <returns></returns>
        IEnumerable<TDto> GetDto<TDto>(Expression<Func<TDto, bool>> predicate = null,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : CoreDTO;
        /// <summary>
        /// Gets the results from the database as Dto collection
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="predicate">Conditions applied to collection</param>
        /// <param name="orderBy">Sorting function that will be applied to collection</param>
        /// <param name="parameters">Specially formatted parameters that will be converted to 'where' conditions</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cacheResultDuration">How long results should be stored in the cache.
        /// Pay attention that result can be not cached at all even if duration is specified.</param>
        /// <param name="extraParameters">These parameters will be placed to {} placeholders in a text query, if any</param>
        /// <returns></returns>
        Task<IEnumerable<TDto>> GetDtoAsync<TDto>(Expression<Func<TDto, bool>> predicate = null,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : CoreDTO;
        /// <summary>
        /// Gets the results from the database as Dto collection
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="orderBy">Sorting columns, separated by comma</param>
        /// <param name="predicate">Conditions applied to collection</param>
        /// <param name="parameters">Specially formatted parameters that will be converted to 'where' conditions</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cacheResultDuration">How long results should be stored in the cache.
        /// Pay attention that result can be not cached at all even if duration is specified.</param>
        /// <param name="extraParameters">These parameters will be placed to {} placeholders in a text query, if any</param>
        /// <returns></returns>
        IEnumerable<TDto> GetDto<TDto>(string orderBy,
            Expression<Func<TDto, bool>> predicate = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : CoreDTO;
        /// <summary>
        /// Gets the results from the database as Dto collection
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="orderBy">Sorting columns, separated by comma</param>
        /// <param name="predicate">Conditions applied to collection</param>
        /// <param name="parameters">Specially formatted parameters that will be converted to 'where' conditions</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cacheResultDuration">How long results should be stored in the cache.
        /// Pay attention that result can be not cached at all even if duration is specified.</param>
        /// <param name="extraParameters">These parameters will be placed to {} placeholders in a text query, if any</param>
        /// <returns></returns>
        Task<IEnumerable<TDto>> GetDtoAsync<TDto>(string orderBy,
            Expression<Func<TDto, bool>> predicate = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : CoreDTO;
        /// <summary>
        /// Creates an IQueryable<TEntity> that can be used to query and save instances of TEntity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate">Filter data conditions. Using: x => x.Id == Guid.Empty</param>
        /// <param name="orderBy">Sorting expression. Using: x => x.OrderBy(el => el.Caption)</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="doNotTrackChanges">Tells change tracker do not track any changes for these entities.
        /// In such case Save() and SaveAsync() methods do not affect</param>
        /// <param name="includeProperties">Navigation properties to include</param>
        /// <returns></returns>
        IEnumerable<TEntity> GetEntity<TEntity>(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int skip = 0,
            int take = 0,
            bool doNotTrackChanges = false,
            params string[] includeProperties) where TEntity : class, IEntity;
        /// <summary>
        /// Adds entity without saving to the database. Call Save or SaveAsync to save changes to the database
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="isUpdating">True - if it is updating of existing entity, false - if entity is new, by default null - if not sure</param>
        /// <returns>Id of entity that will be added to the database</returns>
        Guid Add<TEntity>(TEntity entity, bool? isUpdating = null) where TEntity : class, IEntity;
        /// <summary>
        /// Adds dto as TEntity without saving. Call Save or SaveAsync to save the entity to the database
        /// </summary>
        /// <typeparam name="TEntity">Type to which dto should be mapped</typeparam>
        /// <param name="dto"></param>
        /// <returns>Id of entity that will be added to the database</returns>
        /// <summary>
        /// Adds dto as TEntity without saving. Call Save or SaveAsync to save the entity to the database
        /// </summary>
        /// <typeparam name="TEntity">Type to which dto should be mapped</typeparam>
        /// <param name="dto"></param>
        /// <param name="isUpdating">True - if it is updating of existing entity, false - if entity is new, by default null - if not sure</param>
        /// <returns>Id of entity that will be added to the database</returns>
        Guid Add<TEntity>(CoreDTO dto, bool? isUpdating = null) where TEntity : class, IEntity;
        /// <summary>
        /// Soft deleting an entity (changing RecordState property to RecordState.D) without saving changes to database.
        /// Call Save or SaveAsync to submit soft deleting
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <exception cref="InvalidOperationException">If more than one element satisfies the condition</exception>
        /// <returns>Disabled entity will be returned if any.
        /// Returns null if there is no any entity in database or entity has been already soft deleted </returns>
        TEntity Disable<TEntity>(Guid id) where TEntity : class, IEntity;
        /// <summary>
        /// Soft deleting an entity (changing RecordState property to RecordState.D) without saving changes to database.
        /// Call Save or SaveAsync to submit soft deleting
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Disable<TEntity>(TEntity entity) where TEntity : class, IEntity;
        /// <summary>
        /// Marks entity as deleted. Call Save or SaveAsync to submit changes
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Remove<TEntity>(Guid id) where TEntity : class, IEntity;
        /// <summary>
        /// Marks entity as deleted. Call Save or SaveAsync to submit changes
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Remove<TEntity>(TEntity entity) where TEntity : class, IEntity;
        /// <summary>
        /// Saves all made changes to the database
        /// </summary>
        void SaveChanges();
        /// <summary>
        /// Saves all made changes to the database
        /// </summary>
        Task SaveChangesAsync();
    }
}
