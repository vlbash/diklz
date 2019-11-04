using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Data.Helpers;
using App.Data.Contexts;
using App.Data.DTO.LIMS;
using App.Data.Models.APP;
using App.Data.Models.PRL;
using App.WebApi.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace App.Data.Repositories
{
    public class LimsRepository
    {
        private readonly LimsDbContext _limsDbContext;
        private readonly ISqlRepositoryHelper _sqlHelper;
        private readonly ICommonDataService _dataService;
        private IConfiguration _configuration;

        public LimsRepository(ISqlRepositoryHelper sqlHelper, ICommonDataService dataService, LimsDbContext limsDbContext, IConfiguration configuration)
        {
            this._sqlHelper = sqlHelper;
            _dataService = dataService;
            _limsDbContext = limsDbContext;
            _configuration = configuration;
        }

        private string GetSqlText(string fileName)
        {
            //return _sqlHelper.GetSqlText(type);
            //var x = this.GetType().GetTypeInfo().Assembly.GetManifestResourceNames();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"App.Data.DTO.LIMS.{fileName}.sql";

            string result;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }

        #region P902

        public async Task<long> InsertSgdRepDrugListP902(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteSgdRepDrugInsert");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsId = Convert.ToInt64(paramCollection[0].Value);
            return limsId;
        }

        public async Task UpdateRepDrugStatusP902(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteSgdRepDrugUpdate");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        #endregion
        public async Task<long> InsertPrlApplication(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecutePrlInsertApplication");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsAppId = Convert.ToInt64(paramCollection[0].Value);
            return limsAppId;
        }

        public async Task<long> InsertImlApplication(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteImlInsertApplication");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsAppId = Convert.ToInt64(paramCollection[0].Value);
            return limsAppId;
        }

        public async Task<long> InsertTrlApplication(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteTrlInsertApplication");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsAppId = Convert.ToInt64(paramCollection[0].Value);
            return limsAppId;
        }

        public void CloseApplication(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("PrlAppSetStatus");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception($"Виникла помилка при закритті заяви {paramCollection.Find(parameter => parameter.ParameterName == "@p_AppId")}", e);
            }
        }

        public async Task<long> InsertPrlBranch(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecutePrlInsertBranch");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsId = Convert.ToInt64(paramCollection[0].Value);
            return limsId;
        }

        public async Task<long> InsertImlBranch(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteImlInsertBranch");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsId = Convert.ToInt64(paramCollection[0].Value);
            return limsId;
        }

        public async Task<long> InsertTrlBranch(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteTrlInsertBranch");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsId = Convert.ToInt64(paramCollection[0].Value);
            return limsId;
        }

        public async Task <long> GetId (List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteGetId");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            try
            {
                var limsId = Convert.ToInt64(paramCollection[0].Value);
                return limsId;
            }
            catch
            {
                return 1;
            }
            //return limsId;
        }

        public void PrlAppBranchAdd(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecutePrlAppBranchAdd");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        public void TrlAppBranchAdd(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteTrlAppBranchAdd");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        public void ImlAppBranchAdd(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteImlAppBranchAdd");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        public async Task<long> InsertAppDrugIML(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteInsertAppDrugIML");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsAppId = Convert.ToInt64(paramCollection[0].Value);
            return limsAppId;
        }

        public void ImlAppBranchRem(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteImlAppBranchRem");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        public async Task TrlCheckDivAdd(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteTrlCheckDivAdd");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        public async Task<long> TrlCheckCreate(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteTrlCheckCreate");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsAppId = Convert.ToInt64(paramCollection[0].Value);
            return limsAppId;
        }

        //public async Task TrlCheckSelect(List<SqlParameter> paramCollection)
        //{
        //    var sql = GetSqlText("ExecuteTrlCheckSelect");
        //    try
        //    {
        //        _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Виникла помилка при імпорті данних", e);
        //    }
        //}

        public async Task<long> PrlUpdateAssignee(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecutePrlAuthUpdate");  // для лицензии CRV_LICENSE_AUTH_PERSON_INSERT (созданая с целью обновления поля діє в старом лимсе)
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsAppId = Convert.ToInt64(paramCollection[0].Value);
            return limsAppId;
        }

        public async Task<long> PrlUpdateAssigneeIMLLicense(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecutePrlAuthUpdateLicense");  // для лицензии CRV_LICENSE_AUTH_PERSON_INSERT (созданая с целью обновления поля діє (в старом лимсе)) ПЕредаем бренч, получаем id копиилицензии
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsAppId = Convert.ToInt64(paramCollection[0].Value);
            return limsAppId;
        }

        public void PrlUpdatePersonIds(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("PrlUpdatePersonIds");  // для лицензии CRV_LICENSE_AUTH_PERSON_UPDATE2 (созданая с целью обновления поля діє в старом лимсе)
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        public void PrlUpdatePersonIdsIMLLicense(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ImlUpdatePersonIds");  // для лицензии CRV_LICENSE_AUTH_PERSON_UPDATE2 (созданая с целью обновления поля діє в старом лимсе)
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        public void PrlUpdateAssigneePRL(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecutePrlAuthUpdatePRL"); //для заявы
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        public void PrlUpdateAssigneeIML(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecutePrlAuthUpdateIML"); //для заявы
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        public void PrlUpdateAssigneeIMLPerson(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecutePrlAuthUpdateIMLPerson"); //для заявы
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        public async Task<long> PrlLicenseProcessRun(List<SqlParameter> paramCollection, long appId)
        {
            var sql = GetSqlText("ExecuteNewPrlLicenseProcessRun");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті ліцензії", e);
            }
            var limsId = Convert.ToInt64(paramCollection[0].Value);
            return limsId;
        }

        public async Task<long> ImlLicenseProcessRun(List<SqlParameter> paramCollection, long appId)
        {
            var sql = GetSqlText("ExecuteNewImlLicenseProcessRun");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті ліцензії", e);
            }
            var limsId = Convert.ToInt64(paramCollection[0].Value);
            return limsId;
        }

        public async Task<long> TrlLicenseProcessRun(List<SqlParameter> paramCollection, long appId)
        {
            var sql = GetSqlText("ExecuteNewTrlLicenseProcessRun");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті ліцензії", e);
            }
            var limsId = Convert.ToInt64(paramCollection[0].Value);
            return limsId;
        }

        public void AppSetStatusIML(long appId)
        {
            var preSql = $"EXECUTE IML_APP_SET_STATUS @p_AppId={appId}, @p_StatusId=4";
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(preSql);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void AppSetStatus(long appId)
        {
            var preSql = $"EXECUTE PRL_APP_SET_STATUS @p_AppId={appId}, @p_StatusId=4";
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(preSql);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void AppSetStatusTRL(long appId)
        {
            var preSql = $"EXECUTE TRL_LIC_APP_SET_STATUS @p_LicAppId={appId}, @p_StatusId=4";
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(preSql);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task UpdateExpertise(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("PrlExpertiseUpdate");
            try
            {
                //TODO mace it async
                //var a = await _limsDbContext.Database.ExecuteSqlCommandAsync(sql, paramCollection);
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних єкспертизи", e);
            }
        }

        public async Task<long> InsertLimsNotice(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecutePrlNoticeInsert");
            try
            {
                await _limsDbContext.Database.ExecuteSqlCommandAsync(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті ліцензії", e);
            }
            var limsId = Convert.ToInt64(paramCollection[0].Value);
            return limsId;
        }

        public async Task<long> InsertLimsNoticeIML(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteImlNoticeInsert");
            try
            {
                await _limsDbContext.Database.ExecuteSqlCommandAsync(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті ліцензії", e);
            }
            var limsId = Convert.ToInt64(paramCollection[0].Value);
            return limsId;
        }

        public async Task<long> InsertLimsNoticeTRL(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteTrlNoticeInsert");
            try
            {
                await _limsDbContext.Database.ExecuteSqlCommandAsync(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті ліцензії", e);
            }
            var limsId = Convert.ToInt64(paramCollection[0].Value);
            return limsId;
        }

        public async Task<IQueryable<LimsNotice>> GetLimsNotice()
        {
            var sql = GetSqlText("ImportLimsNotice");
            var protocolList = _limsDbContext.LimsNotices.FromSql(sql);
            return protocolList;
        }

        public async Task<IQueryable<LimsCheck>> GetLimsCheck()
        {
            var sql = GetSqlText("ImportLimsCheck");
            return _limsDbContext.LimsChecks.FromSql(sql);
        }

        public async Task<IQueryable<LimsEndLicCheck>> GetEndLicCheck()
        {
            var sql = GetSqlText("ImportLimsEndLicCheck");
            return _limsDbContext.LimsEndLicChecks.FromSql(sql);
        }

        public async Task<long> InsertPrlLicenseCheck(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecutePrlCheckInsert");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsId = Convert.ToInt64(paramCollection[0].Value);
            return limsId;
        }

        public async Task<long> InsertPrlLicenseCheckIML(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteImlCheckInsert");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsId = Convert.ToInt64(paramCollection[0].Value);
            return limsId;
        }
        public async Task<long> InsertPrlLicenseCheckTRL(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteTrlCheckInsert");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var limsId = Convert.ToInt64(paramCollection[0].Value);
            return limsId;
        }

        public void DeletePrlLicenseCheck(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecutePrlCheckDelete");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        public void DeletePrlLicenseCheckIML(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteImlCheckDelete");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        public void DeletePrlLicenseCheckTRL(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteTrlCheckDelete");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
        }

        public IQueryable<T> GetLimsEntity<T>(ChangesTrackedEnum typeEnum) where T : class
        {
            string sql;
            switch (typeEnum)
            {
                case ChangesTrackedEnum.AppProtocol:
                    sql = GetSqlText("ImportLimsProtocols");
                    break;
                case ChangesTrackedEnum.AppNotice:
                    sql = GetSqlText("ImportLimsProtocols");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeEnum), typeEnum, null);
            }
            return _limsDbContext.Query<T>().FromSql(sql);
        }

        public async Task<IQueryable<LimsProtocol>> GetLimsProtocols()
        {
            var sql = GetSqlText("ImportLimsProtocols");
            var protocolList = _limsDbContext.LimsProtocols.FromSql(sql);
            return protocolList;
        }

        public async Task<IQueryable<LimsOldRP>> GetLimsRp()
        {
            var sql = GetSqlText("ImportLimsRP");
            var RpList = _limsDbContext.LimsOldRP.FromSql(sql);
            return RpList;
        }

        public IQueryable<LimsOldRP> GetLimsRP()
        {
            var sql = GetSqlText("ImportLimsRP");
            var rpList = _limsDbContext.LimsOldRP.FromSql(sql);
            return rpList;
        }

        public IQueryable<LimsSpodu> GetLimsSpodu()
        {
            var sql = GetSqlText("ImportLimsDepartmentalSubordination");
            var spoduList = _limsDbContext.LimsSpodu.FromSql(sql);
            return spoduList;
        }

        public async Task<List<LicenseLIMS>> GetLimsLicense(string licType, string edrpou)
        {
            string lictype;
            switch (licType)
            {
                case "PRL":
                    lictype = "1";
                    break;
                case "TRL":
                    lictype = "2,3,4";
                    break;
                case "IML":
                    lictype = "5";
                    break;
                default:
                    lictype = "1,2,3,4,5";
                    break;
            }
            var sql = new StringBuilder(GetSqlText("License"));
            sql.Replace("@@LicTypes@@", lictype);
            sql.Replace("@@EDRPOU@@", edrpou);

            return await _limsDbContext.Licenses.FromSql(sql.ToString()).ToListAsync();
        }

        public IQueryable<LimsPendingChanges> GetPendingChanges(ChangesTrackedEnum? entity = null)
        {
            var sql = GetSqlText("GetPendingChanges");
            var changesQuery = _limsDbContext.PendingChangeses.FromSql(sql);

            if (entity == null)
            {
                return changesQuery;
            }

            var entityName = entity.Value.GetLimsTableName();
            return changesQuery.Where(x => x.EntityName == entityName);
        }

        public IQueryable<LimsPendingChanges> GetPendingChangesToInsert(ChangesTrackedEnum? entity = null)
        {
            var sql = GetSqlText("GetPendingChangesToInsert");
            var changesQuery = _limsDbContext.PendingChangeses.FromSql(sql);

            if (entity == null)
            {
                return changesQuery;
            }

            var entityName = entity.Value.GetLimsTableName();
            return changesQuery.Where(x => x.EntityName == entityName);
        }

        public void UpdatePendingChanges(List<long> pendingChangesEntityIds)
        {
            if (pendingChangesEntityIds.Count == 0)
            {
                return;
            }
            var sqlQuery = new StringBuilder("UPDATE dbo.PendingChanges SET Processed = 1 WHERE EntityId IN (");
            sqlQuery.AppendJoin(',', pendingChangesEntityIds);
            sqlQuery.Append(")");
            var rowsUpdated = _limsDbContext.Database.ExecuteSqlCommand(sqlQuery.ToString());
            //if (rowsUpdated != pendingChangesIds.Count)
            //{
            //    throw new Exception($"Update pending changes error! Incorrect amount of rows updated! " +
            //                        $"Pending changes to be updated: {pendingChangesIds.Count}. Affected rows on update: {rowsUpdated}");
            //}
        }

        public void UpdatePrlDecision(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("PrlDecisionUpdate");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                string message = "Виникла помилка при оновленні рішення, будь ласка зверніться до адміністратора";
                Log.Error($"Message: {message} Error:{e.Message} InnerError:{e.InnerException?.Message}");
                throw new Exception(message);
            }
        }

        public void UpdateDecisionIML(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ImlDecisionUpdate");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                string message = "Виникла помилка при оновленні рішення, будь ласка зверніться до адміністратора";
                Log.Error($"Message: {message} Error:{e.Message} InnerError:{e.InnerException?.Message}");
                throw new Exception(message);
            }
        }

        public void UpdateDecisionTRL(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteTrlDecisionUpdate");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                string message = "Виникла помилка при оновленні рішення, будь ласка зверніться до адміністратора";
                Log.Error($"Message: {message} Error:{e.Message} InnerError:{e.InnerException?.Message}");
                throw new Exception(message);
            }
        }

        public void RemoveDecision(SqlParameter appId)
        {
            var sql = new StringBuilder("[dbo].[PRL_APP_DECISION_DELETE] @p_AppId").ToString();
            _limsDbContext.Database.ExecuteSqlCommand(sql, appId);
        }

        public void RemoveDecisionIML(SqlParameter appId)
        {
            var sql = new StringBuilder("[dbo].[IML_APP_DECISION_DELETE] @p_AppId").ToString();
            _limsDbContext.Database.ExecuteSqlCommand(sql, appId);
        }

        public void RemoveDecisionTRL(SqlParameter appId)
        {
            var sql = new StringBuilder("[dbo].[TRL_LIC_APP_DECISION_DELETE] @p_LicAppId").ToString();
            _limsDbContext.Database.ExecuteSqlCommand(sql, appId);
        }

        public void RemoveLicenseFile(long oldLimsId)
        {
            var appParams = new SqlParameter("@p_DocId", SqlDbType.Int) { Value = oldLimsId };
            var sql = new StringBuilder("[dbo].[DelInsertFile] @p_DocId").ToString();
            _limsDbContext.Database.ExecuteSqlCommand(sql, appParams);
        }

        public enum ChangesTrackedEnum
        {
            AppProtocol,
            AppNotice,
            AppCheck,
            EndLicCheck,
            LimsRp,
            Application
        }

        public async Task<long> InsertAttachment(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteAttachInsert");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var fileId = Convert.ToInt64(paramCollection[0].Value);
            return fileId;
        }

        public async Task<long> InsertAttachmentMPD(List<SqlParameter> paramCollection)
        {
            var sql = GetSqlText("ExecuteAttachInsertMPD");
            try
            {
                _limsDbContext.Database.ExecuteSqlCommand(sql, paramCollection);
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при імпорті данних", e);
            }
            var fileId = Convert.ToInt64(paramCollection[0].Value);
            return fileId;
        }
        public void InsertAttaches(List<SqlParameter> paramCollection)
        {
            var connectionString = _configuration.GetConnectionString("LimsAttachConnection");

            var sql = GetSqlText("ExecuteAttachFilesInsert");

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(sql, connection);
                paramCollection.ForEach(x => command.Parameters.Add(x));
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Log.Error("Lims_attach database exception");
                    throw new Exception("Виникла помилка при імпорті данних", e);
                }
            }
        }
    }

    internal static class ChangesTrackedExtenstion
    {
        public static string GetLimsTableName(this LimsRepository.ChangesTrackedEnum changesEnum)
        {
            var dic = new Dictionary<LimsRepository.ChangesTrackedEnum, string>
            {
                {LimsRepository.ChangesTrackedEnum.AppProtocol, "LIC_PROTOCOL"},
                {LimsRepository.ChangesTrackedEnum.AppNotice, "APP_NOTICE" },
                {LimsRepository.ChangesTrackedEnum.AppCheck, "PRL_CHECK" },
                {LimsRepository.ChangesTrackedEnum.EndLicCheck, "DOC_LICENSE" },
                {LimsRepository.ChangesTrackedEnum.LimsRp, "LIMS_RP"  }
            };

            return dic.ContainsKey(changesEnum) ? dic[changesEnum] : null;
        }

        public static Type GetLimsImportType(this LimsRepository.ChangesTrackedEnum changesEnum)
        {
            var dic = new Dictionary<LimsRepository.ChangesTrackedEnum, Type>
            {
                {LimsRepository.ChangesTrackedEnum.AppProtocol, typeof(LimsProtocol)},
                {LimsRepository.ChangesTrackedEnum.AppNotice, typeof(LimsNotice)},
                {LimsRepository.ChangesTrackedEnum.AppCheck, typeof(LimsCheck) },
                {LimsRepository.ChangesTrackedEnum.EndLicCheck, typeof(LimsEndLicCheck) }
            };

            return dic.ContainsKey(changesEnum) ? dic[changesEnum] : null;
        }

        //public static Type GetLimsEntityName(this LimsRepository.ChangesTrackedEnum changesEnum)
        //{
        //    var dic = new Dictionary<LimsRepository.ChangesTrackedEnum, Type>
        //    {
        //        {LimsRepository.ChangesTrackedEnum.AppProtocol, typeof(LimsProtocol)}
        //    };

        //    return dic.ContainsKey(changesEnum) ? dic[changesEnum] : null;
        //}
    }
}
