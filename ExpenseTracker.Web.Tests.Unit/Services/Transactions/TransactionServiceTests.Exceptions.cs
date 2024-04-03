using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.Transactions.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using RESTFulSense.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Tests.Unit.Services.Transactions
{
    public partial class TransactionServiceTests
    {
        [Theory]
        [MemberData(nameof(ValidationApiException))]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfBadRequestErrorOccursAndLogItAsync(
            Exception validationApiException)
        {
            // given
            Transaction someTransaction = CreateRandomTransaction();

            var invalidTransactionException =
                new InvalidTransactionException(
                    message: "Invalid transaction error occurred.",
                    innerException: validationApiException,
                    data: validationApiException.Data);

            var expectedTransactionDependencyValidation =
                new TransactionDependencyValidationException(
                    message: "Transaction dependency validation error occurred, please try again.",
                    innerException: invalidTransactionException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostTransactionAsync(It.IsAny<Transaction>()))
                    .ThrowsAsync(validationApiException);

            // when
            ValueTask<Transaction> addTransactionTask =
                this.transactionService.AddTransactionAsync(someTransaction);

            // then
            await Assert.ThrowsAsync<TransactionDependencyValidationException>(() =>
                addTransactionTask.AsTask());

            this.apiBrokerMock.Verify(broker =>
                broker.PostTransactionAsync(It.IsAny<Transaction>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedTransactionDependencyValidation))),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(CriticalDependencyException))]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfCriticalErrorOccursAndLogItAsync(
            Exception criticalDependencyException)
        {
            // given
            Transaction someTransaction = CreateRandomTransaction();

            var failedTransactionDependencyException =
                new FailedTransactionDependencyException(
                    message: "Failed transaction dependency error occurred, contact support.",
                    innerException: criticalDependencyException);

            var expectedTransactionDependencyException = 
                new TransactionDependencyException(
                    message: "Transaction dependency error occurred, contact support.",
                    innerException: failedTransactionDependencyException);

            this.apiBrokerMock.Setup(broker => 
                broker.PostTransactionAsync(It.IsAny<Transaction>()))
                    .ThrowsAsync(criticalDependencyException);

            // when
            ValueTask<Transaction> addTransactionTask = 
                this.transactionService.AddTransactionAsync(someTransaction);

            TransactionDependencyException actualTransactionDependencyException =
                await Assert.ThrowsAsync<TransactionDependencyException>(() =>
                    addTransactionTask.AsTask());
            // then
            actualTransactionDependencyException.Should()
                .BeEquivalentTo(expectedTransactionDependencyException);

            this.apiBrokerMock.Verify(broker => 
                broker.PostTransactionAsync(It.IsAny<Transaction>()), 
                    Times.Once);

            this.loggingBrokerMock.Verify(broker => 
                broker.LogCritical(It.Is(
                    SameExceptionAs(expectedTransactionDependencyException))), 
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfServerInternalErrorOccursAndLogItAsync()
        {
            // given
            Transaction someTransaction = CreateRandomTransaction();
            string exceptionMessage = GetRandomString();
            var httpResponseMessage = new HttpResponseMessage();

            var httpResponseInternalServerErrorException = 
                new HttpResponseInternalServerErrorException(
                    responseMessage: httpResponseMessage,
                    message: exceptionMessage);

            httpResponseInternalServerErrorException.AddData(exceptionMessage);

            var invalidTransactionException =
                new InvalidTransactionException(
                    message: "Invalid transaction error occurred.",
                    innerException: httpResponseInternalServerErrorException,
                    data: httpResponseInternalServerErrorException.Data);

            var expectedTransactionDependencyException =
                new TransactionDependencyException(httpResponseInternalServerErrorException);

            this.apiBrokerMock.Setup(broker => 
                broker.PostTransactionAsync(It.IsAny<Transaction>()))
                    .ThrowsAsync(httpResponseInternalServerErrorException);

            // when
            ValueTask<Transaction> addTransactionTask = 
                this.transactionService.AddTransactionAsync(someTransaction);

            TransactionDependencyException actualTransactionDependencyException =
                await Assert.ThrowsAsync<TransactionDependencyException>(() => 
                    addTransactionTask.AsTask());

            // then
            actualTransactionDependencyException.Should()
                .BeEquivalentTo(expectedTransactionDependencyException);

            this.apiBrokerMock.Verify(broker => 
                broker.PostTransactionAsync(It.IsAny<Transaction>()), 
                    Times.Once);

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(
                    SameExceptionAs(expectedTransactionDependencyException))), 
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
