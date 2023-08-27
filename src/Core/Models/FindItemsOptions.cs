namespace Core.Models
{
    public class FindItemsOptions
    {
        public string Query { get; set; } = string.Empty;

        public string? GroupAlias { get; set; }

        public string? SourceAlias { get; set; }

        public bool CaseSensitive { get; set; }
    }
}
