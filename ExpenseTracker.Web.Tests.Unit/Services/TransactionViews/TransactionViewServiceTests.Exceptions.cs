// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.TransactionViews;
using ExpenseTracker.Web.Models.TransactionViews.Exceptions;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xeptions;

namespace ExpenseTracker.Web.Tests.Unit.Services.TransactionViews
{
    public partial class TransactionViewServiceTests
    {
        [Theory]
        [MemberData(nameof(TransactionServiceValidationException))]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfTransactionValidationErrorOccursAndLogItAsync(
            Xeption transactionServiceValidationException)
        {
            // given
            TransactionView someTransactionView = CreateRandomTransactionView();

            var expectedTransactionViewDependencyValidationException =
                new TransactionViewDependencyValidationException(
                    message: "Transaction dependency validation error occurred, try again.",
                    transactionServiceValidationException);

            this.transactionServiceMock.Setup(service =>
                service.AddTransactionAsync(It.IsAny<Transaction>()))
                    .ThrowsAsync(transactionServiceValidationException);

            // when
            ValueTask<TransactionView> addTransactionTask =
                this.transactionViewService.AddTransactionViewAsync(someTransactionView);

            // then
            var actualTransactionViewDependencyValidationException =
                await Assert.ThrowsAsync<TransactionViewDependencyValidationException>(() =>
                    addTransactionTask.AsTask());

            actualTransactionViewDependencyValidationException.Should()
                .BeEquivalentTo(expectedTransactionViewDependencyValidationException);

            this.userServiceMock.Verify(service =>
                service.GetCurrentlyLoggedInUser(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.transactionServiceMock.Verify(service =>
                service.AddTransactionAsync(It.IsAny<Transaction>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionViewDependencyValidationException))),
                        Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.transactionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(TransactionServiceDependencyException))]
        public async Task ShouldThrowDependencyExceptionOnAddIfTransactionDependencyErrorOccursAndLogItAsync(
            Xeption transactionServiceDependencyException)
        {
            // given
            TransactionView someTransactionView = CreateRandomTransactionView();

            var expectedTransactionViewDependencyException =
                new TransactionViewDependencyException(
                    message: "Transaction view dependency error occurred.",
                    transactionServiceDependencyException);

            this.transactionServiceMock.Setup(service =>
                service.AddTransactionAsync(It.IsAny<Transaction>()))
                    .ThrowsAsync(transactionServiceDependencyException);

            // when
            ValueTask<TransactionView> addTransactionTask =
                this.transactionViewService.AddTransactionViewAsync(someTransactionView);

            // then
            var actualTransactionViewDependencyException =
                await Assert.ThrowsAsync<TransactionViewDependencyException>(() =>
                    addTransactionTask.AsTask());

            actualTransactionViewDependencyException.Should()
                .BeEquivalentTo(expectedTransactionViewDependencyException);

            this.userServiceMock.Verify(service =>
                service.GetCurrentlyLoggedInUser(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.transactionServiceMock.Verify(service =>
                service.AddTransactionAsync(It.IsAny<Transaction>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionViewDependencyException))),
                        Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.transactionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            TransactionView someTransactionView = CreateRandomTransactionView();
            var serviceException = new Exception();

            var expectedTransactionViewServiceException =
                new TransactionViewServiceException(
                    message: "Transaction view service error occurred, contact support.",
                    innerException: serviceException);

            this.userServiceMock.Setup(service =>
                service.GetCurrentlyLoggedInUser())
                    .Throws(serviceException);

            // when
            ValueTask<TransactionView> addTransactionTask =
                this.transactionViewService.AddTransactionViewAsync(someTransactionView);

            // then
            var actualTransactionViewServiceException =
                await Assert.ThrowsAsync<TransactionViewServiceException>(() =>
                    addTransactionTask.AsTask());

            actualTransactionViewServiceException.Should()
                .BeEquivalentTo(expectedTransactionViewServiceException);

            this.userServiceMock.Verify(service =>
                service.GetCurrentlyLoggedInUser(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionViewServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.transactionServiceMock.Verify(service =>
                service.AddTransactionAsync(It.IsAny<Transaction>()),
                    Times.Never);

            this.userServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.transactionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
