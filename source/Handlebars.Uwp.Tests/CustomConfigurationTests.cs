using HandlebarsDotNet.Compiler.Resolvers;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace HandlebarsDotNet.Test
{
    [TestClass]
    public class CustomConfigurationTests
    {
        public IHandlebars HandlebarsInstance { get; private set; }
        public const string ExpectedOutput = "Hello Eric Sharp from Japan. You're <b>AWESOME</b>.";
        public object Value = new
                    {
                        Person = new { Name = "Eric", Surname = "Sharp", Address = new { HomeCountry = "Japan" } },
                        Description = @"<b>AWESOME</b>"
                    };

        [TestInitialize]
        public void Init()
        {
            var configuration = new HandlebarsConfiguration
                                    {
                                        ExpressionNameResolver =
                                            new UpperCamelCaseExpressionNameResolver()
                                    };
                        
            this.HandlebarsInstance = Handlebars.Create(configuration);
        }

        #region UpperCamelCaseExpressionNameResolver Tests

        [TestMethod]
        public void LowerCamelCaseInputModelNaming()
        {
            var template = "Hello {{person.name}} {{person.surname}} from {{person.address.homeCountry}}. You're {{{description}}}.";
            var output = this.HandlebarsInstance.Compile(template).Invoke(Value);

            Assert.AreEqual(output, ExpectedOutput);
        }

        [TestMethod]
        public void UpperCamelCaseInputModelNaming()
        {
            var template = "Hello {{person.name}} {{person.surname}} from {{person.address.homeCountry}}. You're {{{description}}}.";
            var output = this.HandlebarsInstance.Compile(template).Invoke(Value);

            Assert.AreEqual(output, ExpectedOutput);
        }

        [TestMethod]
        public void SnakeCaseInputModelNaming()
        {
            var template = "Hello {{person.name}} {{person.surname}} from {{person.address.home_Country}}. You're {{{description}}}.";
            var output = this.HandlebarsInstance.Compile(template).Invoke(Value);

            Assert.AreEqual(output, ExpectedOutput);
        }

        #endregion
    }
}