using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using App.Business.ViewModels;
using App.Core.Business.Services;
using App.Core.Data.Entities.Common;
using App.Core.Data.Helpers;
using App.Data.DTO.Common;
using Microsoft.Extensions.Configuration;

namespace App.Business.Helpers
{
    public static class FileSignHelper
    {
        public static string CalculateMd5(MemoryStream stream)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(stream);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static double GetOriginalLengthInBytes(string base64string)
        {
            if (string.IsNullOrEmpty(base64string)) { return 0; }

            var characterCount = base64string.Length;
            var paddingCount = base64string.Substring(characterCount - 2, 2)
                .Count(c => c == '=');
            return (3 * (characterCount / 4)) - paddingCount;
        }

        public static async Task SaveFile(IConfiguration config, FilesViewModel model, FileStoreDTO fileStoreDto, ICommonDataService dataService)
        {
            var folderForSave = config.GetSection("FileStorePath").Value + DateTime.Now.ToString("ddMMyyyy") + "/";
            var filePath = Path.GetFullPath(folderForSave);

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var newName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".zip";
            var tempfolder = Path.GetTempPath();
            var fullOrigPath = tempfolder + model.name;
            string fullZipPath = filePath + newName;
            var directoryInfo = (new FileInfo(fullOrigPath)).Directory;
            directoryInfo?.Create();
            await File.WriteAllBytesAsync(fullOrigPath, Convert.FromBase64String(model.file));

            CreateZip(fullZipPath, fullOrigPath);

            FileStoreHelper.DeleteFileIfExist(fullOrigPath);
            fileStoreDto.FileName = newName;
            fileStoreDto.FilePath = fullZipPath;
            dataService.Add<FileStore>(fileStoreDto);
        }

        private static bool CreateZip(string zipFileName, string fileToZip)
        {
            FileInfo zipFile = new FileInfo(zipFileName);
            FileStream fs = zipFile.Create();
            using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(fileToZip, Path.GetFileName(fileToZip), CompressionLevel.Optimal);
            }
            return true;
        }

    }
}
