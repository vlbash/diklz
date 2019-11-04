using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace App.Core.Data.CustomAudit
{
    public static class CustomAuditExtensions
    {
        private static void SetNewEntityFields(IEntity entity, UserInfo userInfo, IDocumentHelper documentHelper)
        {
            if (entity is ICoreEntity baseEntity)
            {
                baseEntity.CreatedBy = userInfo.PersonId;
                baseEntity.CreatedOn = DateTime.Now;
                if (entity.Id == Guid.Empty) {
                    entity.Id = Guid.NewGuid();
                }
            }

            if (entity is IDocument doc && doc.RegNumber == null)
            {
                documentHelper.SetRegNumber(doc);
            }
        }

        private static void UpdateEntityFields(IEntity entity, UserInfo userInfo)
        {
            if (entity is ICoreEntity baseEntity)
            {
                baseEntity.ModifiedBy = userInfo.PersonId;
                baseEntity.ModifiedOn = DateTime.Now;
            }
        }

        public static int SaveChanges_Auditable(this CoreDbContext context)
        {
            return context.SaveChanges(AuditHelper.AutoSaveWithAuditEntryFactory(context.UserInfo?.PersonId.ToString()));
            //return context.SaveChanges(AuditHelper.AutoSaveWithAuditEntryFactory(context.UserInfo.UserGUID));
        }

        public static async Task<int> SaveChangesAsync_Auditable(this CoreDbContext context)
        {
            return await context.SaveChangesAsync(AuditHelper.AutoSaveWithAuditEntryFactory(context.UserInfo?.PersonId.ToString()));
            //return await context.SaveChangesAsync(AuditHelper.AutoSaveWithAuditEntryFactory(context.UserInfo.UserGUID));
        }

        private static void AddInternal(CoreDbContext context, IEntity entity)
        {
            if (entity is IDerivedEntity derived)
            {
                AddInternal(context, derived._BaseClone);
            }

            context.Add(entity);
        }
        public static void Add_Auditable(this CoreDbContext context, IEntity entity)
        {
            SetNewEntityFields(entity, context.UserInfo, context.DocumentHelper);
            AddInternal(context, entity);
        }
        
        public static void AddRange_Auditable(this CoreDbContext context, params IEntity[] entities)
        {
            foreach (var entity in entities)
            {
                Add_Auditable(context, entity);
            }
        }

        public static void AddRange_Auditable(this CoreDbContext context, IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                Add_Auditable(context, entity);
            }
        }

        private static async Task AddInternalAsync(CoreDbContext context, IEntity entity)
        {
            if (entity is IDerivedEntity derived)
            {
                await AddInternalAsync(context, derived._BaseClone);
            }

            await context.AddAsync(entity);
        }
        public static async Task AddAsync_Auditable(this CoreDbContext context, IEntity entity)
        {
            SetNewEntityFields(entity, context.UserInfo, context.DocumentHelper);
            await AddInternalAsync(context, entity);
        }

        public static void Remove_Auditable(this CoreDbContext context, IEntity entity)
        {
            if (entity is IDerivedEntity derived)
            {
                Remove_Auditable(context, derived._BaseQuery(context));
            }

            if (entity != null)
            {
                context.Entry(entity).State = EntityState.Deleted;
            }
        }


        private static void UpdateInternal(CoreDbContext context, IEntity entity)
        {
            if (entity is IDerivedEntity derived)
            {
                UpdateInternal(context, derived._BaseUpdate(context));
            }
        }

        public static void Update_Auditable(this CoreDbContext context, IEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                context.Entry(entity).State = EntityState.Unchanged;
            }

            UpdateEntityFields(entity, context.UserInfo);
            UpdateInternal(context, entity);
        }
    }
}
