using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;
using Core.Common.Attributes;
using Core.Common.Enums;

namespace Core.Data.Common.Dto
{
    //[RightsCheckList(nameof(FileStore))]
    public abstract class BaseFileStoreDto: BaseDto
    {
        [Display(Name = "Тип файлу")]
        public virtual FileType FileType { get; set; }

        [CaseFilter(CaseFilterOperation.Equals)]
        public virtual string EntityName { get; set; }

        //public string EntityNameLocalized { get; set; }

        [CaseFilter(CaseFilterOperation.Equals)]
        public virtual Guid EntityId { get; set; }

        public virtual string FilePath { get; set; }
        public virtual string FileName { get; set; }

        [Display(Name = "Ім'я файлу")]
        public virtual string OrigFileName { get; set; }

        public virtual string ContentType { get; set; }

        [Display(Name = "Розмір файлу")]
        public virtual double FileSize { get; set; }

        [NotMapped]
        [Display(Name = "Розмір файла")]
        public virtual string FileSizeCount
        {
            get {
                var sizeKb = FileSize / 1000;
                if (sizeKb >= 1)
                    return Math.Round(sizeKb, 2) + " " + "Кбайт";
                else
                    return FileSize + " " + "байт";
            }
            set { }
        }

        //[DisplayFormat(DataFormatString = "{0:HH:mm dd/MM/yyyy}")]
        //[Display(Name = "Дата додавання файлу")]
        //public DateTime? FileDate { get; set; }

        public virtual bool Ock { get; set; }
        [Display(Name = "Тип документа")]
        public virtual string DocumentType { get; set; }

        [Display(Name = "Опис")]
        public virtual string Description { get; set; }
    }
}
