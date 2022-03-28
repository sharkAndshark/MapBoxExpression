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
                var attributes = fcToken["attributes"].ToObject<Dictionary<string, dynamic>>();
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