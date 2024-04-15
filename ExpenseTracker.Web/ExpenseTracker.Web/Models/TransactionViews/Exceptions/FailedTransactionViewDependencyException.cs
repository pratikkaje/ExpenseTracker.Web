// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace ExpenseTracker.Web.Models.TransactionViews.Exceptions
{
    public class FailedTransactionViewDependencyException : Xeption
    {
        public FailedTransactionViewDependencyException(Xeption innerException) 
            : base(message:"Failed transaction view dependency error occurred, contact support.", innerException)
        { }

        public FailedTransactionViewDependencyException(string message, Xeption innerException)
            : base(message: message, innerException)
        { }
    }
}
