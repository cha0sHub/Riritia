using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Riritia.Interfaces
{
    public interface IArururor : ISerializable
    {
        IDictionary<string, IArururor> SubState { get; }
    }
}
