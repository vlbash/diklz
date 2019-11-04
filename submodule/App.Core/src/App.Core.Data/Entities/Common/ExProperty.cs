using System.ComponentModel.DataAnnotations;

namespace App.Core.Data.Entities.Common
{
    [Display(Name = "Додане поле")]
    public class ExProperty : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// checkbox, number, text, etc
        /// </summary>
        public string PropTypeEnum { get; set; }

        /// <summary>
        /// Name of object that extended like ConstructionObject
        /// </summary>
        public string Group { get; set; }
        public string KindEnum { get; set; }

        public string SortOrder { get; set; }

        public string COTypeEnum { get; set; }
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
