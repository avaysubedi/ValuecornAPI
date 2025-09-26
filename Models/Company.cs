namespace ValuecornAPI.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string? TradeName { get; set; }
        public DateTime? DateOfIncorporation { get; set; }
        public string? CountryCode { get; set; }
        public string? LegalStructure { get; set; }
        public string? BusinessLicenseNumber { get; set; }
        public DateTime? LicenseExpiryDate { get; set; }
        public string? OfficialPhone { get; set; }
        public string? OfficialEmail { get; set; }
        public string? Website { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CompanyDto
    {
        public int? CompanyId { get; set; }   // NULL for insert, value for update
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string? TradeName { get; set; }
        public DateTime? DateOfIncorporation { get; set; }
        public string? CountryCode { get; set; }
        public string? LegalStructure { get; set; }
        public string? BusinessLicenseNumber { get; set; }
        public DateTime? LicenseExpiryDate { get; set; }
        public string? OfficialPhone { get; set; }
        public string? OfficialEmail { get; set; }
        public string? Website { get; set; }
        public string? IndustryCode { get; set; }  // ✅ Added
        public string? Currency { get; set; }  // ✅ Added


    }

}
