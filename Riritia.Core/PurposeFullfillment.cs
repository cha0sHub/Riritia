using Riritia.Interfaces;
using System.Collections.Generic;

namespace Riritia.Core
{
    public class PurposeFullfillment : IPurposeFullfillment
    {
        public int Weight { get; set; }

        public IReadOnlyList<IKalos> Messages => (IReadOnlyList<IKalos>)MessagesInternal;
        private IList<IKalos> MessagesInternal { get; }

        public PurposeFullfillment()
        {
            MessagesInternal = new List<IKalos>();
        }

        public void AddMessage(IKalos message)
        {
            MessagesInternal.Add(message);
        }
    }
}
