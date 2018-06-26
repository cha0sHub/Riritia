using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riritia.Core
{
    public class Conversation : Event
    {
        Guid id;
        public Guid ID {
            get {
                return id;
            }
        }
        DateTime lastMessage;
        public DateTime LastMessage {
            get {
                return lastMessage;
            }
        }
        List<Event> dialogue;
        List<String> expectedCMDs;
        public List<String> ExpectedCMDs {
            get {
                return expectedCMDs;
            }
        }
        Entity participant;
        Entity Participant {
            get {
                return participant;
            }
        }
        public Conversation(Entity p, String expectedCMD) {
            dialogue = new List<Event>();
            participant = new Entity("???");
            lastMessage = DateTime.Now;
            id = Guid.NewGuid();
            expectedCMDs = new List<string>();
            expectedCMDs.Add(expectedCMD);
        }
        public bool isExpectedCMD(String cmd) {
            return expectedCMDs.Contains(cmd);
        }
        public void addEvent(Event e) {
            dialogue.Add(e);
            lastMessage = DateTime.Now;
        }
        public void setExpectedCMDs(List<String> exp) {
            expectedCMDs.Clear();
            foreach (String s in exp)
                expectedCMDs.Add(s);
        }
    }
}
