using System;
using Riritia.Interfaces;

namespace Riritia.Core
{
    [Serializable]
    public class Kalos : IKalos
    {
        public int Weight { get; set; }

        public string Cmd { get; set; }

        public Guid Id { get; }

        public string Msg { get; set; }

        public string Target { get; set; }

        public string Origin { get; set; }

        public string Sender { get; set; }
        public DateTime Timestamp { get; set; }

        public Kalos(String cmd, String msg) {
            Id = Guid.NewGuid();
            Cmd = cmd;
            Msg = msg;
            Timestamp = DateTime.Now;
        }
    }
}
