using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Dynamic;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace HandlebarsDotNet.Test
{
    [TestClass]
    public class DynamicTests
    {
        [TestMethod]
		public void DynamicObjectBasicTest()
        {
            var model = new MyDynamicModel();

            var source = "Foo: {{foo}}\nBar: {{bar}}";

            var template = Handlebars.Compile(source);

            var output = template(model);

            Assert.AreEqual("Foo: 1\nBar: hello world", output);
        }

		[TestMethod]
		public void JsonTestIfTruthy()
		{
			var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>("{\"myfield\":\"test1\",\"truthy\":\"test2\"}");

			var source = "{{myfield}}{{#if truthy}}{{truthy}}{{/if}}";

			var template = Handlebars.Compile(source);

			var output = template(model);

			Assert.AreEqual("test1test2", output);
		}

		[TestMethod]
		public void JsonTestIfFalsyMissingField()
		{
			var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>("{\"myfield\":\"test1\"}");

			var source = "{{myfield}}{{#if mymissingfield}}{{mymissingfield}}{{/if}}";

			var template = Handlebars.Compile(source);

			var output = template(model);

			Assert.AreEqual("test1", output);
		}

		[TestMethod]
		public void JsonTestIfFalsyValue()
		{
			var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>("{\"myfield\":\"test1\",\"falsy\":null}");

			var source = "{{myfield}}{{#if falsy}}{{falsy}}{{/if}}";

			var template = Handlebars.Compile(source);

			var output = template(model);

			Assert.AreEqual("test1", output);
		}

        [TestMethod]
        public void JsonTestArrays(){
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>("[{\"Key\": \"Key1\", \"Value\": \"Val1\"},{\"Key\": \"Key2\", \"Value\": \"Val2\"}]");

            var source = "{{#each this}}{{Key}}{{Value}}{{/each}}";

            var template = Handlebars.Compile(source);

            var output = template(model);

            Assert.AreEqual("Key1Val1Key2Val2", output);
        }

        [TestMethod]
        public void JObjectTest() {
            object nullValue = null;
            var model = JObject.FromObject(new { Nested = new { Prop = "Prop" }, Nested2 = nullValue });

            var source = "{{NotExists.Prop}}";

            var template = Handlebars.Compile(source);

            var output = template(model);

            Assert.AreEqual("", output);
        }

        [TestMethod]
        public void SystemJsonTestArrays()
        {
            //var model = System.Web.Helpers.Json.Decode("[{\"Key\": \"Key1\", \"Value\": \"Val1\"},{\"Key\": \"Key2\", \"Value\": \"Val2\"}]");
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject("[{\"Key\": \"Key1\", \"Value\": \"Val1\"},{\"Key\": \"Key2\", \"Value\": \"Val2\"}]");

            var source = "{{#each this}}{{Key}}{{Value}}{{/each}}";

            var template = Handlebars.Compile(source);

            var output = template(model);

            Assert.AreEqual("Key1Val1Key2Val2", output);
        }


        private class MyDynamicModel : DynamicObject
        {
            private Dictionary<string, object> properties = new Dictionary<string, object>
            {
                { "foo", 1 },
                { "bar", "hello world" }
            };

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if(properties.ContainsKey(binder.Name))
                {
                    result = properties[binder.Name];
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
        }
    }
}

