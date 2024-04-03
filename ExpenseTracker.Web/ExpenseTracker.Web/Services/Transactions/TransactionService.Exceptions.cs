// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.Transactions.Exceptions;
using RESTFulSense.Exceptions;
using System.Net.Http;
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
            catch (HttpRequestException httpRequestException)
            {
                var failedTransactionDependencyException = 
                    new FailedTransactionDependencyException(httpRequestException);

                throw CreateAndLogCriticalDependencyException(failedTransactionDependencyException);
            }
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException)
            {
                var failedTransactionDependencyException =
                    new FailedTransactionDependencyException(httpResponseUrlNotFoundException);

                throw CreateAndLogCriticalDependencyException(failedTransactionDependencyException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var failedTransactionDependencyException =
                    new FailedTransactionDependencyException(httpResponseUnauthorizedException);

                throw CreateAndLogCriticalDependencyException(failedTransactionDependencyException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidTransactionException =
                    new InvalidTransactionException(
                        innerException: httpResponseBadRequestException,
                        data: httpResponseBadRequestException.Data);

                throw CreateAndLogDependencyValidationException(invalidTransactionException);
            }
            catch (HttpResponseConflictException httpResponseConflictException)
            {
                var invalidTransactionException =
                    new InvalidTransactionException(
                        innerException: httpResponseConflictException,
                        data: httpResponseConflictException.Data);

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

        private TransactionDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var transactionDependencyException = 
                new TransactionDependencyException(exception);

            this.loggingBroker.LogCritical(transactionDependencyException);

            return transactionDependencyException;
        }

    }
}
