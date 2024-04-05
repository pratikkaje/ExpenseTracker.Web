using System;

namespace ExpenseTracker.Web.Services.Users
{
    public class UserService : IUserService
    {
        public Guid GetCurrentlyLoggedInUser() =>
            Guid.NewGuid();
    }
}
