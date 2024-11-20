namespace LoansApi.Services.Models
{
    public class LoanStatusShareResponse
    {
        public IEnumerable<LoanStatusShareSummary> LoanSharesByStatus { get; set; }
        public string Errors { get; set; }
    }
}
