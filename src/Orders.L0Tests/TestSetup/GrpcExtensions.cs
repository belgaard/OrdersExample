using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Orders.L0Tests.TestSetup
{
    public static class GrpcExtensions
    {
        public static GrpcChannel GetGrpcChannel(this WebApplicationFactory<Startup> factory)
        {
            HttpClient client = factory.CreateDefaultClient(new ResponseVersionHandler());
            return GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = client
            });
        }

        private class ResponseVersionHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
                response.Version = request.Version;
                return response;
            }
        }
    }
}