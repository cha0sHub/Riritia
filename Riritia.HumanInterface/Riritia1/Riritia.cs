using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Riritia.Interfaces;

namespace Riritia.HumanInterface.Riritia1
{
    public class Riritia : IHumanInterface
    {
        public string Name => "Riritia";

        private ICollection<IPurpose> Purposes { get; }
        
        private IDictionary<string, ICommunicator> Communicators { get; }

        private IPurposeObserver PurposeObserver { get; }

        public Riritia(IPurposeObserver purposeObserver)
        {
            PurposeObserver = purposeObserver;

            Communicators = new Dictionary<string, ICommunicator>();
        }

        public void AddMind(IPurpose purpose)
        {
            PurposeObserver.AddPurpose(purpose);
        }

        public void AddVoice(ICommunicator communicator)
        {
            if (Communicators.ContainsKey(communicator.Name))
                return;

            Communicators.Add(communicator.Name, communicator);
        }

        public void StartMind(string name)
        {
            PurposeObserver.ActivatePurpose(name);
        }

        public void StopMind(string name)
        {
            PurposeObserver.DeactivatePurpose(name);
        }

        public void StartVoice(string name)
        {
            if (!Communicators.ContainsKey(name) || Communicators[name].Active)
                return;

            Communicators[name].Listen();
            Communicators[name].Active = true;
        }

        public void StopVoice(string name)
        {
            if (!Communicators.ContainsKey(name) || !Communicators[name].Active)
                return;

            Communicators[name].Stop();
            Communicators[name].Active = false;
        }

        public async Task AddMessageToIncomingAsync(IKalos message)
        {
            try
            {
                using (var streamWriter = File.AppendText("../tempChatLog.txt"))
                {
                    streamWriter.WriteLine($"Sender: {message.Sender}|Origin:{message.Origin}|Timestamp:{message.Timestamp}|Message:{message.Msg}");
                }
            }
            catch (Exception e) { }
            var outgoingMessages = await PurposeObserver.ObserveMessageAsync(message);
            foreach (var outMessage in outgoingMessages.Messages)
            {
                if (string.IsNullOrEmpty(outMessage.Target))
                    outMessage.Target = message.Target;
                AddMessageToOutgoing(outMessage);
            }
        }

        public void AddMessageToOutgoing(IKalos message)
        {
            try
            {
                using (var streamWriter = File.AppendText("../tempChatLog.txt"))
                {
                    streamWriter.WriteLine($"Sender: {message.Sender}|Origin:{message.Origin}|Timestamp:{message.Timestamp}|Message:{message.Msg}");
                }
            }
            catch (Exception e)
            { }
            if (!Communicators.ContainsKey(message.Target))
                return;
            Communicators[message.Target].AddMessage(message);
        }
    }
}
