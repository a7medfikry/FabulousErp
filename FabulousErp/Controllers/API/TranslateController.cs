using FabulousDB.DB_Context;
using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace FabulousErp.Controllers.API
{
    public class TranslateController : ApiController
    {
        [OutputCache(VaryByParam ="*", Duration = int.MaxValue,Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        [CustomCompression]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Translate([FromBody]List<WordTag> Keys,[FromUri] Langs Language = Langs.Arabic)
        {
            List<WordTag> Trans = new List<WordTag>();
            if (Keys == null)
            {
                Keys = new List<WordTag>();
            }
            using (DBContext db = new DBContext())
            {
                if (Language == Langs.Arabic)
                {
                    var T = db.Translates.ToList().Where(x => Keys.Any(z => z.Word == x.Key)).Select(x => new ReturnWordTag { Txt = x.Arabic, Key = x.Key,Tag="" }).ToList();
                    foreach (ReturnWordTag i in T)
                    {
                        i.Tag = Keys.FirstOrDefault(z => z.Word == i.Key).Tag;
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, T);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, Keys);
        }

    }
    public class WordTag
    {
        public string Word { get; set; }
        public string Tag { get; set; }
    }
    public class ReturnWordTag
    {
        public string Txt { get; set; }
        public string Key { get; set; }
        public string Tag { get; set; }
    }
}
