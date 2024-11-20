using LoansApi.DataAccess.Enum;
using LoansApi.DataAccess.Models;

namespace LoansApi.Services.Models
{
    public class LoanWithInvoicesResponse
    {
        public int LoanId { get; set; }
        public string CustomerName { get; set; }
        public decimal ApplicationAmount { get; set; }
        public DateTime ApplicationDate { get; set; }
        public LoanStatus Status { get; set; }
        public List<Invoice> Invoices { get; set; }
    }
}
