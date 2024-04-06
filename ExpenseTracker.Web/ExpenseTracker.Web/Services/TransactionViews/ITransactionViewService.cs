// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Models.TransactionViews;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Services.TransactionViews
{
    public interface ITransactionViewService
    {
        ValueTask<TransactionView> AddTransactionViewAsync(TransactionView transactionView);
    }
}
