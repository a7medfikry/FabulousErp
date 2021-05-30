using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Settings.Financial.GeneralSettings.UserGroup
{
    public class UserGroup_DTO
    {
        public R_Group_Info_DTO R_Group_Info_DTO { get; set; }

        public List<UserInfo> UserInfos { get; set; }
    }

    public class R_Group_Info_DTO
    {
        public string GroupName { get; set; }

        public string CreationGroupDate { get; set; }

        public int FromCBF { get; set; }

        public bool? Disactive { get; set; }

        //public string Message { get; set; }


        public string GroupID { get; set; }


        public string GetGName { get; set; }
    }

    public class UserInfo
    {
        public string UserID { get; set; }

        public string UserName { get; set; }

        public string Date { get; set; }

    }
}
