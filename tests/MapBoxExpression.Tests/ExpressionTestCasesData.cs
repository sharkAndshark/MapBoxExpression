using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MapBoxExpression.Tests
{
    internal class ExpressionTestCasesData
    {
        private static string _path = "test_cases/simple.json";
        public static IEnumerable Fixtureparams
        {
            get
            {
                var text = File.ReadAllText(_path);
                var jObj = JObject.Parse(text);

                var arr = jObj["cases"];
                var fcToken = jObj["FeatureContext"];
                var id = fcToken["id"].Value<int>();
                var zoom = fcToken["zoom"].Value<int>();
                var geometryType = fcToken["GeometryType"].Value<string>();

                var attributes = new Dictionary<string, dynamic>()
                {
                    { "a", "a" },
                    { "b", "b" },
                    { "c", 1 },
                    { "d", 2 },
                    { "e", new []{1.0,2.0,3.0, } }
                };
                foreach (var item in arr)
                {
                    var expToken = item["expression"];
                    var resultToken = item["result"];
                    yield return new TestFixtureData(expToken, resultToken, id, zoom, geometryType, attributes);
                }
            }
        }
    }
}