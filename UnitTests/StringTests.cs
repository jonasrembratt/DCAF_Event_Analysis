using DCAF.Inspection._lib;
using Xunit;

namespace UnitTests
{
    public class StringHelperTests
    {
        const string HelloWorldLower = " hello world ";
        const string HelloWorldUpper = " Hello World ";
        const string HelloWorldPascal = "HelloWorld";
        const string HelloWorldCamel = "helloWorld";

        [Fact]
        public void ToUpperInitial()
        {
            var s = HelloWorldLower.ToUpperInitial();
            Assert.Equal(HelloWorldUpper, s);
        }
        
        [Fact]
        public void ToLowerInitial()
        {
            var s = HelloWorldUpper.ToLowerInitial();
            Assert.Equal(HelloWorldLower, s);
        }
        
        [Fact]
        public void ToPascal()
        {
            var s = HelloWorldLower.ToIdentifier(IdentCasing.Pascal);
            Assert.Equal(HelloWorldPascal, s);
        }
        
        [Fact]
        public void ToCamel()
        {
            var s = HelloWorldLower.ToIdentifier(IdentCasing.Camel);
            Assert.Equal(HelloWorldCamel, s);
        }
    }
}