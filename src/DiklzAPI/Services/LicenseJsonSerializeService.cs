using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using App.WebApi.Contexts;
using App.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace App.WebApi.Services
{
    internal class LicenseJsonSerializeService: ILicenseJsonSerializeService
    {
        private readonly APIContext _context;

        public LicenseJsonSerializeService(APIContext context)
        {
            _context = context;
        }

        public List<License> GetLicenses(IList<string> loggingList)
        {
            string path = null;
            string sqlFile = null;
            try
            {
                path = Path.Combine(Path.GetFullPath("Models/"), "License.sql");
                Log.Information($"Opening sql file, located at: {path}");
                loggingList.Add($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: Opening sql file, located at: {path}");
                sqlFile = File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Unable to read file! Path: {path}");
                loggingList.Add($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: Unable to read file! Path: {path}");
                throw;
            }

            List<License> licenses = null;
            try
            {
                loggingList.Add($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: Start executing SQL-query");
                licenses = _context.Licenses.FromSql(sqlFile).ToList();
                loggingList.Add($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: SQL-query executed - {licenses.Count} licenses");
            }
            catch (Exception e)
            {
                Log.Error(e, "Unable to get license data from server!");
                loggingList.Add($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: Unable to get license data from SQL-server");
                throw;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<Branch>), new XmlRootAttribute("Branches"));


            try
            {
                loggingList.Add($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: Start parsing XML branches to object");

                licenses?.ForEach(x =>
                {
                x.Branches = string.IsNullOrEmpty(x.ListOfBranchesString)
                    ? new List<Branch>()
                    : serializer.Deserialize(new StringReader($"<Branches> {x.ListOfBranchesString} </Branches>")) as List<Branch>;
                    x.ListOfBranchesString = null;
                });
                loggingList.Add($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: XML branches successfully parsed");

            }
            catch (Exception e)
            {
                loggingList.Add($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: Error on parsing XML branches");
                throw;
            }

            return licenses;
        }
    }
}
