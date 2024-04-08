// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace ExpenseTracker.Web.Models.TransactionViews.Exceptions
{
    public class NullTransactionViewException : Xeption
    {
        public NullTransactionViewException()
            : base(message: "Null transaction view error occurred.")
        { }

        public NullTransactionViewException(string message)
            : base(message: message)
        { }
    }
}
