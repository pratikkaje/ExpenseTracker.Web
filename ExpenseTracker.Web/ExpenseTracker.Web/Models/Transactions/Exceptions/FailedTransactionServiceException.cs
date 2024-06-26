﻿// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using Xeptions;

namespace ExpenseTracker.Web.Models.Transactions.Exceptions
{
    public class FailedTransactionServiceException : Xeption
    {
        public FailedTransactionServiceException(Exception innerException)
            : base(message: "Failed transaction service error occured.", innerException)
        { }

        public FailedTransactionServiceException(string message, Exception innerException)
            : base(message: message, innerException)
        { }
    }
}
