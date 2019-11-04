using Astum.Core.Data.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Astum.Core.Data.Interfaces
{
    public interface IBaseEntity : IEntity
    {
        RecordState RecordState { get; set; }
        string Caption { get; set; }
        string ModifiedBy { get; set; }
        string ModifiedBy_Id { get; set; }
        DateTime? ModifiedOn { get; set; }
        string CreatedBy { get; set; }
        string CreatedBy_Id { get; set; }
        DateTime CreatedOn { get; set; }
    }
}