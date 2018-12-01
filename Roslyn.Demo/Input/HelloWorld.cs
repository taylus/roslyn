using System;
using System.Linq;
using System.Collections.Generic;
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
            Assert.Equal(2 / 2, 1);
        }

        [Theory, MemberData(nameof(TestData))]
        public void ParameterizedTest(object expected, object actual)
        {
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> TestData => new List<object[]>
        {
            new object[] { 7 / 2, 3 },
            new object[] { 7 / 2.0, 3.5 }
        };
    }
}
