// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using RESTFulSense.Clients;
using System.Threading.Tasks;
using System.Transactions;

namespace ExpenseTracker.Web.Brokers.API
{
    public partial class ApiBroker : IApiBroker
    {
        private readonly IRESTFulApiFactoryClient apiClient;

        public ApiBroker(IRESTFulApiFactoryClient apiClient) =>
            this.apiClient = apiClient;

        private async ValueTask<T> GetAsync<T>(string relativeUrl) =>
            await this.apiClient.GetContentAsync<T>(relativeUrl);

        private async ValueTask<T> PostAsync<T>(string relativeUrl, T content) =>
            await this.apiClient.PostContentAsync<T>(relativeUrl, content);

        private async ValueTask<T> PutAsync<T>(string relativeUrl, T content) =>
            await this.apiClient.PutContentAsync<T>(relativeUrl, content);

        private async ValueTask<T> DeleteAsync<T>(string relativeUrl) =>
            await this.apiClient.DeleteContentAsync<T>(relativeUrl);

    }
}
