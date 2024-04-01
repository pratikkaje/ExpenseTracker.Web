using Xeptions;

namespace ExpenseTracker.Web.Models.Transactions.Exceptions
{
    public class NullTransactionException : Xeption
    {
        public NullTransactionException()
            : base(message: "Transaction is null.")
        { }

        public NullTransactionException(string message)
            : base(message)
        { }
    }
}
