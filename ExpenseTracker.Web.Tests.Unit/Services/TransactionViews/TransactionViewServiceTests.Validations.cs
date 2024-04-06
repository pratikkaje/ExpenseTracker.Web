using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.TransactionViews;
using ExpenseTracker.Web.Models.TransactionViews.Exceptions;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Tests.Unit.Services.TransactionViews
{
    public partial class TransactionViewServiceTests
    {
        [Fact]
        public async Task ShouldthrowValidationExceptionOnAddIfTransactionViewIsNullAndLogItAsync()
        {
            // given
            TransactionView nullTransactionView = null;

            var nullTransactionViewException =
                new NullTransactionViewException(message: "Null transaction view error occurred.");

            var expectedTransactionViewValidationException = 
                new TransactionViewValidationException(nullTransactionViewException);

            // when
            ValueTask<TransactionView> addTransactionViewTask = 
                this.transactionViewService.AddTransactionViewAsync(nullTransactionView);

            // then
            var actualTransactionViewValidation = 
                await Assert.ThrowsAsync<TransactionViewValidationException>(() => 
                    addTransactionViewTask.AsTask());

            actualTransactionViewValidation.Should()
                .BeEquivalentTo(expectedTransactionViewValidationException);

            this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(
                    SameExceptionAs(expectedTransactionViewValidationException))), 
                        Times.Once);

            this.userServiceMock.Verify(service => 
                service.GetCurrentlyLoggedInUser(), 
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker => 
                broker.GetCurrentDateTime(), 
                    Times.Never);

            this.transactionServiceMock.Verify(service => 
                service.AddTransactionAsync(It.IsAny<Transaction>()), 
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.transactionServiceMock.VerifyNoOtherCalls();
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task ShouldThrowValidationExceptionOnAddIfTransactionViewIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            TransactionView invalidTransactionView = new TransactionView
            {
                Category = invalidText,
                PaymentMode = invalidText,
                Description = invalidText
            };

            var invalidTransactionViewException = 
                new InvalidTransactionViewException(message: "Invalid transaction view error occured, try again.");

            invalidTransactionViewException.AddData(
                key: nameof(TransactionView.Category), 
                values: "Text is required.");

            invalidTransactionViewException.AddData(
                key: nameof(TransactionView.PaymentMode),
                values: "Text is required.");

            invalidTransactionViewException.AddData(
                key: nameof(TransactionView.Description),
                values: "Text is required.");

            invalidTransactionViewException.AddData(
                key: nameof(TransactionView.Amount),
                values: "Value is required.");

            var expectedTransactionViewValidationException = 
                new TransactionViewValidationException(invalidTransactionViewException);

            // when
            ValueTask<TransactionView> addTransactionViewTask = 
                this.transactionViewService.AddTransactionViewAsync(invalidTransactionView);

            // then
            var actualTransactionViewException = 
                await Assert.ThrowsAsync<TransactionViewValidationException>(() => 
                    addTransactionViewTask.AsTask());

            actualTransactionViewException.Should()
                .BeEquivalentTo(expectedTransactionViewValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedTransactionViewValidationException))),
                        Times.Once);

            this.userServiceMock.Verify(service => 
                service.GetCurrentlyLoggedInUser(), 
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker => 
                broker.GetCurrentDateTime(), 
                    Times.Never);

            this.transactionServiceMock.Verify(service => 
                service.AddTransactionAsync(
                    It.IsAny<Transaction>()), 
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.transactionServiceMock.VerifyNoOtherCalls();
        }
    }
}
