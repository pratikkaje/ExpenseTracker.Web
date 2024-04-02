// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.Transactions.Exceptions;
using System;

namespace ExpenseTracker.Web.Services.Transactions
{
    public partial class TransactionService
    {
        private void ValidateTransactionOnAdd(Transaction transaction)
        {
            ValidateTransactionIsNotNull(transaction);

            Validate(
                (Rule: IsInvalid(transaction.Id), Parameter: nameof(Transaction.Id)),
                (Rule: IsInvalid(transaction.UserId), Parameter: nameof(Transaction.UserId)),
                (Rule: IsInvalid(transaction.Category), Parameter: nameof(Transaction.Category)),
                (Rule: IsInvalid(transaction.Description), Parameter: nameof(Transaction.Description)),
                (Rule: IsInvalid(transaction.PaymentMode), Parameter: nameof(Transaction.PaymentMode)),
                (Rule: IsInvalid(transaction.Amount), Parameter: nameof(transaction.Amount))
                );
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

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required."
        };

        private static dynamic IsInvalid(Decimal value) => new
        {
            Condition = value == default,
            Message = "Value is required."
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
