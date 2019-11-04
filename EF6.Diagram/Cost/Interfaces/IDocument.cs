using Astum.Core.Data.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Astum.Core.Data.Interfaces
{
    public interface IDocument : IBaseEntity
    {
        [DisplayName("Номер документу")]
        string RegNumber { get; set; }
        DateTime? RegDate { get; set; }
        string Description { get; set; }
    }
}
