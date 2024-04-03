using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.Transactions.Exceptions;
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
        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfBadRequestErrorOccursAndLogItAsync()
        {
            // given
            Transaction someTransaction = CreateRandomTransaction();
            string exceptionMessage = GetRandomString();
            IDictionary randomDictionary = CreateRandomDictionary();
            IDictionary exceptionData = randomDictionary;
            var httpResponseMessage = new HttpResponseMessage();
            
            var httpResponseBadRequestException = 
                new HttpResponseBadRequestException(
                    responseMessage: httpResponseMessage,
                    message: exceptionMessage);

            httpResponseBadRequestException.AddData(exceptionData);

            var invalidTransactionException = 
                new InvalidTransactionException(
                    message: "Invalid transaction error occurred.",
                    innerException: httpResponseBadRequestException,
                    data: exceptionData);

            var expectedTransactionDependencyValidation =
                new TransactionDependencyValidationException(
                    message: "Transaction dependency validation error occurred, please try again.",
                    innerException: invalidTransactionException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostTransactionAsync(It.IsAny<Transaction>()))
                    .ThrowsAsync(httpResponseBadRequestException);

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
    }
}
