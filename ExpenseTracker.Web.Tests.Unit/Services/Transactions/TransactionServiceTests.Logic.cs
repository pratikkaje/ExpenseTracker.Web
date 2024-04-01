using ExpenseTracker.Web.Models.Transactions;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Tests.Unit.Services.Transactions
{
    public partial class TransactionServiceTests
    {
        [Fact]
        public async Task ShouldAddTransactionAsync()
        {
            // Given
            Transaction randomTransaction = CreateRandomTransaction();
            Transaction inputTransaction = randomTransaction;
            Transaction retrievedTransaction = inputTransaction;
            Transaction expectedTransaction = retrievedTransaction;

            this.apiBrokerMock.Setup(broker => 
                broker.PostTransactionAsync(inputTransaction))
                    .ReturnsAsync(retrievedTransaction);
            
            // When
            Transaction actualTransaction = 
                await this.transactionService.AddTransactionAsync(inputTransaction);

            // Then
            actualTransaction.Should().BeEquivalentTo(expectedTransaction);

            this.apiBrokerMock.Verify(broker =>
                broker.PostTransactionAsync(inputTransaction),
                    Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
