using System;
using System.IO;
using Microsoft.CodeAnalysis;

namespace Roslyn.Demo
{
    public class CompilerFrontend
    {
        public Func<string, SyntaxTree> Parse { get; set; }
        public Func<SyntaxTree, string, Compilation> Compile { get; set; }

        /// <summary>
        /// Compiles the given source file into an IL byte array.
        /// </summary>
        public byte[] FromFile(string filename, string assemblyName)
        {
            string source = File.ReadAllText(filename);
            return FromString(source, assemblyName);
        }

        /// <summary>
        /// Compiles the given string of source code into an IL byte array.
        /// </summary>
        public byte[] FromString(string code, string assemblyName)
        {
            var parsedSyntaxTree = Parse(code);
            var compilation = Compile(parsedSyntaxTree, assemblyName);

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
}
