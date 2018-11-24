using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Roslyn.Demo.Tests
{
    [TestClass]
    public class VBCompiler_Should : BaseCompilerTest
    {
        [TestMethod]
        public void Compile_Hello_World_Program()
        {
            var compiler = new VBCompiler();

            byte[] il = compiler.Compile.FromFile("Input/HelloWorld.vb", assemblyName: "HelloWorld");
            var assembly = Assembly.Load(il);

            string output = RunAndCaptureOutput(assembly, args: new string[] { "VB.NET" });
            Assert.AreEqual("Hello VB.NET!" + Environment.NewLine, output);
        }
    }
}
