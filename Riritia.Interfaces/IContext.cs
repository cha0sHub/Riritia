using System;
using System.Collections.Generic;
using System.Text;

namespace Riritia.Interfaces
{
    public interface IContext
    {
        bool AddressedToSelf { get; set; }
        string CurrentGame { get; set; }
    }
}
