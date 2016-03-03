using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace HandlebarsDotNet.Test
{
    [TestClass]
    public class NumericLiteralTests
    {
        public NumericLiteralTests()
        {
            Handlebars.RegisterHelper("numericLiteralAdd", (writer, context, args) =>
                {
                    args = args.Select(a => (object)int.Parse((string)a)).ToArray();
                    writer.Write(args.Aggregate(0, (a, i) => a + (int)i));
                });
        }

        [TestMethod]
        public void NumericLiteralTest1()
        {
            var source = "{{numericLiteralAdd 3 4}}";
            var template = Handlebars.Compile(source);
            var data = new {};
            var result = template(data);
            Assert.AreEqual("7", result);
        }

        [TestMethod]
        public void NumericLiteralTest2()
        {
            var source = "{{numericLiteralAdd 3  4}}";
            var template = Handlebars.Compile(source);
            var data = new {};
            var result = template(data);
            Assert.AreEqual("7", result);
        }

        [TestMethod]
        public void NumericLiteralTest3()
        {
            var source = "{{numericLiteralAdd 3 4 }}";
            var template = Handlebars.Compile(source);
            var data = new {};
            var result = template(data);
            Assert.AreEqual("7", result);
        }

        [TestMethod]
        public void NumericLiteralTest4()
        {
            var source = "{{numericLiteralAdd 3    4 }}";
            var template = Handlebars.Compile(source);
            var data = new {};
            var result = template(data);
            Assert.AreEqual("7", result);
        }

        [TestMethod]
        public void NumericLiteralTest5()
        {
            var source = "{{numericLiteralAdd    3    4 }}";
            var template = Handlebars.Compile(source);
            var data = new {};
            var result = template(data);
            Assert.AreEqual("7", result);
        }

        [TestMethod]
        public void NumericLiteralTest6()
        {
            var source = "{{numericLiteralAdd 3 \"4\"}}";
            var template = Handlebars.Compile(source);
            var data = new {};
            var result = template(data);
            Assert.AreEqual("7", result);
        }

        [TestMethod]
        public void NumericLiteralTest7()
        {
            var source = "{{numericLiteralAdd 3 \"4\" }}";
            var template = Handlebars.Compile(source);
            var data = new {};
            var result = template(data);
            Assert.AreEqual("7", result);
        }

        [TestMethod]
        public void NumericLiteralTest8()
        {
            var source = "{{numericLiteralAdd 3    \"4\" }}";
            var template = Handlebars.Compile(source);
            var data = new {};
            var result = template(data);
            Assert.AreEqual("7", result);
        }

        [TestMethod]
        public void NumericLiteralTest9()
        {
            var source = "{{numericLiteralAdd    3   \"4\" }}";
            var template = Handlebars.Compile(source);
            var data = new {};
            var result = template(data);
            Assert.AreEqual("7", result);
        }

        [TestMethod]
        public void NumericLiteralTest10()
        {
            var source = "{{numericLiteralAdd \"3\" 4}}";
            var template = Handlebars.Compile(source);
            var data = new {};
            var result = template(data);
            Assert.AreEqual("7", result);
        }

        [TestMethod]
        public void NumericLiteralTest11()
        {
            var source = "{{numericLiteralAdd \"3\" 4 }}";
            var template = Handlebars.Compile(source);
            var data = new {};
            var result = template(data);
            Assert.AreEqual("7", result);
        }
    }
}

