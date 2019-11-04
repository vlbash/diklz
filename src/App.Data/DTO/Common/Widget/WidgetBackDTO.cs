using App.Core.Base;

namespace App.Data.DTO.Common.Widget
{
    public class WidgetBackDTO : CoreDTO
    {
        public int PrlNewApplications { get; set; }
        public int ImlNewApplications { get; set; }
        public int TrlNewApplications { get; set; }

        public int PrlNewMessage { get; set; }
        public int ImlNewMessage { get; set; }
        public int TrlNewMessage { get; set; }

        public int PrlReviewApplication { get; set; }
        public int ImlReviewApplication { get; set; }
        public int TrlReviewApplication { get; set; }

        public int PrlProjectApplication { get; set; }
        public int ImlProjectApplication { get; set; }
        public int TrlProjectApplication { get; set; }
    }
}
