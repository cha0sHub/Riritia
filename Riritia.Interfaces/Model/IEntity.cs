using System;

namespace Riritia.Interfaces.Model
{
    public interface IEntity
    {
        Guid Id { get; }
        string Name { get; }
    }
}
