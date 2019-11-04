using App.Core.Base;
using App.Core.Data.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Core.Data.Interfaces
{
    public interface IDocument : ICoreEntity
    {
        [DisplayName("Номер документу")]
        string RegNumber { get; set; }
        DateTime? RegDate { get; set; }
        string Description { get; set; }
    }
}
