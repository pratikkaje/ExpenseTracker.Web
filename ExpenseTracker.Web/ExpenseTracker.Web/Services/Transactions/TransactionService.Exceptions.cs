// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.Transactions.Exceptions;
using System.Threading.Tasks;
using Xeptions;

namespace ExpenseTracker.Web.Services.Transactions
{
    public partial class TransactionService
    {
        private delegate ValueTask<Transaction> ReturningTransactionFunction();

        private async ValueTask<Transaction> TryCatch(ReturningTransactionFunction returningTransactionFunction)
        {
            try
            {
                return await returningTransactionFunction();
            }
            catch (NullTransactionException nullTransactionException)
            {
                throw CreateAndLogValidationException(nullTransactionException);
            }
        }

        private TransactionValidationException CreateAndLogValidationException(Xeption exception)
        {
            var transactionValidationException =
                new TransactionValidationException(exception);

            this.loggingBroker.LogError(transactionValidationException);

            return transactionValidationException;
        }

    }
}
