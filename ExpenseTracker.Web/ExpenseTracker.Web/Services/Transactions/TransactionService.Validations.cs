// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.Transactions.Exceptions;

namespace ExpenseTracker.Web.Services.Transactions
{
    public partial class TransactionService
    {
        private void ValidateTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new NullTransactionException();
            }
        }
    }
}
