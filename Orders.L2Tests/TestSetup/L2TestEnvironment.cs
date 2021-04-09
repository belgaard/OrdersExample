using System.Net;
using System.Net.Http;
using LeanTest.Xunit;
using Orders.L2Tests.TestSetup;

[assembly: AssemblyFixture(typeof(AssemblyInitializer))]
[assembly: Xunit.TestFramework("LeanTest.Xunit.XunitExtensions.XunitTestFrameworkWithAssemblyFixture", "LeanTest.Xunit")]

namespace Orders.L2Tests.TestSetup
{
    public class L2TestEnvironment
    {
        public L2TestEnvironment()
        {
            HttpClient httpClient = new(new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.GZip,
                UseCookies = false,
                DefaultProxyCredentials = CredentialCache.DefaultNetworkCredentials
            });
            HttpClient = httpClient;
        }

        /// <summary>
        /// A HttpClient configured to prefix requests that uses a relative path with <see cref="P:Iit.OpenApi.TestClient.L2TestEnvironment.BaseUrl" />.
        /// </summary>
        public HttpClient HttpClient { get; }
    }
}
