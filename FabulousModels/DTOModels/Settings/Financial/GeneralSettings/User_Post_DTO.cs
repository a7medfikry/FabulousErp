using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Settings.Financial.GeneralSettings
{
    public class User_Post_DTO
    {
        public int ID { get; set; }

        public string UserID { get; set; }

        public string FormCode { get; set; }



        public string PostingType { get; set; }

        public string CreateJEPer { get; set; }

        public string Batch { get; set; }

        public string PostingDataFrom { get; set; }

        public string ExistingBatch { get; set; }

        public string EditPostingDate { get; set; }



        public bool Ask { get; set; }

        public bool PrintDirect { get; set; }

        public bool Analytic { get; set; }

        public bool CostCenter { get; set; }

    }
}
