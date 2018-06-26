using Riritia.Core;
using Riritia.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riritia.HumanInterface.Riritia1
{
    internal class PurposeObserver : IPurposeObserver
    {
        private const int ConfidenceThreshhold = 1000000;
        private const string DefaultMessage = "I am here to observe.";


        private Dictionary<string,IPurpose> Purposes { get; }
        public PurposeObserver()
        {
            Purposes = new Dictionary<string, IPurpose>();
        }

        public void AddPurpose(IPurpose purpose)
        {
            if (Purposes.ContainsKey(purpose.Name))
                return;
            Purposes.Add(purpose.Name, purpose);
        }

        public void ActivatePurpose(string purposeName)
        {
            if (!Purposes.ContainsKey(purposeName))
                return;
            Purposes[purposeName].Active = true;
        }

        public void DeactivatePurpose(string purposeName)
        {
            if (!Purposes.ContainsKey(purposeName))
                return;
            Purposes[purposeName].Active = false;
        }

        public ICollection<IPurpose> GetPurposes()
        {
            return Purposes.Values;
        }

        public async Task<IPurposeFullfillment> ObserveMessageAsync(IKalos message)
        {
            var activeTasks = new List<Task<IPurposeFullfillment>>();
            foreach (var purpose in Purposes.Where(p => p.Value.Active))
            {
                activeTasks.Add(purpose.Value.WorkAsync(EvaluateContext(message), message));
            }

            var bestScore = 0;
            var returnedMessages = new List<IPurposeFullfillment>();
            while (bestScore < ConfidenceThreshhold && activeTasks.Count > 0)
            {
                var completedTask = await Task.WhenAny(activeTasks);
                activeTasks.Remove(completedTask);
                var returnMessage = await completedTask;
                if (returnMessage.Weight > bestScore)
                    bestScore = returnMessage.Weight;
                returnedMessages.Add(returnMessage);
            }
            return returnedMessages.OrderBy(p => p.Weight).Reverse().FirstOrDefault();
        }

        private static IPurposeFullfillment CreateDefaultResponse()
        {
            var purposeFullfillment = new PurposeFullfillment
            {
                Weight = 0
            };

            return purposeFullfillment;
        }

        private IContext EvaluateContext(IKalos message)
        {
            var context = new Context();
            //for now, just check if message contains "riritia"
            if (message.Msg.ToLowerInvariant().Contains("riritia"))
            {
                context.AddressedToSelf = true;
            }
            return context;
        }
    }
}
