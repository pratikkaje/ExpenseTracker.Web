// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Brokers.API;
using ExpenseTracker.Web.Brokers.Logging;
using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Services.Transactions;
using Moq;
using RESTFulSense.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using Tynamix.ObjectFiller;
using Xeptions;

namespace ExpenseTracker.Web.Tests.Unit.Services.Transactions
{
    public partial class TransactionServiceTests
    {
        private readonly Mock<IApiBroker> apiBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITransactionService transactionService;

        public TransactionServiceTests()
        {
            this.apiBrokerMock = new Mock<IApiBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.transactionService = new TransactionService(
                apiBroker: this.apiBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object
                );
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static Dictionary<String, List<string>> CreateRandomDictionary()
        {
            var filler = new Filler<Dictionary<string, List<string>>>();

            return filler.Create();
        }

        public static TheoryData CriticalDependencyException()
        {
            string exceptionMessage = GetRandomMessage();
            var httpResponseMessage = new HttpResponseMessage();

            var httpRequestException = new HttpRequestException();

            var httpResponseUrlNotFoundException =
                new HttpResponseUrlNotFoundException(
                    responseMessage: httpResponseMessage,
                    message: exceptionMessage);

            var httpResponseUnauthorizedException = 
                new HttpResponseUnauthorizedException(
                    responseMessage: httpResponseMessage,
                    message: exceptionMessage);

            return new TheoryData<Exception>
            {
                httpRequestException,
                httpResponseUrlNotFoundException,
                httpResponseUnauthorizedException
            };

        }

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static Transaction CreateRandomTransaction() =>
            CreateTransactionFiller().Create();

        private static Filler<Transaction> CreateTransactionFiller()
        {
            DateTimeOffset date = DateTimeOffset.UtcNow;

            var filler = new Filler<Transaction>();

            filler.Setup().OnType<DateTimeOffset>().Use(date);

            return filler;
        }
    }
}
