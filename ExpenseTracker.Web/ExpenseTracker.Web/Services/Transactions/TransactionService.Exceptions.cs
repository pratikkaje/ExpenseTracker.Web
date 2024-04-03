// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.Transactions.Exceptions;
using RESTFulSense.Exceptions;
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
            catch (InvalidTransactionException invalidTransactionException)
            {
                throw CreateAndLogValidationException(invalidTransactionException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidTransactionException =
                    new InvalidTransactionException(
                        innerException: httpResponseBadRequestException,
                        data: httpResponseBadRequestException.Data);

                throw CreateAndLogDependencyValidationException(invalidTransactionException);
            }
        }

        private TransactionValidationException CreateAndLogValidationException(Xeption exception)
        {
            var transactionValidationException =
                new TransactionValidationException(exception);

            this.loggingBroker.LogError(transactionValidationException);

            return transactionValidationException;
        }

        private TransactionDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var transactionDependencyValidationException =
                new TransactionDependencyValidationException(exception);

            this.loggingBroker.LogError(transactionDependencyValidationException);

            return transactionDependencyValidationException;
        }
    }
}
