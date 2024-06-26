﻿// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.Transactions.Exceptions;
using RESTFulSense.Exceptions;
using System;
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
            catch (HttpResponseConflictException httpResponseConflictException)
            {
                var invalidTransactionException =
                    new InvalidTransactionException(
                        innerException: httpResponseConflictException,
                        data: httpResponseConflictException.Data);

                throw CreateAndLogDependencyValidationException(invalidTransactionException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidTransactionException =
                    new InvalidTransactionException(
                        innerException: httpResponseBadRequestException,
                        data: httpResponseBadRequestException.Data);

                throw CreateAndLogDependencyValidationException(invalidTransactionException);
            }
            catch (HttpResponseInternalServerErrorException httpResponseInternalServerErrorException)
            {
                var invalidTransactionException =
                    new InvalidTransactionException(httpResponseInternalServerErrorException);

                throw CreateAndLogDependencyException(invalidTransactionException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var invalidTransactionException =
                    new InvalidTransactionException(httpResponseException);

                throw CreateAndLogDependencyException(invalidTransactionException);
            }
            catch (Exception exception)
            {
                var failedTransactionServiceException =
                    new FailedTransactionServiceException(exception);

                throw CreateAndLogServiceException(failedTransactionServiceException);
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

        private TransactionDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var transactionDependencyException =
                new TransactionDependencyException(exception);

            this.loggingBroker.LogError(transactionDependencyException);

            return transactionDependencyException;
        }

        private TransactionServiceException CreateAndLogServiceException(Xeption exception)
        {
            var transactionServiceException =
                new TransactionServiceException(exception);

            this.loggingBroker.LogError(transactionServiceException);

            return transactionServiceException;
        }
    }
}
