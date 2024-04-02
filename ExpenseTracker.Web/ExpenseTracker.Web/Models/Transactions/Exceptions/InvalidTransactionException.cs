// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace ExpenseTracker.Web.Models.Transactions.Exceptions
{
    public class InvalidTransactionException : Xeption
    {
        public InvalidTransactionException()
            : base(message: "Invalid transaction error occurred.")
        { }

        public InvalidTransactionException(string message)
            : base(message: message)
        { }
    }
}
