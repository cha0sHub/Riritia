using System.Threading.Tasks;

namespace Riritia.Interfaces
{
    public interface IPurpose
    {
        string Name { get; }
        bool Active { get; set; }
        Task<IPurposeFullfillment> WorkAsync(IContext context, IKalos message);
    }
}
