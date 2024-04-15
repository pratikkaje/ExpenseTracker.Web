// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace ExpenseTracker.Web.Models.TransactionViews.Exceptions
{
    public class FailedTransactionViewDependencyValidationException : Xeption
    {
        public FailedTransactionViewDependencyValidationException(Xeption innerException)
            : base(message: "Failed transaction view dependency error occurred, try again.", innerException)
        { }

        public FailedTransactionViewDependencyValidationException(string message, Xeption innerException)
            : base(message: message, innerException)
        { }
    }
}
