// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.Transactions;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Services.Transactions
{
    public interface ITransactionService
    {
        ValueTask<Transaction> AddTransactionAsync(Transaction transaction);
    }
}
