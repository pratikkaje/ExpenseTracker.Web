using Xeptions;

namespace ExpenseTracker.Web.Models.Transactions.Exceptions
{
    public class TransactionDependencyValidationException : Xeption
    {
        public TransactionDependencyValidationException(Xeption innerException)
            : base(message: "Transaction dependency validation error occurred, please try again.",
                  innerException)
        { }

        public TransactionDependencyValidationException(string message, Xeption innerException)
            : base(message: message, innerException)
        { }
    }
}
