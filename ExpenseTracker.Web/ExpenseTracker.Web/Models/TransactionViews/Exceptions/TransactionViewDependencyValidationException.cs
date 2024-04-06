using Xeptions;

namespace ExpenseTracker.Web.Models.TransactionViews.Exceptions
{
    public class TransactionViewDependencyValidationException : Xeption
    {
        public TransactionViewDependencyValidationException(Xeption innerException)
            : base(message: "Transaction dependency validation error occurred, try again.", innerException)
        { }

        public TransactionViewDependencyValidationException(string message, Xeption innerException)
            : base(message: message, innerException)
        { }
    }
}
