using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Entities.Common;
using App.Core.Data.Enums;
using App.Core.Security;

namespace App.Data.DTO.Common
{
    [RightsCheckList(nameof(FileStore))]
    public class FileStoreDTO: BaseDTO
    {
        [DisplayName("Тип файлу")]
        public FileType FileType { get; set; }

        [PredicateCase()]
        public string EntityName { get; set; }

        [PredicateCase()]
        public Guid EntityId { get; set; }

        public string FilePath { get; set; }
        public string FileName { get; set; }

        [DisplayName("Ім'я файлу")]
        public string OrigFileName { get; set; }

        public string ContentType { get; set; }

        [DisplayName("Розмір файлу")]
        public double FileSize { get; set; }

        [NotMapped]
        [DisplayName("Розмір файла")]
        public string FileSizeCount
        {
            get
            {
                var sizeKb = FileSize / 1000;
                if (sizeKb >= 1)
                    return Math.Round(sizeKb, 2) + " " + "Кбайт";
                else
                    return FileSize + " " + "байт";
            }
            set { }
        }

        public bool Ock { get; set; }
        [DisplayName("Тип документа")]
        public string DocumentType { get; set; }

        [DisplayName("Опис")]
        public string Description { get; set; }
        
        [DisplayName("Статус")]
        public string EdocumentStatus { get; set; }

        public string EdocumentType { get; set; }

        public DateTime EdocumentCreatedOn { get; set; }
    }
}
