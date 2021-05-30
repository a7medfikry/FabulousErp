﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace FabulousErp
{
    public static class JavascriptExtension
    {
        public static MvcHtmlString IncludeVersionedJs(this HtmlHelper helper, string filename)
        {
            string version = GetVersion(helper, filename);
            return MvcHtmlString.Create("<script type='text/javascript' src='" + filename + version + "'></script>");
        } 
        public static MvcHtmlString IncludeVersionedCss(this HtmlHelper helper, string filename)
        {
            string version = GetVersion(helper, filename);
            return MvcHtmlString.Create("<link rel='stylesheet' href='" + filename + version + "' />");
        }

        private static string GetVersion(this HtmlHelper helper, string filename)
        {
            var context = helper.ViewContext.RequestContext.HttpContext;

          //  if (context.Cache[filename] == null)
            {
                if (filename.Contains("?"))
                {
                    filename = filename.Split('?')[0];
                }
                var physicalPath = context.Server.MapPath(filename);
                var version = $"?v={new System.IO.FileInfo(physicalPath).LastWriteTime.ToString("MMddHHmmss")}";
                //context.Cache.Add(filename, version, null,
                //  DateTime.Now.AddMinutes(5), TimeSpan.Zero,
                //  CacheItemPriority.Normal, null);
                return version;
            }
            //else
            //{
            //    return context.Cache[filename] as string;
            //}
        }
    }

}