using LoansApi.DataAccess.Enum;

namespace LoansApi.DataAccess.Models
{
    public  class Loan
    {
        public int LoanId {  get; set; }
        public string CustomerName { get; set; }
        public decimal ApplicationAmount { get; set; }
        public DateTime ApplicationDate { get; set; }
        public LoanStatus Status { get; set; }
    }
}