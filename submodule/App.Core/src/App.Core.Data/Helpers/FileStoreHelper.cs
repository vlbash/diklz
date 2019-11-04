using App.Core.Data.DTO.Common;
using App.Core.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace App.Core.Data.Helpers
{
    public static class FileStoreHelper
    {
        public static string CreateMD5(string input) {
            using (var md5 = MD5.Create()) {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++) {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static FileStoreDTO SaveFile(IConfiguration config, IFormFile formFile, FileStoreDTO fileStoreDTO)
        {
            var folderForSave = config.GetSection("FileStorePath").Value + DateTime.Now.ToString("ddMMyyyy") + "/";
            var filePath = Path.GetFullPath(folderForSave);

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            if (formFile.Length > 0) {
                var fileExt = Path.GetExtension(formFile.FileName).ToLower();
                var newName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".zip";
                var tempfolder = Path.GetTempPath();
                var fullOrigPath = tempfolder + formFile.FileName;
                string fullZipPath = filePath + newName;

                using (var stream = new FileStream(fullOrigPath, FileMode.Create)) {
                    formFile.CopyTo(stream);
                }

                bool result = CreateZip(fullZipPath, fullOrigPath);

                DeleteFileIfExist(fullOrigPath);

                return new FileStoreDTO() {
                    EntityId = fileStoreDTO.EntityId,
                    EntityName = fileStoreDTO.EntityName,
                    FileType = GetFileType(fileExt),
                    FileName = newName,
                    FilePath = fullZipPath,
                    OrigFileName = formFile.FileName,
                    ContentType = GetTypeByContentType(formFile.ContentType),
                    FileSize = formFile.Length,
                    DocumentType = fileStoreDTO.DocumentType,
                    Description = fileStoreDTO.Description
                };
            }

            return null;
        }

        public static bool LoadFile(FileStoreDTO fileStoreDTO, out MemoryStream stream, out string contentType)
        {
            stream = null;
            contentType = GetContentType(Path.GetExtension(fileStoreDTO.OrigFileName).ToLower());

            if (fileStoreDTO == null)
                return false;

            string tempDir = Path.GetTempPath();

            if (UnZipFile(fileStoreDTO.FilePath, tempDir, fileStoreDTO.OrigFileName)) {
                var unZipFilePath = Path.Combine(tempDir, fileStoreDTO.OrigFileName);
                stream = new MemoryStream();
                using (var st = new FileStream(unZipFilePath, FileMode.Open)) {
                    st.CopyTo(stream);
                }
                stream.Position = 0;
                DeleteFileIfExist(unZipFilePath);
                return true;
            }

            return false;
        }

        private static bool CreateZip(string zipFileName, string fileToZip) {
            FileInfo zipFile = new FileInfo(zipFileName);
            FileStream fs = zipFile.Create();
            using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Create)) {
                zip.CreateEntryFromFile(fileToZip, Path.GetFileName(fileToZip), CompressionLevel.Optimal);
            }
            return true;
        }

        private static bool UnZipFile(string zipFileName, string dirToUnzipTo, string origFileName) {
            using (ZipArchive archive = ZipFile.OpenRead(zipFileName)) {
                //Loops through each file in the zip file
                foreach (ZipArchiveEntry file in archive.Entries) {
                    //Identifies the destination file name and path
                    String fileUnzipFullName = Path.Combine(dirToUnzipTo, origFileName);

                    //Extracts the files to the output folder in a safer manner
                    if (!System.IO.File.Exists(fileUnzipFullName)) {
                        file.ExtractToFile(fileUnzipFullName);
                    }
                }
            }
            return true;
        }

        public static MemoryStream GetMultipleFileZip(Dictionary<string, string> fileStoreInfos)
        {
            var path = Path.GetTempPath();
            using (var ms = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var fileStoreInfo in fileStoreInfos)
                    {
                        if (UnZipFile(fileStoreInfo.Key, path, fileStoreInfo.Value))
                        {
                            var unZipFilePath = Path.Combine(path, fileStoreInfo.Value);
                            zipArchive.CreateEntryFromFile(unZipFilePath, Path.GetFileName(fileStoreInfo.Value),
                                CompressionLevel.Optimal);
                            DeleteFileIfExist(unZipFilePath);
                        }
                    }
                }
                return ms;
            }
        }

        public static void DeleteFileIfExist(string path) {
            if (System.IO.File.Exists(path)) {
                System.IO.File.Delete(path);
            }
        }

        private static string GetContentType(string path) {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private static string GetTypeByContentType(string mime) {
            var types = GetMimeTypes();
            return types.FirstOrDefault(x => x.Value == mime).Key; ;
        }

        private static string GetFileTypeByContentType(string path) {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private static FileType GetFileType(string ext) {
            Dictionary<string, FileType> dict = new Dictionary<string, FileType>
            {
                {".txt", FileType.Txt},
                {".pdf", FileType.Pdf},
                {".doc", FileType.Docx},
                {".docx", FileType.Docx},
                {".xls", FileType.Xlsx},
                {".xlsx", FileType.Xlsx},
                {".png", FileType.Img},
                {".jpg", FileType.Img},
                {".jpeg", FileType.Img},
                {".gif", FileType.Img},
                {".csv", FileType.Csv},
                {".p7s", FileType.Unknown }
            };
            return dict[ext];
        }

        private static Dictionary<string, string> GetMimeTypes() {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".p7s", "application/pkcs7-signature"}
            };
        }
    }
}
