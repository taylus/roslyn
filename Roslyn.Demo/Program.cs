using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Roslyn.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CompileAndRunCSharpDemo();
            Console.WriteLine();
            CompileAndRunVBDemo();
        }

        private static void CompileAndRunCSharpDemo()
        {
            var compiler = new CSharpCompiler();
            var xunit = new List<MetadataReference>()
            {
                MetadataReference.CreateFromFile(typeof(Assert).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(FactAttribute).Assembly.Location)
            };
            byte[] il = compiler.Compile.FromFile("Input/HelloWorld.cs", assemblyName: "CSHelloWorld", additionalReferences: xunit);

            var assembly = Assembly.Load(il);
            Console.WriteLine($"Compiled [{assembly}]");

            Console.WriteLine($"Running {GetFullName(assembly.EntryPoint)}...");
            assembly.EntryPoint.Invoke(null, new object[] { new string[] { "from the outside world" } });

            RunTestMethods(assembly);
        }

        private static void CompileAndRunVBDemo()
        {
            var compiler = new VBCompiler();
            byte[] il = compiler.Compile.FromFile("Input/HelloWorld.vb", assemblyName: "VBHelloWorld");

            var assembly = Assembly.Load(il);
            Console.WriteLine($"Compiled [{assembly}]");

            Console.WriteLine($"Running {GetFullName(assembly.EntryPoint)}...");
            assembly.EntryPoint.Invoke(null, new object[] { new string[] { "from the outside world" } });

            RunTestMethods(assembly);
        }

        private static void RunTestMethods(Assembly assembly)
        {
            //TODO: use Xunit's AssemblyRunner instead?
            //https://stackoverflow.com/a/45153763

            var testMethods = assembly.GetTypes().SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttribute(typeof(FactAttribute), false) != null);

            foreach (var testMethod in testMethods)
            {
                Console.WriteLine($"Running test method {testMethod}...");
                var instance = Activator.CreateInstance(testMethod.DeclaringType);
                testMethod.Invoke(instance, null);
            }
        }

        private static string GetFullName(MethodInfo method) => method.DeclaringType.FullName + "." + method.Name;
    }
}
