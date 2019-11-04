using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Common.Models
{
    [Display(Name = "Додане поле")]
    [Table("ExProperty")]
    public abstract class BaseExProperty : BaseEntity
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }

        /// <summary>
        /// checkbox, number, text, etc
        /// </summary>
        public virtual string PropTypeEnum { get; set; }

        /// <summary>
        /// Name of object that extended like ConstructionObject
        /// </summary>
        public virtual string Group { get; set; }
        public virtual string KindEnum { get; set; }

        public virtual string SortOrder { get; set; }

        public virtual string CoTypeEnum { get; set; }
    }
}

//----Для ConstructionObject та ProjectConstructionObject-----//
//Для додаткових влативлстей обектів будівництва, залежно від типа наступні параметри : 
//Group - для ConstructionObject = ConstructionObject, для ProjectConstructionObject = ProjectCObject, 
//Name - назва поля, яка буде відображатися на представленні
//Code - назва поля в коді
//PropTypeEnum - heckbox, number, text і тд
//KindEnum - якщо поле це selectlist то вказується тип Enum, з якого будуть обиратись значення            
//COTypeEnum - вид об'єкта до якого належить ця характеристика(7-дороги, 1 - будівлі і тд, див ObjectKindEnum)
//SortOrder поле для сортування цих полей на представленні
