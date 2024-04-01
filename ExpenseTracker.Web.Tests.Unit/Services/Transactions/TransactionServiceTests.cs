using ExpenseTracker.Web.Brokers.API;
using ExpenseTracker.Web.Brokers.Logging;
using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Services.Transactions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;

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
