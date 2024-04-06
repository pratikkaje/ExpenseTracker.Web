using System;
using Xeptions;

namespace ExpenseTracker.Web.Models.TransactionViews.Exceptions
{
    public class TransactionViewServiceException : Xeption
    {
        public TransactionViewServiceException(Exception innerException)
            : base(message: "Transaction view service error occurred, contact support.", innerException)
        { }

        public TransactionViewServiceException(string message,Exception innerException)
            : base(message: message, innerException)
        { }
    }
}
