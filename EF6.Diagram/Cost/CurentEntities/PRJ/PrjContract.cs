using Astum.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Astum.Core.Data.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Entities.PRJ
{
    public class PrjContract : BaseEntity
    {
        public PrjContract()
        {
            Children = new HashSet<PrjContract>();
        }

        [DisplayName("Посилання на пов’язаний договір")]
        public Guid? ParentId { get; set; }
        public PrjContract Parent { get; set; }
        public ICollection<PrjContract> Children { get; set; }

        public DateTime ContractDate { get; set; }

        public string Number { get; set; }
        
        public string Comment { get; set; }

        //вартiсть
        public double Price { get; set; }

        //статус документа(Договір) з "Класифікатор станів договорів" ContractStatus
        public string ContractStatusEnum { get; set; } //enum

        //Тип договору з "Класифікатор  Тип договору" ContractType
        public string ContractTypeEnum { get; set; } //enum

        //Орг заказчик
        public Guid CustomerId { get; set; } // при сохранени ссылки на OrgUnit, добавляем также в развязку PrjParticipant
 //       public OrgUnit Customer { get; set; }

        //Орг исполнитель
        public Guid ExecutorId { get; set; }
 //         public OrgUnit Executor { get; set; }

        //Підписант заказчик
        public Guid SignerCustomerId { get; set; }
//        public OrgUnitEmployee SignerCustomer { get; set; }

        //Підписант исполнитель
        public Guid SignerExecutorId { get; set; }
//        public OrgUnitEmployee SignerExecutor { get; set; }
        
        //связка с Проекти
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public string ProzorroCode { get; set; }

        public string PurchaseTypeEnum { get; set; }
    }
}
