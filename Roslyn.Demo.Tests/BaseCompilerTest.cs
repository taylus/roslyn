using System;
using System.IO;
using System.Reflection;

namespace Roslyn.Demo.Tests
{
    public class BaseCompilerTest
    {
        /// <summary>
        /// Run the given assembly's entry point and return its console output.
        /// This method uses <see cref="Console.SetOut"/> and is *not* thread safe!
        /// </summary>
        protected string RunAndCaptureOutput(Assembly assembly, string[] args = null)
        {
            if (args == null) args = new string[] { };
            var oldOut = Console.Out;
            var oldErr = Console.Error;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Console.SetError(sw);

                assembly.EntryPoint.Invoke(null, new object[] { args });

                Console.SetOut(oldOut);
                Console.SetError(oldErr);
                return sw.ToString();
            }
        }
    }
}
