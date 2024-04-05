using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.TransactionViews;
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
        public async Task ShouldAddTransactionViewAsync()
        {
            // given
            Guid randomUserId = Guid.NewGuid();
            DateTimeOffset randomDateTime = GetRandomDateTime();

            dynamic randomTransactionViewProperties =
                CreateRandomTransactionViewProperties(
                    auditDates: randomDateTime,
                    auditIds: randomUserId);

            var randomTransactionView = new TransactionView
            {
                Amount = randomTransactionViewProperties.Amount,
                Category = randomTransactionViewProperties.Category,
                Description = randomTransactionViewProperties.Description,
                PaymentMode = randomTransactionViewProperties.PaymentMode,
                TransactionDate = randomTransactionViewProperties.TransactionDate
            };

            TransactionView inputTransactionView = randomTransactionView;
            TransactionView expectedTransactionView = inputTransactionView;

            var randomTransaction = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Category = randomTransactionViewProperties.Category,
                PaymentMode = randomTransactionViewProperties.PaymentMode,
                Description = randomTransactionViewProperties.Description,
                Amount = randomTransactionViewProperties.Amount,
                TransactionDate = randomTransactionViewProperties.TransactionDate,
                CreatedDate = randomDateTime,
                UpdatedDate = randomDateTime,
                CreatedBy = randomUserId,
                UpdatedBy = randomUserId
            };

            Transaction expectedInputTransaction = randomTransaction;
            Transaction returnedTransaction = expectedInputTransaction;

            this.userServiceMock.Setup(service => 
                service.GetCurrentlyLoggedInUser())
                    .Returns(randomUserId);

            this.dateTimeBrokerMock.Setup(broker => 
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            this.transactionServiceMock.Setup(service => 
                service.AddTransactionAsync(It.Is(
                    SameTransactionAs(expectedInputTransaction))))
                        .ReturnsAsync(returnedTransaction);

            // when
            var actualTransactionView = 
                await this.transactionViewService.AddTransactionViewAsync(inputTransactionView);

            // then
            actualTransactionView.Should()
                .BeEquivalentTo(expectedTransactionView);

            this.userServiceMock.Verify(service =>
                service.GetCurrentlyLoggedInUser(), 
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), 
                    Times.Once);

            this.transactionServiceMock.Verify(service =>
                service.AddTransactionAsync(It.Is(
                    SameTransactionAs(expectedInputTransaction))), 
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
            this.transactionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
