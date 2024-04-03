using Xeptions;

namespace ExpenseTracker.Web.Models.Transactions.Exceptions
{
    public class TransactionServiceException : Xeption
    {
        public TransactionServiceException(Xeption innerException)
            : base(message: "Transaction service error occured.", innerException)
        { }

        public TransactionServiceException(string message, Xeption innerException)
             : base(message: message, innerException)
        { }
    }
}
