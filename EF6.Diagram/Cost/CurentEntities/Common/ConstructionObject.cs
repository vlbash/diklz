using App.Data.Entities.PRJ;
using Astum.Core.Data.Entities.Common;
using System.Collections.Generic;

namespace App.Data.Entities.Common
{
    public class ConstructionObject : BaseEntity
    {
        //Назва об'єкта
        public string Name { get; set; }
        
        public string Code { get; set; }

        //Вид будывельних об'єктів з "Нумератор видів будівельних об'єктів" ObjectKind
        //будівлі, дороги
        public string ObjectKindEnum { get; set; } //enum

        //public string PropertiesJson { get; set; }

        //связка 
        public List<ProjectCObject> ProjectCObject { get; set; }
    }
}
