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
            catch (NullTransactionViewException nullTransactionViewException)
            {
                throw CreateAndLogValidationException(nullTransactionViewException);
            }
            catch (InvalidTransactionViewException invalidTransactionViewException)
            {
                throw CreateAndLogValidationException(invalidTransactionViewException);
            }
            catch(TransactionValidationException transactionValidationException)
            {
                throw CreateAndLogDependencyValidationException(transactionValidationException);
            }
            catch (TransactionDependencyValidationException transactionDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(transactionDependencyValidationException);
            }
            catch (TransactionDependencyException transactionDependencyException)
            {
                throw CreateAndLogDependencyException(transactionDependencyException);
            }
            catch (TransactionServiceException transactionServiceException)
            {
                throw CreateAndLogDependencyException(transactionServiceException);
            }
        }

        private TransactionViewValidationException CreateAndLogValidationException(Xeption exception)
        {
            var transactionViewValidationException =
                new TransactionViewValidationException(exception);

            this.loggingBroker.LogError(transactionViewValidationException);

            return transactionViewValidationException;
        }

        private TransactionViewDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var transactionViewDependencyValidationException = 
                new TransactionViewDependencyValidationException(exception);

            this.loggingBroker.LogError(transactionViewDependencyValidationException);

            return transactionViewDependencyValidationException;
        }

        private TransactionViewDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var transactionViewDependencyException = 
                new TransactionViewDependencyException(exception);

            this.loggingBroker.LogError(transactionViewDependencyException);

            return transactionViewDependencyException;
        }
    }
}
