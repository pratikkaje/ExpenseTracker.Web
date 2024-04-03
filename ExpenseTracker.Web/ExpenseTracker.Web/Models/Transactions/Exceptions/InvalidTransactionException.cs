// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Collections;
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

        public InvalidTransactionException(Exception innerException)
            : base(message: "Invalid transaction error occurred.", innerException)
        { }

        public InvalidTransactionException(string message, Exception innerException)
            : base(message: message, innerException)
        { }

        public InvalidTransactionException(Exception innerException, IDictionary data)
            : base(message: "Invalid transaction error occurred.", innerException, data)
        { }

        public InvalidTransactionException(string message, Exception innerException, IDictionary data)
            : base(message: message, innerException, data)
        { }
    }
}
