using Riritia.Core;
using Riritia.Interfaces;
using System.Threading.Tasks;
using System.IO;

namespace Riritia.Purposes.HereToObserve
{
    public class HereToObservePurpose : IPurpose
    {
        private const string PurposeName = "HereToObserve";
        private const string Response = "I am here to observe.";

        public string Name => PurposeName;

        public bool Active { get; set; }

        private IHumanInterface HumanInterface { get; }
        private IOvermindAccessor OvermindAccessor { get; }

        public HereToObservePurpose(IHumanInterface humanInterface, IOvermindAccessor overmindAccessor)
        {
            HumanInterface = humanInterface;
            OvermindAccessor = overmindAccessor;
        }

        public async Task<IPurposeFullfillment> WorkAsync(IContext context, IKalos message)
        {
            
            var purposeFullfillment = new PurposeFullfillment();
            if (!context.AddressedToSelf)
            {
                purposeFullfillment.Weight = -1;
                return purposeFullfillment;
            }
            var outgoingMessage = MessageCreator.CreateOutgoingMessage(message.Origin, HumanInterface.Name, Response);
            purposeFullfillment.AddMessage(outgoingMessage);
            purposeFullfillment.Weight = 1;

            return purposeFullfillment;
        }
    }
}
