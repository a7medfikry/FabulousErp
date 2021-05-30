using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Settings.Financial.GeneralSettings.UGFormAccess
{
    public class UserGroupAccess_DTO
    {
        public Names Names { get; set; }

        public List<Forms> Forms { get; set; }
    }

    public class Names
    {
        public string Name { get; set; }
    }

    public class Forms
    {
        public string FormName { get; set; }
    }
}
