using ExpenseTracker.Web.Models.Transactions.Exceptions;
using ExpenseTracker.Web.Models.TransactionViews;
using ExpenseTracker.Web.Models.TransactionViews.Exceptions;
using System.Threading.Tasks;
using Xeptions;

namespace ExpenseTracker.Web.Services.TransactionViews
{
    public partial class TransactionViewService
    {
        private delegate ValueTask<TransactionView> ReturningTransactionViewFunction();

        private async ValueTask<TransactionView> TryCatch(ReturningTransactionViewFunction returningTransactionViewFunction)
        {
            try
            {
                return await returningTransactionViewFunction();
            }
            catch (InvalidTransactionViewException invalidTransactionViewException)
            {
                throw CreateAndLogValidationException(invalidTransactionViewException);
            }
        }

        private TransactionViewValidationException CreateAndLogValidationException(Xeption exception)
        {
            var transactionViewValidationException =
                new TransactionViewValidationException(exception);

            this.loggingBroker.LogError(transactionViewValidationException);

            return transactionViewValidationException;
        }


    }
}
