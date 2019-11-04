using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Astum.Core.Data.Enums
{
    public enum FileType //Тип вмісту
    {
        [Display(Name = "Unknown")]
        Unknown = 0,
        [Display(Name = "Docx")]
        Docx = 1,
        [Display(Name = "Pdf")]
        Pdf = 2,
        [Display(Name = "Xlsx")]
        Xlsx = 3,
        [Display(Name = "Img")]
        Img = 4,
        [Display(Name = "Txt")]
        Txt = 5,
        [Display(Name = "Csv")]
        Csv = 6
    }
}
