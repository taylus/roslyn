using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Roslyn.Demo.Tests
{
    [TestClass]
    public class CSharpCompiler_Should : BaseCompilerTest
    {
        [TestMethod]
        public void Compile_Hello_World_Program()
        {
            var compiler = new CSharpCompiler();

            byte[] il = compiler.Compile.FromFile("Input/HelloWorld.cs", assemblyName: "HelloWorld");
            var assembly = Assembly.Load(il);

            string output = RunAndCaptureOutput(assembly, args: new string[] { "C#" });
            Assert.AreEqual("Hello C#!" + Environment.NewLine, output);
        }
    }
}
