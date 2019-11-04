using System;
using System.Collections.Generic;
using System.Text;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Security;

namespace App.Core.Data.Extensions
{
    public static class UserInfoExtensions
    {
        public static void AssertCanWriteDto(this UserInfo userInfo, string entityName, CoreDTO dto)
        {
            if (userInfo.Rights == null)
            {
                throw new NoRightsException();
            }

            userInfo.Rights.AssertCanWriteEntity(entityName, dto);
        }

        public static void AssertCanReadTypeData(this UserInfo userInfo, Type dataType)
        {
            if (userInfo.Rights == null)
            {
                throw new NoRightsException();
            }

            userInfo.Rights.AssertCanReadTypeData(dataType);
        }

        public static void AssertWritableEntity(this UserInfo userInfo, string entityName)
        {
            if (userInfo.Rights == null)
            {
                throw new NoRightsException();
            }

            userInfo.Rights.AssertWritableEntity(entityName);
        }
    }
}
