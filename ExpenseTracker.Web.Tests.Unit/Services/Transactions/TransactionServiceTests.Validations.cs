// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.Transactions.Exceptions;
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
        public async Task ShouldThrowValidationExceptionOnAddIfTransactionIsNullAndLogItAsync()
        {
            // given
            Transaction invalidTransaction = null;

            var nullTransactionException = 
                new NullTransactionException(
                    message: "Transaction is null."
                    );

            var expectedTransactionValidationException =
                new TransactionValidationException(
                    message: "Transaction Validation Error Occurred, try again.",
                    innerException: nullTransactionException
                    );

            // when
            ValueTask<Transaction> addTransactionTask =
                this.transactionService.AddTransactionAsync(invalidTransaction);

            // then
            TransactionValidationException actualTransactionValidationException =
                await Assert.ThrowsAsync<TransactionValidationException>(() =>
                    addTransactionTask.AsTask());

            actualTransactionValidationException.Should()
                .BeEquivalentTo(expectedTransactionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTransactionValidationException))),
                        Times.Once);

            this.apiBrokerMock.Verify(broker =>
                broker.PostTransactionAsync(It.IsAny<Transaction>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }
    }
}
