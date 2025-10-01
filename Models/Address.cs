namespace ValuecornAPI.Models
{
    public class CompanyAddress
    {
        public int? AddressId { get; set; }
        public int CompanyId { get; set; }
        public string AddressType { get; set; } = string.Empty;
        public string? AddressLine { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? POBox { get; set; }
        public string? CountryCode { get; set; }
        public int? UserId { get; set; }
    }
}
