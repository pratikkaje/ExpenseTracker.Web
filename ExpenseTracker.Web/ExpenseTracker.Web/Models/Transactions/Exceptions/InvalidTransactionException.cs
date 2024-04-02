using Xeptions;

namespace ExpenseTracker.Web.Models.Transactions.Exceptions
{
    public class InvalidTransactionException : Xeption
    {
        public InvalidTransactionException()
            : base(message: "Invalid transaction error occurred.")
        { }

        public InvalidTransactionException(string message)
            : base(message: message)
        { }
    }
}
