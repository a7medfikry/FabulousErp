using FabulousErp.Repository.Business;
using FabulousErp.Repository.Interface;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace FabulousErp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IRepetitionBusiness, RSelectBusiness>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}