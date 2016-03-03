using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.IO;

namespace HandlebarsDotNet.Test
{
	[TestClass]
	public class TripleStashTests
	{
		[TestMethod]
		public void UnencodedPartial()
		{
			string source = "Hello, {{{>unenc_person}}}!";

			var template = Handlebars.Compile(source);

			var data = new {
				name = "Marc"
			};

			var partialSource = "<div>{{name}}</div>";
			using(var reader = new StringReader(partialSource))
			{
				var partialTemplate = Handlebars.Compile(reader);
				Handlebars.RegisterTemplate("unenc_person", partialTemplate);
			}

			var result = template(data);
			Assert.AreEqual("Hello, <div>Marc</div>!", result);
		}

		[TestMethod]
		public void EncodedPartialWithUnencodedContents()
		{
			string source = "Hello, {{>enc_person}}!";

			var template = Handlebars.Compile(source);

			var data = new {
				name = "<div>Marc</div>"
			};

			var partialSource = "<div>{{{name}}}</div>";
			using(var reader = new StringReader(partialSource))
			{
				var partialTemplate = Handlebars.Compile(reader);
				Handlebars.RegisterTemplate("enc_person", partialTemplate);
			}

			var result = template(data);
			Assert.AreEqual("Hello, <div><div>Marc</div></div>!", result);
		}

		[TestMethod]
		public void UnencodedObjectEnumeratorItems()
		{
			var source = "{{#each enumerateMe}}{{{this}}} {{/each}}";
			var template = Handlebars.Compile(source);
			var data = new
			{
				enumerateMe = new
				{
					foo = "<div>hello</div>",
					bar = "<div>world</div>"
				}
			};
			var result = template(data);
			Assert.AreEqual("<div>hello</div> <div>world</div> ", result);
		}

        [TestMethod]
        public void FailingBasicTripleStash()
        {
            string source = "{{#if a_bool}}{{{dangerous_value}}}{{/if}}Hello, {{{dangerous_value}}}!";

            var template = Handlebars.Compile(source);

            var data = new
                {
                    a_bool = false,
                    dangerous_value = "<div>There's HTML here</div>"
                };

            var result = template(data);
            Assert.AreEqual("Hello, <div>There's HTML here</div>!", result);
        }
	}
}

