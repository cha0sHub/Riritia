using Riritia.Interfaces;

namespace Riritia.Core
{
    public class Context : IContext
    {
        public bool AddressedToSelf { get; set; }
        public string CurrentGame { get; set; }

        public Context()
        {
            AddressedToSelf = false;
            CurrentGame = string.Empty;
        }
    }
}
