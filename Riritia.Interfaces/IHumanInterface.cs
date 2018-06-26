using System.Threading.Tasks;

namespace Riritia.Interfaces
{
    public interface IHumanInterface
    {
        string Name { get; }
        void AddMind(IPurpose purpose);
        void AddVoice(ICommunicator communicator);
        void StartMind(string name);
        void StopMind(string name);
        void StartVoice(string name);
        void StopVoice(string name);
        Task AddMessageToIncomingAsync(IKalos message);
        void AddMessageToOutgoing(IKalos message);
    }
}
