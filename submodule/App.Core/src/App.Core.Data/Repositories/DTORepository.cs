using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using App.Core.Business.Services;
using App.Core.Data.Helpers;
using App.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Data.Repositories
{
    public class DTORepository<TDTO>: IDTORepository<TDTO> where TDTO : class
    {
        protected CoreDbContext Context { get; }

        protected ISqlRepositoryHelper RepositoryHelper { get; }

        protected static readonly bool TotalRecordsCounted;
        protected static readonly Type DtoType = typeof(TDTO);

        static DTORepository()
        {
            TotalRecordsCounted = typeof(IPagingCounted).IsAssignableFrom(typeof(TDTO));
        }

        public DTORepository(CoreDbContext context, ISqlRepositoryHelper repositoryHelper)
        {
            Context = context;
            RepositoryHelper = repositoryHelper;
        }

        public virtual IQueryable<TDTO> GetDTO(IDictionary<string, string> parameters = null, params object[] paramArray)
        {
            var sqlText = GetParameterizedQueryString(parameters, paramArray);
            return Context.Query<TDTO>().FromSql(sqlText);
        }
      
        public virtual string GetParameterizedQueryString(IDictionary<string, string> parameters = null, params object[] paramArray)
        {
            return RepositoryHelper.GetParameterizedQueryString(DtoType, parameters, TotalRecordsCounted, null, paramArray);
        }

    }
}
