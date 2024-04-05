using ExpenseTracker.Web.Brokers.DateTime;
using ExpenseTracker.Web.Brokers.Logging;
using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Services.Transactions;
using ExpenseTracker.Web.Services.TransactionViews;
using ExpenseTracker.Web.Services.Users;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;

namespace ExpenseTracker.Web.Tests.Unit.Services.TransactionViews
{
    public partial class TransactionViewServiceTests
    {
        private readonly Mock<ITransactionService> transactionServiceMock;
        private readonly Mock<IUserService> userServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly ITransactionViewService transactionViewService;

        public TransactionViewServiceTests()
        {
            this.transactionServiceMock = new Mock<ITransactionService>();
            this.userServiceMock = new Mock<IUserService>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            var compareconfig = new ComparisonConfig();
            compareconfig.IgnoreProperty<Transaction>(transaction => transaction.Id);
            compareconfig.IgnoreProperty<Transaction>(transaction => transaction.UserId);
            this.compareLogic = new CompareLogic(compareconfig);

            this.transactionViewService = new TransactionViewService(
                transactionService: this.transactionServiceMock.Object,
                userService: this.userServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static dynamic CreateRandomTransactionViewProperties(
            DateTimeOffset auditDates, Guid auditIds)
        {
            return new
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Category = GetRandomString(),
                PaymentMode = GetRandomString(),
                Description = GetRandomString(),
                Amount = GetRandomDecimalValue(),
                TransactionDate = GetRandomDateTime(),
                CreatedDate = auditDates,
                UpdatedDate = auditDates,
                CreatedBy = auditIds,
                UpdatedBy = auditIds
            };
        }

        private Expression<Func<Transaction,bool>> SameTransactionAs(Transaction expectedTransaction) => 
            actualTransaction => this.compareLogic.Compare(expectedTransaction, actualTransaction).AreEqual;            

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static decimal GetRandomDecimalValue() =>
            new LongRange(min: 2, max: 2000).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

    }
}
