//To Remind us of our Sins

namespace Riritia.HumanInterface
{/*
    public class Riritia0
    {
        private EventWaitHandle inHandle;
        private EventWaitHandle outHandle;
        private Dictionary<String, IKalos> memory;
        public Dictionary<String, IKalos> Memory {
            get { 
                return memory;
            }
        }
        private List<Thread> mind;
        private Dictionary<String, Thread> purposeToMind;
        private List<Thread> voice;
        private Dictionary<String, Thread> communicatorToVoice;
        private List<IKalos> incoming;
        private List<IKalos> outgoing;
        private Dictionary<String,bool> communicatorToActive;
        public Dictionary<String, bool> CommunicatorToActive {
            get {
                return communicatorToActive;
            }
        }
        private Dictionary<String,bool> purposeToActive;
        public Dictionary<String, bool> PurposeToActive {
            get {
                return purposeToActive;
            }
        }
        private Dictionary<String, ICommunicator> communicators;
        private Dictionary<String, IPurpose> purposes;
        public Random rand;
        private RiritiaView view;
        public Riritia0() {
            rand = new Random(DateTime.Now.Millisecond);
            memory = new Dictionary<string, IKalos>();
            mind = new List<Thread>();
            voice = new List<Thread>();
            incoming = new List<Kalos>();
            outgoing = new List<Kalos>();
            inHandle = new ManualResetEvent(false);
            outHandle = new ManualResetEvent(false);
            communicators = new Dictionary<string, ICommunicator>();
            communicatorToActive = new Dictionary<string, bool>();
            purposes = new Dictionary<string, IPurpose>();
            purposeToActive = new Dictionary<string, bool>();
            purposeToMind = new Dictionary<string, Thread>();
            communicatorToVoice = new Dictionary<string, Thread>();
            
        }
        public void addVoice(String cName, ICommunicator c, bool startActive) {
            communicators.Add(cName, c);
            communicatorToActive.Add(cName, startActive);
        }
        public void addMind(String pName, IPurpose p, bool startActive)
        {
            purposes.Add(pName, p);
            purposeToActive.Add(pName, startActive);
        }
        public void runRiritia() {
            foreach (KeyValuePair<String, ICommunicator> c in communicators) {
                if (communicatorToActive[c.Key] == true) {
                    Thread cThread = new Thread(c.Value.run);
                    cThread.Start();
                    voice.Add(cThread);
                    communicatorToVoice.Add(c.Key, cThread);
                }
            }
            foreach (KeyValuePair<String, IPurpose> p in purposes)
            {
                if (purposeToActive[p.Key] == true)
                {
                    Thread pThread = new Thread(p.Value.run);
                    pThread.Start();
                    mind.Add(pThread);
                    purposeToMind.Add(p.Key, pThread);
                }
            }
            view = new RiritiaView(this);
            view.ShowDialog();
        }
        public void startPurpose(String purp) {
            Thread pThread = new Thread(purposes[purp].run);
            pThread.Start();
            mind.Add(pThread);
            purposeToMind.Add(purp, pThread);
            purposeToActive[purp] = true;
        }
        public void stopPurpose(String purp) {
            purposes[purp].stop();
            purposeToMind[purp].Join(5000);
            mind.Remove(purposeToMind[purp]);
            purposeToMind.Remove(purp);
            purposeToActive[purp] = false;
        }
        public void startCommunicator(String comm)
        {
            Thread cThread = new Thread(communicators[comm].run);
            cThread.Start();
            voice.Add(cThread);
            communicatorToVoice.Add(comm, cThread);
            communicatorToActive[comm] = true;
        }
        public void stopCommunicator(String comm)
        {
            communicators[comm].stop();
            communicatorToVoice[comm].Join(5000);
            voice.Remove(communicatorToVoice[comm]);
            communicatorToVoice.Remove(comm);
            communicatorToActive[comm] = false;
        }
        public void addMessageToIncoming(Kalos inc) {
            lock (incoming)
            {
                incoming.Add(inc);
            }
            inHandle.Set();
        }
        public void addMessageToOutgoing(Kalos outg)
        {
            lock (outgoing)
            {
                outgoing.Add(outg);
            }
            outHandle.Set();
        }
        public List<Kalos> waitForIncomingMessages(Purpose p) {
            List<Kalos> messagesToProcess = new List<Kalos>();
            inHandle.WaitOne(100);
            messagesToProcess = getIncomingMessages(p);
            return messagesToProcess;
        }
        public List<Kalos> getIncomingMessages(Purpose p)
        {
            List<Kalos> messagesToProcess = new List<Kalos>();
            lock (incoming)
            {
                List<int> messagesToDelete = new List<int>();
                int i = 0;
                foreach (Kalos msg in incoming) {
                    bool beenProcessed = false;
                    if (msg.Objects.ContainsKey("purposeIDs"))
                    {
                        if(((HashSet<Guid>)msg.Objects["purposeIDs"]).Contains(p.Id))
                            beenProcessed = true;
                    }
                    else { 
                        msg.Objects.Add("purposeIDs", new HashSet<Guid>());
                    }
                    if (msg.ID.Equals(new Guid()) == false && beenProcessed == false) {
                        if(p.CommandsToProcess.Contains(msg.CMD)) {
                            messagesToProcess.Add(msg);
                        }
                        ((HashSet<Guid>)msg.Objects["purposeIDs"]).Add(p.Id);
                        if (((HashSet<Guid>)msg.Objects["purposeIDs"]).Count >= mind.Count) {
                            messagesToDelete.Add(i);
                        }
                    }
                    i++;
                }
                for (int x = messagesToDelete.Count - 1; x >= 0; x--) {
                    incoming.RemoveAt(messagesToDelete[x]);
                }
                if (incoming.Count == 0)
                    inHandle.Reset();
            }
            return messagesToProcess;
        }
        public List<Kalos> waitForOutgoingMessages(Communicator c) {
            List<Kalos> messagesToProcess = new List<Kalos>();
            inHandle.WaitOne(100);
            messagesToProcess = getOutgoingMessages(c);
            return messagesToProcess;
        }
        public List<Kalos> getOutgoingMessages(Communicator c)
        {
            List<Kalos> messagesToProcess = new List<Kalos>();
            lock (outgoing)
            {
                List<int> messagesToDelete = new List<int>();
                int i = 0;
                foreach (Kalos msg in outgoing)
                {
                    String target = "";
                    if (msg.Objects.ContainsKey("Target")) {
                        target = (String)msg.Objects["Target"];
                    }
                    if (target.Equals(c.Destination)) {
                        messagesToProcess.Add(msg);
                        messagesToDelete.Add(i);
                    }
                    i++;
                }
                for (int x = messagesToDelete.Count - 1; x >= 0; x--)
                {
                    outgoing.RemoveAt(messagesToDelete[x]);
                }
                if (outgoing.Count == 0)
                    outHandle.Reset();
            }
            return messagesToProcess;
        }
    }*/
}
