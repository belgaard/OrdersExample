using System;
using System.IO;
using System.Reflection;

namespace Orders.L2Tests.TestData
{
    public class TestData
    {
        public string this[string path] => ReadTestDataFromFile(path);

        /// <summary>We cannot use .resx files with referenced files with dotnet build (only msbuild which has problems with .Net Core),
        /// so we embed test files individually instead and retrieve them with this method.</summary>
        private static string ReadTestDataFromFile(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"Orders.L2Tests.TestData.{path}.json";
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new Exception(
                    $"Resource {resourceName} not found in {assembly.FullName}.  Valid resources are: {string.Join(", ", assembly.GetManifestResourceNames())}.");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}