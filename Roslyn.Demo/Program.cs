using System;
using System.Reflection;

namespace Roslyn.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var compiler = new VBCompiler();
            byte[] il = compiler.Compile.FromFile("input/HelloWorld.vb", assemblyName: "HelloWorld");

            var assembly = Assembly.Load(il);
            Console.WriteLine($"Compiled [{assembly}]");

            Console.WriteLine($"Running {GetFullName(assembly.EntryPoint)}...");
            assembly.EntryPoint.Invoke(null, new object[] { new string[] { "from the outside world" } });
        }

        private static string GetFullName(MethodInfo method) => method.DeclaringType.FullName + "." + method.Name;
    }
}
