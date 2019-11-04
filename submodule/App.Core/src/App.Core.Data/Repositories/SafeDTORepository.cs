using System;
using System.Collections.Generic;
using System.Linq;
using App.Core.Data.Extensions;
using App.Core.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Data.Repositories
{
    public class SafeDTORepository<TDTO>: DTORepository<TDTO> where TDTO: class
    {
        public SafeDTORepository(CoreDbContext context, ISqlRepositoryHelper repositoryHelper) : base(context, repositoryHelper)
        {
        }

        public override IQueryable<TDTO> GetDTO(IDictionary<string, string> parameters = null, params object[] paramArray)
        {
            Context.UserInfo.AssertCanReadTypeData(DtoType);
            return base.GetDTO(parameters, paramArray);
        }

        public override string GetParameterizedQueryString(IDictionary<string, string> parameters = null, params object[] paramArray)
        {
            return RepositoryHelper.GetParameterizedQueryString(DtoType, parameters, TotalRecordsCounted, Context.UserInfo?.Rights, paramArray);
        }
    }
}
