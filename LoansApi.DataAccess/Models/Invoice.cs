namespace LoansApi.DataAccess.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string Number { get; set; }
        public int Amount { get; set; }
        public int LoanId { get; set; }
    }
}
