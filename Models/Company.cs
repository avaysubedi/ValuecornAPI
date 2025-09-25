namespace ValuecornAPI.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CountryCode { get; set; }
        public string IndustryCode { get; set; }
        public string Currency { get; set; }
        public byte? FiscalYearEnd { get; set; }
        public decimal? SharesOutstanding { get; set; }
        public decimal? NetDebt { get; set; }
        public decimal? CashBalance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
