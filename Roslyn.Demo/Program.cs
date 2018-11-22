using System;
using System.Reflection;

namespace Roslyn.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //TODO: turn into unit tests
            CompileAndRunCSharpDemo();
            Console.WriteLine();
            CompileAndRunVBDemo();
        }

        private static void CompileAndRunCSharpDemo()
        {
            var compiler = new CSharpCompiler();
            byte[] il = compiler.Compile.FromFile("input/HelloWorld.cs", assemblyName: "CSHelloWorld");

            var assembly = Assembly.Load(il);
            Console.WriteLine($"Compiled [{assembly}]");

            Console.WriteLine($"Running {GetFullName(assembly.EntryPoint)}...");
            assembly.EntryPoint.Invoke(null, new object[] { new string[] { "from the outside world" } });
        }

        private static void CompileAndRunVBDemo()
        {
            var compiler = new VBCompiler();
            byte[] il = compiler.Compile.FromFile("input/HelloWorld.vb", assemblyName: "VBHelloWorld");

            var assembly = Assembly.Load(il);
            Console.WriteLine($"Compiled [{assembly}]");

            Console.WriteLine($"Running {GetFullName(assembly.EntryPoint)}...");
            assembly.EntryPoint.Invoke(null, new object[] { new string[] { "from the outside world" } });
        }

        private static string GetFullName(MethodInfo method) => method.DeclaringType.FullName + "." + method.Name;
    }
}
