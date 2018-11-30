using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Roslyn.Demo
{
    /// <summary>
    /// A rudimentary C# compiler using Roslyn.
    /// </summary>
    /// <see cref="https://stackoverflow.com/a/32770961/7512368"/>
    /// <see cref="http://www.tugberkugurlu.com/archive/compiling-c-sharp-code-into-memory-and-executing-it-with-roslyn"/>
    public class CSharpCompiler
    {
        private static readonly CSharpCompilationOptions DefaultCompilationOptions = new CSharpCompilationOptions(OutputKind.ConsoleApplication);

        private static SyntaxTree Parse(string text)
        {
            var stringText = SourceText.From(text, Encoding.UTF8);
            return SyntaxFactory.ParseSyntaxTree(stringText);
        }

        private static Compilation CompileAssembly(SyntaxTree syntaxTree, string assemblyName, IEnumerable<MetadataReference> additionalReferences = null)
        {
            var references = ReferenceResolver.GetDefaultReferences().Concat(additionalReferences ?? Enumerable.Empty<MetadataReference>());
            return CSharpCompilation.Create(assemblyName, new SyntaxTree[] { syntaxTree }, references, DefaultCompilationOptions);
        }

        public CompilerFrontend Compile { get; } = new CompilerFrontend() { Parse = Parse, Compile = CompileAssembly };
    }
}
