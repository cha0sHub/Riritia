using Riritia.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riritia.Communicators.RemotePackager
{
    public interface IRemoteClient
    {
        void QueueMessage(IKalos message);
        bool StartListening();
        void StopListening();
    }
}
