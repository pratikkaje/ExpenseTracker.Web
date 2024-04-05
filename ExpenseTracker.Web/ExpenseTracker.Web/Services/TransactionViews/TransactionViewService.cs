using ExpenseTracker.Web.Brokers.DateTime;
using ExpenseTracker.Web.Brokers.Logging;
using ExpenseTracker.Web.Models.TransactionViews;
using ExpenseTracker.Web.Services.Transactions;
using ExpenseTracker.Web.Services.Users;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Services.TransactionViews
{
    public class TransactionViewService : ITransactionViewService
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

        public ValueTask<TransactionView> AddTransactionViewAsync(TransactionView transactionView)
        {
            throw new System.NotImplementedException();
        }
    }
}
