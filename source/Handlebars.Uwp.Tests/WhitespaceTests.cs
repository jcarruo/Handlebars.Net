using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace HandlebarsDotNet.Test
{
    [TestClass]
    public class WhitespaceTests
    {
        [TestMethod]
        public void PreceedingWhitespace()
        {
            var source = "Hello, {{~name}}!";
            var template = Handlebars.Compile(source);
            var data = new {
                name = "Handlebars.Net"
            };
            var result = template(data);
            Assert.AreEqual("Hello,Handlebars.Net!", result);
        }

        [TestMethod]
        public void TrailingWhitespace()
        {
            var source = "Hello, {{name~}} !";
            var template = Handlebars.Compile(source);
            var data = new {
                name = "Handlebars.Net"
            };
            var result = template(data);
            Assert.AreEqual("Hello, Handlebars.Net!", result);
        }

        [TestMethod]
        public void ComplexTest()
        {
            var source =
@"{{#each nav ~}}
  <a href=""{{url}}"">
    {{~#if test}}
      {{~title}}
    {{~else~}}
      Empty
    {{~/if~}}
  </a>
{{~/each}}";
            var template = Handlebars.Compile(source);
            var data = new {
                nav = new [] {
                    new {
                        url = "https://google.com",
                        test = true,
                        title = "Google"
                    },
                    new {
                        url = "https://bing.com",
                        test = false,
                        title = "Bing"
                    }
                }
            };
            var result = template(data);
            Assert.AreEqual(@"<a href=""https://google.com"">Google</a><a href=""https://bing.com"">Empty</a>", result);
        }
    }
}

