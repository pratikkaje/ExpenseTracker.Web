// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Brokers.API;
using ExpenseTracker.Web.Brokers.Logging;
using ExpenseTracker.Web.Models.Transactions;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Services.Transactions
{
    public partial class TransactionService : ITransactionService
    {
        private readonly IApiBroker apiBroker;
        private readonly ILoggingBroker loggingBroker;

        public TransactionService(IApiBroker apiBroker, ILoggingBroker loggingBroker)
        {
            this.apiBroker = apiBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Transaction> AddTransactionAsync(Transaction transaction) =>
            TryCatch(async () =>
            {
                ValidateTransaction(transaction);
                return await this.apiBroker.PostTransactionAsync(transaction);
            });
    }
}
