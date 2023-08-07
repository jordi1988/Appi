namespace Core.Exceptions
{
    public class SourceNotFoundException : CoreException
    {
        public SourceNotFoundException(string sourceName)
            : base($"The source `{sourceName}` could not be found.")
        {
        }
    }
}
