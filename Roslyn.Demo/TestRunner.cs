using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Roslyn.Demo
{
    /// <summary>
    /// Discover and run xUnit tests in given assemblies.
    /// </summary>
    /// <remarks>
    /// Feels like reinventing the wheel... use Xunit's AssemblyRunner instead?
    /// https://stackoverflow.com/a/45153763
    /// </remarks>
    public static class TestRunner
    {
        public static void TestFacts(Assembly assembly)
        {
            var testMethods = assembly.GetTypes().SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(false).Any(a => a is FactAttribute) &&
                           !m.GetCustomAttributes(false).Any(a => a is TheoryAttribute));

            foreach (var testMethod in testMethods)
            {
                Console.WriteLine($"Running test method {testMethod}...");
                var instance = Activator.CreateInstance(testMethod.DeclaringType);
                testMethod.Invoke(instance, null);
            }
        }

        public static void TestTheories(Assembly assembly)
        {
            //https://stackoverflow.com/questions/9110419/test-parameterization-in-xunit-net-similar-to-nunit/51172545#51172545
            //only basic MemberData support exists right now... feels like I'm reinventing xUnit but its AssemblyRunner only works on files and not Assembly objects :(
            var testMethods = assembly.GetTypes().SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(false).Any(a => a is TheoryAttribute) &&
                            m.GetCustomAttributes(false).Any(a => a is MemberDataAttribute));

            foreach (var testMethod in testMethods)
            {
                //get the name of the member referenced in the [MemberData] attribute
                var instance = Activator.CreateInstance(testMethod.DeclaringType);
                var memberData = testMethod.GetCustomAttribute<MemberDataAttribute>();
                if (string.IsNullOrWhiteSpace(memberData.MemberName)) continue;

                //get the value of the member referenced in the [MemberData] attribute
                var testData = GetValue(testMethod.DeclaringType.GetMember(memberData.MemberName).FirstOrDefault(), instance) as IEnumerable<object[]>;
                if (testData == null) continue;

                //run the test with each set of inputs defined in the member data
                foreach(var testInput in testData)
                {
                    Console.WriteLine($"Running parameterized test method {testMethod} with input {string.Join(", ", testInput)}...");
                    testMethod.Invoke(instance, testInput);
                }
            }
        }

        /// <summary>
        /// Gets the value of a reflected member (if possible).
        /// </summary>
        private static object GetValue(MemberInfo memberInfo, object forObject)
        {
            switch (memberInfo?.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(forObject);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(forObject);
                default:
                    return null;
            }
        }
    }
}
