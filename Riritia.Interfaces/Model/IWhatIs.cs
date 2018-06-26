namespace Riritia.Interfaces.Model
{
    public interface IWhatIs
    {
        string Context { get; }
        string Subject { get; }
        string Object { get; }
        string Relation { get; }
        string Answer { get; }
    }
}
