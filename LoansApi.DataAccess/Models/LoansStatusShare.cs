using LoansApi.DataAccess.Enum;

namespace LoansApi.DataAccess.Models
{
    public class LoansStatusShare
    {
        public LoanStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
