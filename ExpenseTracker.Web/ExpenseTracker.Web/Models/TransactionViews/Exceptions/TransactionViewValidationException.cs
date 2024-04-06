using Xeptions;

namespace ExpenseTracker.Web.Models.TransactionViews.Exceptions
{
    public class TransactionViewValidationException : Xeption
    {
        public TransactionViewValidationException(Xeption innerException) :
            base(message: "Transaction view validation error occurred, try again.", innerException)
        { }

        public TransactionViewValidationException(string message, Xeption innerException) :
            base(message: message, innerException)
        { }
    }
}
