using System;
using Core.Base.Data;

namespace Core.Data.Common.Models
{
    public interface IPerson : ICoreEntity
    {
        string Name { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string Location { get; set; }
        DateTime Birthday { get; set; }
        string GenderEnum { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        string UserId { get; set; }
        string UserName { get; set; }
        string Photo { get; set; }
        string Description { get; set; }
        string IPN { get; set; }
        bool NoIPN { get; set; }
        string IdentityDocumentTypeEnum { get; set; }
        string DocPrefix { get; set; }
        string PersonalDocumentNumber { get; set; }
        string PersonalDocumentAuthority { get; set; }
        DateTime? DocumentIssueDate { get; set; }
        DateTime? ExpirationDate { get; set; }
    }
}
