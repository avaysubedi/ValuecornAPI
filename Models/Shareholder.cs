namespace ValuecornAPI.Models
{
    public class ShareholderDto
    {
        public string ShareholderName { get; set; } = string.Empty;
        public string? Nationality { get; set; }
        public decimal? Percentage { get; set; }
        public bool? IsUBO { get; set; }
        public string? UBOId { get; set; }
        public string? Type { get; set; }   // Individual / Corporate
    }

    public class ShareholderTempDto : ShareholderDto
    {
        public int TempId { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
