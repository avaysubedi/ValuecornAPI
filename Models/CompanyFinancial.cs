namespace ValuecornAPI.Models
{
    public class CompanyFinancialDto
    {
        public int? FinancialId { get; set; }
        public int CompanyId { get; set; }
        public byte? FiscalYearEnd { get; set; }
        public string? AccountingStandards { get; set; }
        public string? ExternalAuditor { get; set; }
    }

    public class CompanyTaxDto
    {
        public int? TaxId { get; set; }
        public int CompanyId { get; set; }
        public string? VATRegNo { get; set; }
        public DateTime? VATDate { get; set; }
        public string? CorporateTaxRegNo { get; set; }
        public DateTime? CorporateTaxDate { get; set; }
        public string? FreeZoneBenefit { get; set; }
        public string? Exemptions { get; set; }
    }
}
