// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.Transactions.Exceptions;
using System;
using System.Data;

namespace ExpenseTracker.Web.Services.Transactions
{
    public partial class TransactionService
    {
        private void ValidateTransactionOnAdd(Transaction transaction)
        {
            ValidateTransactionIsNotNull(transaction);

            //Validate(
            //    (Rule: IsInvalid(transaction.Id), Parameter: nameof(Transaction.Id)),

            //    );
        }

        private static void ValidateTransactionIsNotNull(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new NullTransactionException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required."
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidTransactionException = 
                new InvalidTransactionException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTransactionException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTransactionException.ThrowIfContainsErrors();
        }
    }
}
