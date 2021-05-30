using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Important;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FabulousErp.Controllers.API
{
    public class FavouriteAPIController : ApiController
    {

        DBContext DB = new DBContext();

        [HttpPost]
        public HttpResponseMessage AddFavourite([FromUri]string formCode, [FromUri]string checkURL, [FromUri]int type, [FromUri]string formName)
        {
            FavouritesForms_Table favouritesForms_Table = new FavouritesForms_Table()
            {
                FormCode = formCode,

                UserID = FabulousErp.Business.GetUserId(),

                FormURL = checkURL,

                Type = type,

                FormName = formName
            };
            DB.FavouritesForms_Tables.Add(favouritesForms_Table);
            DB.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public HttpResponseMessage RemoveFavourite([FromUri]string formCode)
        {
            string userID = FabulousErp.Business.GetUserId();
            var remove = DB.FavouritesForms_Tables.Where(x => x.UserID == userID && x.FormCode == formCode).ToList();
            if (remove != null)
            {
                DB.FavouritesForms_Tables.RemoveRange(remove);
                DB.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        public HttpResponseMessage GetFavourits()
        {
            string UserId = FabulousErp.Business.GetUserId();
            return Request.CreateResponse(HttpStatusCode.OK, DB.FavouritesForms_Tables.Where(x => x.UserID == UserId).ToList());
        }



        public enum FormType
        {
            Setting = 1,

            Transaction = 2,

            Inquiry = 3,

            Report = 4
        }

    }
}
