// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using Xeptions;

namespace ExpenseTracker.Web.Models.TransactionViews.Exceptions
{
    public class FailedTransactionViewServiceException : Xeption
    {
        public FailedTransactionViewServiceException(Exception innerException)
            : base(message: "Transaction view service failure occurred, contact support.", innerException)
        { }

        public FailedTransactionViewServiceException(string message, Exception innerException)
            : base(message: message, innerException)
        { }
    }
}
