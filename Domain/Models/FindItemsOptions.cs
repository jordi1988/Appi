namespace Core.Models
{
    public class FindItemsOptions
    {
        public string Query { get; set; } = string.Empty;

        public bool CaseSensitive { get; set; }
    }
}
