// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace ExpenseTracker.Web.Models.TransactionViews.Exceptions
{
    public class TransactionViewDependencyException : Xeption
    {
        public TransactionViewDependencyException(Xeption innerException)
            : base(message: "Transaction view dependency error occurred.", innerException)
        { }

        public TransactionViewDependencyException(string message, Xeption innerException)
            : base(message: message, innerException)
        { }
    }
}
