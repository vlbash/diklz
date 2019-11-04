using System;
using System.Collections.Generic;
using System.Text;
using App.Core.Base;

namespace App.Core.Data.DTO.Common
{
    public class EnumRecordDto: BaseDTO
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public RecordState RecordState { get; set; }

        public string EnumType { get; set; }

        public string ExParam1 { get; set; }

        public string ExParam2 { get; set; }
    }
}
