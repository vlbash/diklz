using System;
using Castle.Facilities.WcfIntegration;
using Castle.Windsor;

namespace NetWCFApp
{
    public class Global: System.Web.HttpApplication
    {
        static IWindsorContainer container;

        protected void Application_Start(object sender, EventArgs e)
        {
            container = new WindsorContainer();
            container.AddFacility<WcfFacility>();
            container.Install(new WindsorInstaller());
        }
    }
}
