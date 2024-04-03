using System;
using Xeptions;

namespace ExpenseTracker.Web.Models.Transactions.Exceptions
{
    public class FailedTransactionDependencyException : Xeption
    {
        public FailedTransactionDependencyException(Exception innerException)
            : base(message: "Failed transaction dependency error occurred, contact support.", innerException)
        { }

        public FailedTransactionDependencyException(string message, Exception innerException)
            : base(message: message, innerException)
        { }
    }
}
