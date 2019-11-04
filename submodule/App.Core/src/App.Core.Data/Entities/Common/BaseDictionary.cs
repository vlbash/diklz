using System.ComponentModel;

namespace App.Core.Data.Entities.Common
{
    /// <summary>
    /// base entity for dictionary tables
    /// </summary>
    public class BaseDictionary: BaseEntity
    {
        [DisplayName("Назва")]
        public string Name { get; set; }
        [DisplayName("Код")]
        public string Code { get; set; }
        [DisplayName("Опис")]
        public string Description { get; set; }
    }
}
