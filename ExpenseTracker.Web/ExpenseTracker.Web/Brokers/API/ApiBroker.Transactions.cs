// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.Transactions;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Brokers.API
{
    public partial class ApiBroker
    {
        private const string relativeUrl = "api/transactions";
        public async ValueTask<Transaction> PostTransactionAsync(Transaction transaction) =>
            await PostAsync<Transaction>(relativeUrl, transaction);
    }
}
