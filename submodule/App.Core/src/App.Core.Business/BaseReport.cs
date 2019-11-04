using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using App.Core.Business.Services;

namespace App.Core.Business
{
    public class BaseReport<TReportDTO> where TReportDTO : class
    {
        public IQueryable<TReportDTO> Data => ReportData;
        protected readonly IDTOService<TReportDTO> DtoService;
        protected IQueryable<TReportDTO> ReportData;

        public BaseReport(IDTOService<TReportDTO> baseService)
        {
            DtoService = baseService;
        }

        public virtual void Generate()
        {
            GenerateReport();
        }

        public virtual void Generate(Expression<Func<TReportDTO, bool>> predicate)
        {
            GenerateReport(predicate: predicate);
        }

        public virtual void Generate(IDictionary<string, string> paramList)
        {
            GenerateReport(parameters: paramList);
        }

        public virtual void Generate(IDictionary<string, string> paramList, Expression<Func<TReportDTO, bool>> predicate)
        {
            GenerateReport(paramList, predicate);
        }

        protected virtual void ProcessData()
        {

        }

        private void GenerateReport(IDictionary<string, string> parameters = null, Expression<Func<TReportDTO, bool>> predicate = null)
        {
            ReportData = DtoService.GetDTO(parameters);

            if (predicate != null) {
                ReportData = ReportData.Where(predicate);
            }

            ProcessData();
        }
    }
}


