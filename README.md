# Roslyn C# and VB.NET Compilation Demo
A C# program that compiles another C# or VB.NET program and runs it dynamically.

The program being compiled:
```csharp
using System;
using System.Linq;

namespace HelloWorld
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"Hello {args.FirstOrDefault() ?? "world"}!");
        }
    }
}
```

Using the compiler:
```csharp
var compiler = new CSharpCompiler();
byte[] il = compiler.Compile.FromFile("input/HelloWorld.cs", assemblyName: "HelloWorld");

var assembly = Assembly.Load(il);
Console.WriteLine($"Compiled [{assembly}]");

Console.WriteLine($"Running {assembly.EntryPoint}...");
assembly.EntryPoint.Invoke(null, new object[] { new string[] { "from the outside world" } });
```

Running this demo:
``` bash
$ dotnet run
Compiled [HelloWorld, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]
Running HelloWorld.Program.Main...
Hello from the outside world!
```