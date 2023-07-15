namespace Domain.Exceptions
{
    public class SourceNotFoundException : DomainException
    {
        public SourceNotFoundException(string sourceName)
            : base($"The source `{sourceName}` could not be found.")
        {
        }
    }
}
