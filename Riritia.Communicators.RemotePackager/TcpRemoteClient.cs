using System;
using System.Threading;
using System.Net.Sockets;
using Riritia.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Xml.Serialization;

namespace Riritia.Communicators.RemotePackager
{
    public class TcpRemoteClient : IRemoteClient
    {
        private const int FiveSeconds = 5000;
        private TcpClient Client { get; set; }
        private ICommunicator Communicator { get; }
        private IConfiguration Configuration { get; }
        private Thread ListenerThread { get; set; }
        private Thread SenderThread { get; set; }
        private bool IsRunning { get; set; }
        private BlockingCollection<IKalos> OutgoingMessages { get; }
        private CancellationTokenSource TokenSource { get; set; }

        public TcpRemoteClient(ICommunicator communicator, IConfiguration configuration)
        {
            Communicator = communicator;
            Configuration = configuration;

            OutgoingMessages = new BlockingCollection<IKalos>();
        }

        public bool StartListening()
        {
            IsRunning = true;
            Client = new TcpClient();
            Client.Connect(Configuration[SettingNames.TcpAddress], Convert.ToInt32(Configuration[SettingNames.TcpPort]));
            if (!Client.Connected)
                return false;
            ListenerThread = new Thread(Listen);
            ListenerThread.Start();
            SenderThread = new Thread(Send);
            SenderThread.Start();
            return true;
        }


        private void Listen()
        {
            var xmlSerializer = new XmlSerializer(typeof(IKalos));
            while (IsRunning)
            {
            }
        }

        private void Send()
        {
            while (IsRunning)
            {
                var message = OutgoingMessages.Take();
            }
        }

        public void QueueMessage(IKalos message)
        {
            OutgoingMessages.Add(message);
        }

        public void StopListening()
        {
            IsRunning = false;
            OutgoingMessages.
            ListenerThread.Join(FiveSeconds);
        }
    }
}
