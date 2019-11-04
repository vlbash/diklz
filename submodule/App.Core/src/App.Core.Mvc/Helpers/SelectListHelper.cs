using App.Core.Data.Entities.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using App.Core.Business.Services;
using LinqKit;
using App.Core.Data.DTO.Common;
using App.Core.Base;

namespace App.Core.Mvc.Helpers
{
    public class SelectListHelper
    {
        private readonly ICommonDataService _dataService;

        public SelectListHelper(ICommonDataService dataService)
        {
            _dataService = dataService;
        }

        private void InitSelectList(ref SelectList list, string initialSelectedValue)
        {
            if ((initialSelectedValue != null) && (list.Count() > 0))
            {
                if (initialSelectedValue == "")
                {
                    var oldList = list.ToList();
                    oldList.Insert(0, new SelectListItem("", "", true));
                    list = new SelectList(oldList, "Value", "Text", list.SelectedValue);
                }
                else
                {
                    foreach (var item in list)
                    {
                        if (item.Value == initialSelectedValue)
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        public SelectList List<T>(
            string idPropertyName = "Id",
            string textPropertyName = "Caption",
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string initialSelectedValue = null,
            int skip = 0,
            int take = 0,
            int expirationTimeSeconds = 0) where T : CoreDTO
        {
            var data = _dataService.GetDto<T>(predicate, orderBy, cacheResultDuration: expirationTimeSeconds)
                .Skip(skip);
            if (take > 0)
            {
                data = data.Take(take);
            }

            var selList = new SelectList(data, idPropertyName, textPropertyName);
            
            InitSelectList(ref selList, initialSelectedValue);
            return selList;
        }

        public SelectList Enum(string enumType,
            Expression<Func<EnumRecordDto, bool>> predicate = null,
            Func<IQueryable<EnumRecordDto>, IOrderedQueryable<EnumRecordDto>> orderBy = null,
            string initialSelectedValue = null,
            int take = 0,
            int expirationTimeSeconds = 600)
        {
            Expression<Func<EnumRecordDto, bool>> filter;
            if (predicate == null)
            {
                filter = x => x.EnumType.ToLower() == enumType.ToLower();
            } else
            {
                filter = predicate.And(x => x.EnumType.ToLower() == enumType.ToLower());
            }

            return List<EnumRecordDto>("Code", "Name", filter, orderBy, initialSelectedValue, 0, take, expirationTimeSeconds);
        }

        
        public MultiSelectList Multi<T>(
            IEnumerable selectedItems = null,
            string idPropertyName = "Id",
            string textPropertyName = "Caption",
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int skip = 0,
            int take = 0,
            int expirationTimeSeconds = 0) where T : CoreDTO
        {
            var data = _dataService.GetDto<T>(predicate, orderBy, cacheResultDuration: expirationTimeSeconds)
                .Skip(skip);
            if (take > 0)
            {
                data = data.Take(take);
            }

            return new MultiSelectList(data, idPropertyName, textPropertyName, selectedItems);
        }
    }
}
