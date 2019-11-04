using App.WebApi.SOAP;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace NetWCFApp
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IWebApiService, WebApiService>(),
                Component.For<ICacheDataService, CacheDataService>(),
                Component.For<IMongoService, MongoDbService>()
                );
        }
    }
}
