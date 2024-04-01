// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace ExpenseTracker.Web.Models.Transactions.Exceptions
{
    public class TransactionValidationException : Xeption
    {
        public TransactionValidationException(Xeption innerException) :
            base(message:"Transaction Validation Error Occurred, try again.", innerException)
        { }

        public TransactionValidationException(string message, Xeption innerException) :
            base(message: message, innerException)
        { }
    }
}
