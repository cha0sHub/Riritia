using MQTTnet;
using MQTTnet.Server;
using Riritia.Interfaces;
using System;
using System.Threading.Tasks;

namespace Riritia.Mqtt.Server
{
    internal class RiritiaServer
    {
        private IMqttServer MqttServer { get; set; }

        private IMqttSettings ServerSettings { get; }

        private IHumanInterface Riritia { get; }

        public RiritiaServer(IMqttSettings serverSettings, IHumanInterface riritia)
        {
            ServerSettings = serverSettings;
            Riritia = riritia;
        }

        public async Task Start()
        {
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpointPort(ServerSettings.DefaultPort);
            MqttServer = new MqttFactory().CreateMqttServer();
            MqttServer.ApplicationMessageReceived += MqttServer_ApplicationMessageReceived;
            await MqttServer.StartAsync(optionsBuilder.Build());
        }

        private void MqttServer_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            MqttServer.
        }
    }
}
