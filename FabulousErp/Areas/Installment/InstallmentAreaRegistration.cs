using System.Web.Mvc;

namespace FabulousErp.Areas.Installment
{
    public class InstallmentAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Installment";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Installment_default",
                "Installment/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Installment.Controllers" }
            );
        }
    }
}