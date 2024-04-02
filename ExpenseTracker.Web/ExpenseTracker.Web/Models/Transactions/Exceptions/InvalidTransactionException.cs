using Xeptions;

namespace ExpenseTracker.Web.Models.Transactions.Exceptions
{
    public class InvalidTransactionException : Xeption
    {
        public InvalidTransactionException(string parameterName, object parameterValue)
            : base(message: "Invalid transaction error occurred, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}")
        { }
    }
}
