using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.TransactionViews;
using ExpenseTracker.Web.Models.TransactionViews.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
