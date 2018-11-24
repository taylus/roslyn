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
