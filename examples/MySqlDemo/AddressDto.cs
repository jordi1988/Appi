namespace Infrastructure.MySql
{
    public class AddressDto
    {
        public string? Street { get; set; } = string.Empty;
        public int? Number { get; set; }
        public string? PostalCode { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
    }
}
