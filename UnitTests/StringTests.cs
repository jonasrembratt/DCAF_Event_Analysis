using System;
using DCAF.Inspection._lib;
using Xunit;

namespace UnitTests
{
    public class StringHelperTests
    {
        [Fact]
        public void Test1()
        {
            const string HelloWorldLower = "hello world";
            const string HelloWorldUpper = "Hello World";
            var s = HelloWorldLower.ToUpperInitial();
            Assert.Equal(HelloWorldUpper, s);
            s = HelloWorldUpper.ToLowerInitial();
        }
    }
}