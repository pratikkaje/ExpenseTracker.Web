﻿using Xeptions;

namespace ExpenseTracker.Web.Models.Transactions.Exceptions
{
    public class TransactionDependencyException : Xeption
    {
        public TransactionDependencyException(Xeption innerException)
            : base(message: "Transaction dependency error occurred, contact support.", innerException)
        { }

        public TransactionDependencyException(string message, Xeption innerException)
            : base(message: message, innerException)
        { }
    }
}
