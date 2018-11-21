using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.Text;

namespace Roslyn.Demo
{
    /// <summary>
    /// A rudimentary VB.NET compiler using Roslyn.
    /// </summary>
    public class VBCompiler
    {
        //TODO: eliminate magic string?
        private static string runtimePath = @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\{0}.dll";

        private static readonly IEnumerable<MetadataReference> DefaultReferences = new[]
        {
            MetadataReference.CreateFromFile(string.Format(runtimePath, "mscorlib")),
            MetadataReference.CreateFromFile(string.Format(runtimePath, "System")),
            MetadataReference.CreateFromFile(string.Format(runtimePath, "System.Core")),
            MetadataReference.CreateFromFile(string.Format(runtimePath, "Microsoft.VisualBasic"))
        };

        private static readonly IEnumerable<string> DefaultNamespaces = new[]
        {
            "System",
            "System.IO",
            "System.Net",
            "System.Linq",
            "System.Text",
            "System.Text.RegularExpressions",
            "System.Collections.Generic"
        };

        private static readonly VisualBasicCompilationOptions DefaultCompilationOptions = new VisualBasicCompilationOptions(OutputKind.ConsoleApplication);

        private static SyntaxTree Parse(string text)
        {
            var stringText = SourceText.From(text, Encoding.UTF8);
            return SyntaxFactory.ParseSyntaxTree(stringText);
        }

        public class Frontend
        {
            /// <summary>
            /// Compiles the given VB.NET file into an IL byte array.
            /// </summary>
            public byte[] FromFile(string filename, string assemblyName)
            {
                string source = File.ReadAllText(filename);
                return FromString(source, assemblyName);
            }

            /// <summary>
            /// Compiles the given string of VB.NET source code into an IL byte array.
            /// </summary>
            public byte[] FromString(string code, string assemblyName)
            {
                var parsedSyntaxTree = Parse(code);
                var compilation = VisualBasicCompilation.Create(assemblyName, new SyntaxTree[] { parsedSyntaxTree }, DefaultReferences, DefaultCompilationOptions);

                using (var stream = new MemoryStream())
                {
                    var result = compilation.Emit(stream);
                    if (!result.Success)
                    {
                        throw new CompilationException($"Compilation failed:\n{string.Join("\n", result.Diagnostics)}");
                    }
                    else
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        return stream.ToArray();
                    }
                }
            }
        }

        public Frontend Compile { get; } = new Frontend();
    }
}
