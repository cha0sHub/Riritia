using Riritia.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riritia.Communicators.RemotePackager
{
    public class RiritiaProxy : IHumanInterface
    {
        public string Name => "Riritia Proxy";

        private IDictionary<string, ICommunicator> Communicators { get; }
        private IRemoteClient RemoteClient { get; }

        public RiritiaProxy(IRemoteClient remoteClient)
        {
            RemoteClient = remoteClient;
            Communicators = new Dictionary<string, ICommunicator>();
        }

        public Task AddMessageToIncomingAsync(IKalos message)
        {
            throw new NotImplementedException();
        }

        public void AddMessageToOutgoing(IKalos message)
        {
            throw new NotImplementedException();
        }

        public void AddMind(IPurpose purpose)
        {
            // Unused
        }

        public void AddVoice(ICommunicator communicator)
        {

        }

        public void StartMind(string name)
        {
            // Unused
        }

        public void StartVoice(string name)
        {
            throw new NotImplementedException();
        }

        public void StopMind(string name)
        {
            // Unused
        }

        public void StopVoice(string name)
        {
            throw new NotImplementedException();
        }
    }
}
