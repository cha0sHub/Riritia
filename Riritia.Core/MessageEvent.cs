using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riritia.Core
{
    public class MessageEvent : Event
    {
        Entity participant;
        Entity Participant {
            get {
                return participant;
            }
        }
        String message;
        public String Message {
            get {
                return message;
            }
        }
        public MessageEvent(Entity p, String m) {
            participant = p;
            message = m;
        }
    }
}
