namespace Riritia.Interfaces
{
    public interface ICommunicator
    {
        string Name { get; set; }
        bool Active { get; set; }
        string Destination { get; set; }
        
        void Listen();
        void AddMessage(IKalos message);
        void Stop();
    }
}
