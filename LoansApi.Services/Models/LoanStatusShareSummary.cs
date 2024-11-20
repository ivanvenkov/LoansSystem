using LoansApi.DataAccess.Enum;

namespace LoansApi.Services.Models
{
    public class LoanStatusShareSummary
    {
        public string Status {  get; set; }
        public decimal TotalAmount {  get; set; }  
        public double Percentage {  get; set; }
    }
}
