using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapBoxExpression.Tests
{
    [TestFixtureSource(typeof(ExpressionTestCasesData), nameof(ExpressionTestCasesData.Fixtureparams))]
    internal class ExpTest
    {
        private JToken expToken;
        private JToken resultToken;
        private object id;
        private int zoom;
        private string geometryType;
        private Dictionary<string, dynamic> attributes;

        public ExpTest(JToken expToken, JToken resultToken, dynamic id, dynamic zoom, dynamic geometryType, Dictionary<string, dynamic> attributes)
        {
            this.expToken = expToken;
            this.resultToken = resultToken;
            this.id = id;
            this.zoom = zoom;
            this.geometryType = geometryType;
            this.attributes = attributes;
        }

        [Test]
        public void Test()
        {
            object result;
            switch (resultToken.Type)
            {
                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Object:
                case JTokenType.Boolean:
                    result = Exp.Execute(expToken, zoom, geometryType, id, attributes);
                    break;
                case JTokenType.Null:
                    Assert.IsNull(Exp.Execute(expToken, zoom, geometryType, id, attributes));
                    return;
                case JTokenType.Array:
                    result = Exp.Execute(expToken, zoom, geometryType, id, attributes);
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (result as object[] != null)
            {
                var arr = result as object[];
                var expectedArr = resultToken.ToArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    var item = arr[i];
                    var expectedResult = expectedArr[i];
                    TestEqual(item, expectedResult);
                }
            }
            else
            {
                TestEqual(result, resultToken);
            }
        }
        private void TestEqual(dynamic item, JToken token)
        {
            if (item.GetType() == typeof(string))
            {
                Assert.AreEqual(item, token.Value<string>());
            }
            else if (item.GetType() == typeof(int))
            {
                Assert.AreEqual(item, token.Value<int>());
            }
            else if (item.GetType() == typeof(long))
            {
                Assert.AreEqual(item, token.Value<long>());
            }
            else if (item.GetType() == typeof(double))
            {
                Assert.AreEqual(item, token.Value<double>(), 5);
            }
            else if (item.GetType().FullName.StartsWith("System.Collections.Generic.Dictionary"))
            {
                var objRes = item as Dictionary<string, dynamic>;

                foreach (var key in objRes.Keys)
                {
                    var r = objRes[key];
                    var e = token[key];
                    TestEqual(r, e);
                }
            }
        }
    }
}