using System;
using System.IO;
using System.Reflection;

namespace Tbl.TestDouble.TestData
{
    /// <summary>Reading test data from embedded Json files.</summary>
    /// <remarks>We are reusing test files from the simulator as those test files double as built-in test data.</remarks>
    public class TestData
    {
        public string this[string path] => ReadTestDataFromFile(path);
        public string this[Type type, string fileName] => ReadTestDataFromFile($"{type.Name}.{fileName}");

        /// <summary>We cannot use .resx files with referenced files with dotnet build (only msbuild which has problems with .Net Core),
        /// so we embed test files individually instead and retrieve them with this method.</summary>
        /// <exception cref="FileNotFoundException">Thrown if the resource is not found.</exception>
        private static string ReadTestDataFromFile(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"Iit.PricingAsAService.Simulator.TestData.{path}";
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new FileNotFoundException(
                    $"Resource {resourceName} not found in {assembly.FullName}. Valid resources are: {string.Join(", ", assembly.GetManifestResourceNames())}.");
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
