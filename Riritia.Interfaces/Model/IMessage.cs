using System;

namespace Riritia.Interfaces.Model
{
    public interface IMessage
    {
        Guid Id { get; }
        Guid EntityId { get; }
        Guid CommunicatorId { get; }
        DateTime Timestamp { get; }
        string Text { get; }
    }
}
