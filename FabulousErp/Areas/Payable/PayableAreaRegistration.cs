using System.Web.Mvc;

namespace FabulousErp.Areas.Payable
{
    public class PayableAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Payable";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Payable_default",
                "Payable/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Payable.Controllers" }

            );
        }
    }
}