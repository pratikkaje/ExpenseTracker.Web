using Xeptions;

namespace ExpenseTracker.Web.Models.TransactionViews.Exceptions
{
    public class InvalidTransactionViewException : Xeption
    {
        public InvalidTransactionViewException()
            : base(message: "Invalid transaction view error occured, try again.")
        { }

        public InvalidTransactionViewException(string message)
            : base(message: message)
        { }
    }
}
