using App.Core.Eq.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Eq.Entities.Schedule
{
    [Table("HolidaysDictionary")]
    public class HolidaysDictionary : EqBaseEntity
    {
        [DisplayName("Назва свята")]
        public string Name { get; set; }

        [DisplayName("Дата святкування")]
        public DateTime HolidayDate { get; set; }
    }
}
