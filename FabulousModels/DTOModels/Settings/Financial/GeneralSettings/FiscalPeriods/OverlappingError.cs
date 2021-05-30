using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Settings.Financial.GeneralSettings.FiscalPeriods
{
    public class OverlappingError
    {
        public string Message { get; set; }
        
        public string Start { get; set; }

        public string End { get; set; }

        public string StartAdj { get; set; }

        //public string EndAdj { get; set; }
    }
}
