// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.Transactions;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Brokers.API
{
    public partial interface IApiBroker
    {
        ValueTask<Transaction> PostTransactionAsync(Transaction transaction);
    }
}
