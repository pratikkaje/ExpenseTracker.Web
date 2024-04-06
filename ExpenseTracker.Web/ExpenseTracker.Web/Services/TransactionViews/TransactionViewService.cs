using ExpenseTracker.Web.Brokers.DateTime;
using ExpenseTracker.Web.Brokers.Logging;
using ExpenseTracker.Web.Models.Transactions;
using ExpenseTracker.Web.Models.TransactionViews;
using ExpenseTracker.Web.Services.Transactions;
using ExpenseTracker.Web.Services.Users;
using System;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Services.TransactionViews
{
    public partial class TransactionViewService : ITransactionViewService
    {
        private readonly ITransactionService transactionService;
        private readonly IUserService userService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public TransactionViewService(
            ITransactionService transactionService,
            IUserService userService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.transactionService = transactionService;
            this.userService = userService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<TransactionView> AddTransactionViewAsync(TransactionView transactionView) =>
            TryCatch(async () => {
                ValidateTransactionViewOnAdd(transactionView);
                Transaction transaction = MapToTransactionView(transactionView);
                await this.transactionService.AddTransactionAsync(transaction);

                return transactionView;
            });

        private Transaction MapToTransactionView(TransactionView transactionView)
        {
            Guid currentLoggedInUserId = this.userService.GetCurrentlyLoggedInUser();
            DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTime();

            return new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = currentLoggedInUserId,
                Amount = transactionView.Amount,
                Category = transactionView.Category,
                PaymentMode = transactionView.PaymentMode,
                Description = transactionView.Description,
                TransactionDate = transactionView.TransactionDate,
                CreatedDate = currentDateTime,
                UpdatedDate = currentDateTime,
                CreatedBy = currentLoggedInUserId,
                UpdatedBy = currentLoggedInUserId
            };
        }
    }
}
