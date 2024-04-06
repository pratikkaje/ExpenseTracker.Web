using ExpenseTracker.Web.Brokers.DateTime;
using ExpenseTracker.Web.Brokers.Logging;
using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.Transactions.Exceptions;
using ExpenseTracker.Web.Models.TransactionViews;
using ExpenseTracker.Web.Services.Transactions;
using ExpenseTracker.Web.Services.TransactionViews;
using ExpenseTracker.Web.Services.Users;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;
using Xeptions;

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

        public static TheoryData TransactionServiceValidationException()
        {
            var innerException = new Xeption();

            return new TheoryData<Exception>
            {
                new TransactionValidationException(
                    message: "Transaction Validation Error Occurred, try again.",
                    innerException),

                new TransactionDependencyValidationException(
                    message:"Transaction dependency validation error occurred, please try again.",
                    innerException)
            };
        }

        private Expression<Func<Transaction,bool>> SameTransactionAs(Transaction expectedTransaction) => 
            actualTransaction => this.compareLogic.Compare(expectedTransaction, actualTransaction).AreEqual;

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static decimal GetRandomDecimalValue() =>
            new LongRange(min: 2, max: 2000).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static TransactionView CreateRandomTransactionView() =>
            CreateTransactionViewFiller().Create();

        private static Filler<TransactionView> CreateTransactionViewFiller()
        {
            var filler = new Filler<TransactionView>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow);

            return filler;
        }
    }
}
