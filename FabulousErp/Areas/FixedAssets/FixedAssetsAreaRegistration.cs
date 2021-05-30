using System.Web.Mvc;

namespace FabulousErp.Areas.FixedAssets
{
    public class FixedAssetsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FixedAssets";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "FixedAssets_area_default",
                "FixedAssets/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "FixedAssets.Controllers" }

            );
        }
    }
}