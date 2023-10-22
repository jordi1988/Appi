﻿namespace Infrastructure.SqLite
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Company { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? PostalCode { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? Fax { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
    }
}
