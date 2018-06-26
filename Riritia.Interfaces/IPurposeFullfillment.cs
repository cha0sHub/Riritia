using System.Collections.Generic;

namespace Riritia.Interfaces
{
    public interface IPurposeFullfillment
    {
        int Weight { get; }
        IReadOnlyList<IKalos> Messages { get; }
    }
}
