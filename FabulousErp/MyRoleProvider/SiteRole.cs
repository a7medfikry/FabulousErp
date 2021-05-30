using FabulousDB.DB_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FabulousErp.MyRoleProvider
{
    public class SiteRole : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string NoImpUser)
        {
            string UserID = FabulousErp.Business.GetUserId();
            DBContext DB = new DBContext();

            if (UserID == "SA")
            {
                string[] result1 = new string[] { "SCL", "SCNA", "SUAP", "SUACP" };

                return result1;
            }
            else
            {
                string[] result = DB.UserFormsAccess_Tables.Where(x => x.UserID == UserID).Select(x => x.FormCode).ToArray();
                return result;
            }
        }
        public static bool UserHasRole(string FormCode)
        {
            DBContext DB = new DBContext();
            string UserId = FabulousErp.Business.GetUserId();
            if (UserId=="SA"&&(FormCode== "SCNA"||FormCode== "SCNG"||FormCode== "SCL"
                ||FormCode== "SUACP"|| FormCode == "SUAP"))
            {
                return true;
            }
            return DB.UserFormsAccess_Tables.Any(x => x.UserID == UserId
            && x.FormCode == FormCode);
        }
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}