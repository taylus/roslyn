using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;

namespace Roslyn.Demo
{
    /// <summary>
    /// Finds and returns default references to use when compiling assemblies.
    /// </summary>
    public static class ReferenceResolver
    {
        public static string RuntimePath => RuntimeEnvironment.GetRuntimeDirectory();

        public static IEnumerable<MetadataReference> GetDefaultReferences()
        {
            var references = new string[]
            {
                "mscorlib.dll",
                "System.dll",
                "System.Core.dll",
                "System.Linq.dll",
                "System.Console.dll",
                "System.Runtime.dll",
                "System.Private.CoreLib.dll",
                "Microsoft.CSharp.dll",
                "Microsoft.VisualBasic.dll"
            };

            return references.Select(r => MetadataReference.CreateFromFile(RuntimePath + r));
        }
    }
}
