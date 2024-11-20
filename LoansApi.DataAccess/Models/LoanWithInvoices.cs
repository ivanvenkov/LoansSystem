namespace LoansApi.DataAccess.Models
{
    public class LoanWithInvoices  : Loan
    {
        public List<Invoice> Invoices { get; set; } = new();
    }
}
