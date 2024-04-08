// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;

namespace ExpenseTracker.Web.Services.Users
{
    public interface IUserService
    {
        Guid GetCurrentlyLoggedInUser();
    }
}
