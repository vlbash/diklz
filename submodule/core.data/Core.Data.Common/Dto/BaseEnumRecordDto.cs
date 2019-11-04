using Core.Base.Data;

namespace Core.Data.Common.Dto
{
    public abstract class BaseEnumRecordDto: BaseDto
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual RecordState RecordState { get; set; }

        public virtual string EnumType { get; set; }

        public virtual string ExParam1 { get; set; }

        public virtual string ExParam2 { get; set; }
    }
}
