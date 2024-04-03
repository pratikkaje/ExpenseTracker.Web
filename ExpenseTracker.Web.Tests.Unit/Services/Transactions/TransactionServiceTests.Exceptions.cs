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

        [Theory]
        [MemberData(nameof(DependencyApiExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync(
            Exception dependencyApiExceptions)
        {
            // given
            Transaction someTransaction = CreateRandomTransaction();

            var invalidTransactionException =
                new InvalidTransactionException(
                    message: "Invalid transaction error occurred.",
                    innerException: dependencyApiExceptions);

            var expectedTransactionDependencyException =
                new TransactionDependencyException(invalidTransactionException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostTransactionAsync(It.IsAny<Transaction>()))
                    .ThrowsAsync(dependencyApiExceptions);

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

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Transaction someTransaction = CreateRandomTransaction();
            Exception serviceException = new Exception();

            var failedTransactionServiceException =
                new FailedTransactionServiceException(
                    message: "Failed transaction service error occured.",
                    innerException: serviceException);

            var expectedTransactionServiceException = 
                new TransactionServiceException(failedTransactionServiceException);

            this.apiBrokerMock.Setup(broker => 
                broker.PostTransactionAsync(It.IsAny<Transaction>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Transaction> addTransactionTask = 
                this.transactionService.AddTransactionAsync(someTransaction);

            // then
            TransactionServiceException actualTransactionServiceException = 
                await Assert.ThrowsAsync<TransactionServiceException>(() => 
                    addTransactionTask.AsTask());

            actualTransactionServiceException.Should()
                .BeEquivalentTo(expectedTransactionServiceException);

            this.apiBrokerMock.Verify(broker => 
                broker.PostTransactionAsync(It.IsAny<Transaction>()), 
                    Times.Once);

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(
                    SameExceptionAs(expectedTransactionServiceException))), 
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
