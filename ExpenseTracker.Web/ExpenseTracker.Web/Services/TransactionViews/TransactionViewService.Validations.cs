﻿using ExpenseTracker.Web.Models.Transactions.Exceptions;
using ExpenseTracker.Web.Models.TransactionViews;
using ExpenseTracker.Web.Models.TransactionViews.Exceptions;
using System;

namespace ExpenseTracker.Web.Services.TransactionViews
{
    public partial class TransactionViewService
    {
        public void ValidateTransactionViewOnAdd(TransactionView transactionView)
        {
            Validate(
            (Rule: IsInvalid(transactionView.Category), Parameter: nameof(transactionView.Category)),
            (Rule: IsInvalid(transactionView.Description), Parameter: nameof(transactionView.Description)),
            (Rule: IsInvalid(transactionView.PaymentMode), Parameter: nameof(transactionView.PaymentMode)),
            (Rule: IsInvalid(transactionView.Amount), Parameter: nameof(transactionView.Amount))
                );
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required."
        };

        private static dynamic IsInvalid(decimal value) => new
        {
            Condition = value == default,
            Message = "Value is required."
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidTransactionViewException = new InvalidTransactionViewException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTransactionViewException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTransactionViewException.ThrowIfContainsErrors();
        }

    }
}
