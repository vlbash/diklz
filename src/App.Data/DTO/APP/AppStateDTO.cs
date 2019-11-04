using System;
using System.Collections.Generic;
using System.Text;
using App.Core.Data.DTO.Common;

namespace App.Data.DTO.APP
{
    public class AppStateDTO: BaseDTO
    {
        public string BackOfficeAppState { get; set; }
        public string AppState { get; set; }
        public string AppSort { get; set; }
        public string AppType { get; set; }
        public string PerformerOfExpertise { get; set; }
    }
}
