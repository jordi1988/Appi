namespace Core.Abstractions
{
    public interface ISourcesSelector
    {
        string QueryWithinDescription { get; }

        IEnumerable<ISource> GetSources();
    }
}
