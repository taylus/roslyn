using System;
using System.Linq;
using Xunit;

namespace HelloWorld
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"Hello {args.FirstOrDefault() ?? "world"}!");
        }

        [Fact]
        public void Test()
        {
            Assert.True(true);
        }
    }
}
