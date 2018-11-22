using System.Text;
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
        private static readonly VisualBasicCompilationOptions DefaultCompilationOptions = new VisualBasicCompilationOptions(OutputKind.ConsoleApplication);

        private static SyntaxTree Parse(string text)
        {
            var stringText = SourceText.From(text, Encoding.UTF8);
            return SyntaxFactory.ParseSyntaxTree(stringText);
        }

        private static Compilation CompileAssembly(SyntaxTree syntaxTree, string assemblyName)
        {
            return VisualBasicCompilation.Create(assemblyName, new SyntaxTree[] { syntaxTree }, ReferenceResolver.GetDefaultReferences(), DefaultCompilationOptions);
        }

        public CompilerFrontend Compile { get; } = new CompilerFrontend() { Parse = Parse, Compile = CompileAssembly };
    }
}
