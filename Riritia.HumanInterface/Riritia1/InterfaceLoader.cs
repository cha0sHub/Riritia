using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Riritia.Interfaces;
using Ninject;

namespace Riritia.HumanInterface.Riritia1
{
    public static class InterfaceLoader
    {

        public static IHumanInterface GetHumanInterfaceInstance()
        {
            var bindings = new Bindings();
            using (var kernel = new StandardKernel(bindings))
            {
                var pluginDirectory = bindings.Configuration[ConfigurationStrings.PluginDirectory];

                if (string.IsNullOrEmpty(pluginDirectory))
                    return null;

                var humanInterface = kernel.Get<IHumanInterface>();
                foreach (var file in Directory.EnumerateFiles(pluginDirectory).Where(file => file.EndsWith(".dll")))
                {
                    try
                    {
                        var assembly = Assembly.LoadFile(file);
                        foreach (var type in assembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof(IPurpose))))
                        {
                            kernel.Bind<IPurpose>().To(type);
                            var purpose = kernel.Get<IPurpose>();
                            humanInterface.AddMind(purpose);
                            kernel.Unbind<IPurpose>();
                        }
                        foreach (var type in assembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof(ICommunicator))))
                        {
                            kernel.Bind<ICommunicator>().To(type);
                            var communicator = kernel.Get<ICommunicator>();
                            humanInterface.AddVoice(communicator);
                            kernel.Unbind<ICommunicator>();
                        }
                            
                    }
                    catch (Exception e)
                    {
                        bindings.Logger.Error(e, "Error occurred while loading plugin.");
                    }
                }
                return humanInterface;
            }
        }
    }
}
