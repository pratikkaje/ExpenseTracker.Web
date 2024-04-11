// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using ExpenseTracker.Web.Brokers.API;
using ExpenseTracker.Web.Brokers.DateTime;
using ExpenseTracker.Web.Brokers.Logging;
using ExpenseTracker.Web.Models.Configurations;
using ExpenseTracker.Web.Services.Transactions;
using ExpenseTracker.Web.Services.TransactionViews;
using ExpenseTracker.Web.Services.Users;
using ExpenseTracker.Web.Views;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RESTFulSense.Clients;
using Syncfusion.Blazor;
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Syncfusion.Licensing.SyncfusionLicenseProvider
            .RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCekx3TXxbf1x0ZFRHalhYTnReUiweQnxTdEFjXX5fcXVRT2JdVEJzWw==");


        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();           

        builder.Services.AddSyncfusionBlazor();

        AddHttpClient(builder);

        builder.Services
            .AddScoped<IApiBroker, ApiBroker>()
            .AddScoped<ILogger, Logger<LoggingBroker>>()
            .AddScoped<ILoggingBroker, LoggingBroker>()
            .AddScoped<IDateTimeBroker, DateTimeBroker>();

        builder.Services
            .AddScoped<ITransactionService, TransactionService>()
            .AddScoped<ITransactionViewService, TransactionViewService>()
            .AddScoped<IUserService, UserService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(ExpenseTracker.Web.Client._Imports).Assembly);

        app.Run();
    }

    private static void AddHttpClient(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient<IRESTFulApiFactoryClient, RESTFulApiFactoryClient>(client =>
        {
            LocalConfigurations localConfigurations = builder.Configuration.Get<LocalConfigurations>();
            string apiUrl = localConfigurations.ApiConfigurations.Url;
            client.BaseAddress = new Uri(apiUrl);
        });
    }
}