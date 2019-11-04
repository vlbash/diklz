using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ReflectionIT.Mvc.Paging {

    public class PagingList {

        public static async Task<PagingList<T>> CreateAsync<T>(IOrderedQueryable<T> qry, int pageSize, int pageIndex,
            Func<T, int?> totalCount = null,
            string action = "Index") where T : class
        {
            var list = await qry.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalRecordCountInvoke = (totalCount != null && list.Count > 0) ? totalCount(list.First()) : null;
            var totalRecordCount = totalRecordCountInvoke ?? await qry.CountAsync();
            var pageCount = (int)Math.Ceiling(totalRecordCount / (double)pageSize);
            return new PagingList<T>(list, pageSize, pageIndex, pageCount, totalRecordCount) { Action = action };
        }

        public static async Task<PagingList<T>> CreateAsync<T>(IQueryable<T> qry, int pageSize, int pageIndex, string sortExpression, string defaultSortExpression,
            Func<T, int?> totalCount = null,
            string action = "Index") where T : class
        {
            var list = await Extensions.OrderBy(qry, sortExpression).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalRecordCountInvoke = (totalCount != null && list.Count > 0) ? totalCount(list.First()) : null;
            var totalRecordCount = totalRecordCountInvoke ?? await qry.CountAsync();
            var pageCount = (int)Math.Ceiling(totalRecordCount / (double)pageSize);
            return new PagingList<T>(list, pageSize, pageIndex, pageCount, sortExpression, defaultSortExpression, totalRecordCount) { Action = action };
        }

        public static PagingList<T> Create<T>(IEnumerable<T> collection, int pageSize, int pageIndex,
            Func<T, int?> totalCount = null,
            string action = "Index",
            bool readyToUse = false) where T : class
        {
            IEnumerable<T> list;
            if (readyToUse) {
                list = collection;                
            } else {
                list = collection.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            int? totalRecords = null;
            if (totalCount != null) {
                var first = collection.FirstOrDefault();
                if (first != null) {
                    totalRecords = totalCount(first);
                }
            }

            var totalRecordCount = totalRecords ?? collection.Count();
            var pageCount = (int)Math.Ceiling(totalRecordCount / (double)pageSize);
            return new PagingList<T>(list, pageSize, pageIndex, pageCount, totalRecordCount) { Action = action };
        }

        public static PagingList<T> Create<T>(IEnumerable<T> collection, int pageSize, int pageIndex, string sortExpression, string defaultSortExpression,
            Func<T, int?> totalCount = null,
            string action = "Index",
            bool readyToUse = false) where T : class
        {
            IEnumerable<T> list;
            if (readyToUse) {
                list = collection;
            }
            else {
                list = collection.OrderBy(sortExpression).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            int? totalRecords = null;
            if (totalCount != null) {
                var first = collection.FirstOrDefault();
                if (first != null) {
                    totalRecords = totalCount(first);
                }
            }

            var totalRecordCount = totalRecords ?? collection.Count();
            var pageCount = (int)Math.Ceiling(totalRecordCount / (double)pageSize);
            return new PagingList<T>(list, pageSize, pageIndex, pageCount, sortExpression, defaultSortExpression, totalRecordCount) { Action = action };
        }
    }
}