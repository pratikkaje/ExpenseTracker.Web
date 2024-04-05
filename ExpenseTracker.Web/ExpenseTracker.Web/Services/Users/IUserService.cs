using System;

namespace ExpenseTracker.Web.Services.Users
{
    public interface IUserService
    {
        Guid GetCurrentlyLoggedInUser();
    }
}
