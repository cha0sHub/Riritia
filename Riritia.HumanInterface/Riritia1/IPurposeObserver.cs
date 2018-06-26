using System.Collections.Generic;
using System.Threading.Tasks;
using Riritia.Interfaces;

namespace Riritia.HumanInterface.Riritia1
{
    public interface IPurposeObserver
    {
        void ActivatePurpose(string purposeName);
        void AddPurpose(IPurpose purpose);
        void DeactivatePurpose(string purposeName);
        ICollection<IPurpose> GetPurposes();
        Task<IPurposeFullfillment> ObserveMessageAsync(IKalos message);
    }
}