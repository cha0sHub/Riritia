using System;
using System.Collections.Generic;

namespace Riritia.Interfaces
{
    
    public interface IKalos
    {
        string Cmd { get; }
        Guid Id { get; }
        string Msg { get; }
        string Target { get; set; }
        string Origin { get; }
        string Sender { get; }
        DateTime Timestamp { get; }
    }
}
