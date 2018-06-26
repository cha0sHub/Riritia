using Microsoft.Extensions.Configuration;
using Ninject.Modules;
using Riritia.IdseConnection;
using Riritia.Interfaces;
using Serilog;
using System.IO;

namespace Riritia.HumanInterface.Riritia1
{
    internal class Bindings : NinjectModule
    {
        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }
        
        public Bindings()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            var log = new LoggerConfiguration().WriteTo.Console().WriteTo.File("log.txt");
            Logger = log.CreateLogger();
        }
        public override void Load()
        {
            var purposeObserver = new PurposeObserver();
            Bind<IPurposeObserver>().ToConstant(purposeObserver);
            Bind<IHumanInterface>().ToConstant(new Riritia(purposeObserver));
            Bind<IConfiguration>().ToConstant(Configuration);
            Bind<ILogger>().ToConstant(Logger);
            Bind<IOvermindAccessor>().To<IdseDataContext>();
        }
    }
}
