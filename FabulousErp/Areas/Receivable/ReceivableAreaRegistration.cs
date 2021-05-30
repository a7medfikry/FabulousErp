using System.Web.Mvc;

namespace FabulousErp.Areas.Receivable
{
    public class ReceivableAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Receivable";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Receivable_default",
                "Receivable/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Receivable.Controllers" }

            );
        }
    }
}