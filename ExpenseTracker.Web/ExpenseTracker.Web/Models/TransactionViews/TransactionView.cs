using System;

namespace ExpenseTracker.Web.Models.TransactionViews
{
    public class TransactionView
    {
        public string Category { get; set; }
        public string PaymentMode { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}
