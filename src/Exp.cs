using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBoxExpression
{
    public static class Exp
    {
        private static Dictionary<string, (OperatorType type, Operator opt)> OptInfos = new()
        {
            { "array", (OperatorType.TypeOperator, Operator.Array) },
            { "boolean", (OperatorType.TypeOperator, Operator.Boolean) },
            { "literal", (OperatorType.TypeOperator, Operator.Literal) },
            { "number", (OperatorType.TypeOperator, Operator.Number) },
            { "object", (OperatorType.TypeOperator, Operator.Object) },
            { "string", (OperatorType.TypeOperator, Operator.String) },
            { "to-boolean", (OperatorType.TypeOperator, Operator.ToBoolean) },
            { "to-number", (OperatorType.TypeOperator, Operator.ToNumber) },
            { "to-string", (OperatorType.TypeOperator, Operator.ToString) },
            { "typeof", (OperatorType.TypeOperator, Operator.Typeof) },

            { "id", (OperatorType.FeatureDataOperator, Operator.Id) },
            { "geometry-type", (OperatorType.FeatureDataOperator, Operator.GeometryType) },
            { "properties", (OperatorType.FeatureDataOperator, Operator.Properties) },

            { "at", (OperatorType.LookupOperator, Operator.At) },
            { "get", (OperatorType.LookupOperator, Operator.Get) },
            { "has", (OperatorType.LookupOperator, Operator.Has) },
            { "in", (OperatorType.LookupOperator, Operator.In) },
            { "index-of", (OperatorType.LookupOperator, Operator.IndexOf) },
            { "length", (OperatorType.LookupOperator, Operator.Length) },
            { "slice", (OperatorType.LookupOperator, Operator.Slice) },

            { "!", (OperatorType.DecisionOperator, Operator.Logicalnegation) },
            { "!=", (OperatorType.DecisionOperator, Operator.NotEqual) },
            { "<", (OperatorType.DecisionOperator, Operator.LessThan) },
            { "<=", (OperatorType.DecisionOperator, Operator.LessOrEqual) },
            { ">=", (OperatorType.DecisionOperator, Operator.GreateOrEqual) },
            { "==", (OperatorType.DecisionOperator, Operator.Equal) },
            { ">", (OperatorType.DecisionOperator, Operator.GreateThan) },
            { "all", (OperatorType.DecisionOperator, Operator.All) },
            { "any", (OperatorType.DecisionOperator, Operator.Any) },
            { "case", (OperatorType.DecisionOperator, Operator.Case) },
            { "coalesce", (OperatorType.DecisionOperator, Operator.Coalesce) },
            { "match", (OperatorType.DecisionOperator, Operator.Match) },

            { "interpolate", (OperatorType.ScalesOperator, Operator.Interpolate) },
            { "step", (OperatorType.ScalesOperator, Operator.Step) },

            { "let", (OperatorType.VariableOperator, Operator.Let) },
            { "var", (OperatorType.VariableOperator, Operator.Var) },

            { "concat", (OperatorType.StringOpereator, Operator.Concat) },
            { "downcase", (OperatorType.StringOpereator, Operator.Downcase) },
            { "upcase", (OperatorType.StringOpereator, Operator.Upcase) },

            { "-", (OperatorType.MathOperator, Operator.Minus) },
            { "*", (OperatorType.MathOperator, Operator.Product) },
            { "/", (OperatorType.MathOperator, Operator.Division) },
            { "%", (OperatorType.MathOperator, Operator.Remainder) },
            { "^", (OperatorType.MathOperator, Operator.Power) },
            { "+", (OperatorType.MathOperator, Operator.Plus) },
            { "abs", (OperatorType.MathOperator, Operator.Abs) },
            { "acos", (OperatorType.MathOperator, Operator.Acos) },
            { "asin", (OperatorType.MathOperator, Operator.Asin) },
            { "atan", (OperatorType.MathOperator, Operator.Atan) },
            { "ceil", (OperatorType.MathOperator, Operator.Ceil) },
            { "cos", (OperatorType.MathOperator, Operator.Cos) },
            { "e", (OperatorType.MathOperator, Operator.E) },
            { "floor", (OperatorType.MathOperator, Operator.Floor) },
            { "hash", (OperatorType.MathOperator, Operator.Hash) },
            { "ln", (OperatorType.MathOperator, Operator.Ln) },
            { "ln2", (OperatorType.MathOperator, Operator.Ln2) },
            { "log10", (OperatorType.MathOperator, Operator.Log10) },
            { "log2", (OperatorType.MathOperator, Operator.Log2) },
            { "max", (OperatorType.MathOperator, Operator.Max) },
            { "min", (OperatorType.MathOperator, Operator.Min) },
            { "pi", (OperatorType.MathOperator, Operator.Pi) },
            { "random", (OperatorType.MathOperator, Operator.Random) },
            { "round", (OperatorType.MathOperator, Operator.Round) },
            { "sin", (OperatorType.MathOperator, Operator.Sin) },
            { "sqrt", (OperatorType.MathOperator, Operator.Sqrt) },
            { "tan", (OperatorType.MathOperator, Operator.Tan) },

            { "zoom", (OperatorType.ZoomOperator, Operator.Zoom) },
        };

        public static dynamic Execute(JToken jToken, int zoom, string geometryType, object id = null, Dictionary<string, dynamic> attributes = null)
        {
            var type = jToken.Type;
            if (type == JTokenType.Array)//是表达式
            {
                return ExecuteExp(jToken, zoom, geometryType, id, attributes);
            }
            else//是字面量
            {
                return ExecuteConstants(jToken, type);
            }
        }

        private static dynamic ExecuteConstants(JToken jToken, JTokenType type)
        {
            switch (type)
            {
                case JTokenType.None:
                case JTokenType.Null:
                case JTokenType.Undefined:
                    return null;
                case JTokenType.Object:
                    return jToken.ToObject<Dictionary<string, dynamic>>();
                case JTokenType.Integer:
                case JTokenType.Float:
                    return jToken.Value<double>();
                case JTokenType.String:
                case JTokenType.Uri:
                case JTokenType.Guid:
                    return jToken.Value<string>();
                case JTokenType.Boolean:
                    return jToken.Value<bool>();
                case JTokenType.Date:
                    return jToken.ToString();
                default:
                    throw new NotImplementedException();
            }
        }

        private static dynamic ExecuteExp(JToken jToken, int zoom, string geometryType, object id, Dictionary<string, dynamic> attributes = null)
        {
            var tokens = jToken.ToArray();
            var optToken = tokens[0];
            var optStr = optToken.ToString();
            var opt = OptInfos[optStr].opt;
            switch (opt)
            {
                case Operator.Array:
                    break;
                case Operator.Boolean:
                    break;
                case Operator.Literal:
                    return tokens[1].ToArray().Select(t => Execute(t, zoom, geometryType, id, attributes)).ToArray();
                case Operator.Number:
                    break;
                case Operator.Object:
                    break;
                case Operator.String:
                    break;
                case Operator.ToBoolean:
                    break;
                case Operator.ToNumber:
                    break;
                case Operator.ToString:
                    return ToStringExp(zoom, geometryType, id, attributes, Execute(tokens[1], zoom, geometryType, id, attributes));
                case Operator.Typeof:
                    break;
                case Operator.Id:
                    return id;
                case Operator.GeometryType:
                    return geometryType;
                case Operator.Properties:
                    return attributes;
                case Operator.At:
                    var atIdx = (int)Execute(tokens[1], zoom, geometryType, id, attributes);
                    var atArr = Execute(tokens[2], zoom, geometryType, id, attributes);
                    return Execute(atArr[atIdx], zoom, geometryType, id, attributes);
                case Operator.Get:
                    var len = tokens.Length;
                    var attributeName = Execute(tokens[1], zoom, geometryType, id, attributes);
                    switch (len)
                    {
                        case 2:
                            if (attributes.ContainsKey(attributeName)) return attributes[attributeName];
                            return null;
                        case 3:
                            var target = Execute(tokens[2], zoom, geometryType, id, attributes);
                            var dic = target as Dictionary<string, dynamic>;
                            if (!dic.ContainsKey(attributeName)) return null;
                            return dic[attributeName];
                        default:
                            throw new NotSupportedException();
                    }
                case Operator.Has:
                    var keyName = Execute(tokens[1], zoom, geometryType, id, attributes);
                    switch (tokens.Length)
                    {
                        case 2:
                            return attributes.ContainsKey(keyName);
                        case 3:
                            var target = Execute(tokens[2], zoom, geometryType, id, attributes);
                            var dic = target as Dictionary<string, dynamic>;
                            return dic.ContainsKey(keyName);
                        default:
                            throw new NotSupportedException();
                    }
                case Operator.In:
                    break;
                case Operator.IndexOf:
                    break;
                case Operator.Length:
                    break;
                case Operator.Slice:
                    break;
                case Operator.Logicalnegation:
                    return !Execute(tokens[1], zoom, geometryType, id, attributes);
                case Operator.NotEqual:
                    return Execute(tokens[1], zoom, geometryType, id, attributes) != Execute(tokens[2], zoom, geometryType, id, attributes);
                case Operator.LessThan:
                    return Execute(tokens[1], zoom, geometryType, id, attributes) < Execute(tokens[2], zoom, geometryType, id, attributes);
                case Operator.LessOrEqual:
                    return Execute(tokens[1], zoom, geometryType, id, attributes) <= Execute(tokens[2], zoom, geometryType, id, attributes);
                case Operator.GreateOrEqual:
                    return Execute(tokens[1], zoom, geometryType, id, attributes) >= Execute(tokens[2], zoom, geometryType, id, attributes);
                case Operator.Equal:
                    return Execute(tokens[1], zoom, geometryType, id, attributes) == Execute(tokens[2], zoom, geometryType, id, attributes);
                case Operator.GreateThan:
                    return Execute(tokens[1], zoom, geometryType, id, attributes) > Execute(tokens[2], zoom, geometryType, id, attributes);
                case Operator.All:
                    break;
                case Operator.Any:
                    break;
                case Operator.Case:
                    var chunks = tokens.Skip(1).Chunk(2).ToArray();
                    for (int i = 0; i < chunks.Count() - 1; i++)
                    {
                        var condition = Execute(chunks[i][0], zoom, geometryType, id, attributes);
                        if (condition == true)
                        {
                            return Execute(chunks[i][1], zoom, geometryType, id, attributes);
                        }
                    }
                    return Execute(chunks.Last()[0], zoom, geometryType, id, attributes);
                case Operator.Coalesce:
                    break;
                case Operator.Match:
                    var inputValue = Execute(tokens[1], zoom, geometryType, id, attributes);
                    var chunksMatch = tokens.Skip(2).Chunk(2).ToArray();
                    for (int i = 0; i < chunksMatch.Length - 1; i++)
                    {
                        var labelOutput = chunksMatch[i];
                        var labelToken = labelOutput[0];
                        if(labelToken.Type == JTokenType.Array)
                        {
                            dynamic[] labelArr = labelToken.ToArray();
                            foreach (var item in labelArr)
                            {
                                var label = Execute(item,zoom, geometryType, id, attributes);
                                if(label == inputValue)
                                {
                                    return Execute(labelOutput[1],zoom, geometryType, id, attributes);
                                }
                            }
                        }
                        else
                        {
                            var label = Execute(labelOutput[0], zoom, geometryType, id, attributes);
                            if (label == inputValue) return Execute(labelOutput[1], zoom, geometryType, id, attributes);
                        }
                    }
                    return Execute(tokens.Last(), zoom, geometryType, id, attributes);
                case Operator.Interpolate:
                    return ExecuteInterpolateExp(tokens, zoom, geometryType, id, attributes);
                case Operator.Step:
                    break;
                case Operator.Let:
                    throw new NotImplementedException();
                case Operator.Var:
                    throw new NotImplementedException();
                case Operator.Concat:
                    var strArr = tokens.Skip(1).Select(t =>
                    {
                        var executeRes = Execute(t, zoom, geometryType, id, attributes);
                        return ToStringExp(zoom, geometryType, id, attributes, executeRes);
                    });
                    return string.Concat(strArr);
                case Operator.Downcase:
                    return Execute(tokens[1], zoom, geometryType, id, attributes).ToLower();
                case Operator.Upcase:
                    return Execute(tokens[1], zoom, geometryType, id, attributes).ToUpper();
                case Operator.Minus:
                    return Execute(tokens[1], zoom, geometryType, id, attributes) - Execute(tokens[2], zoom, geometryType, id, attributes);
                case Operator.Product:
                    return Execute(tokens[1], zoom, geometryType, id, attributes) * Execute(tokens[2], zoom, geometryType, id, attributes);
                case Operator.Division:
                    return (Execute(tokens[1], zoom, geometryType, id, attributes)) / (Execute(tokens[2], zoom, geometryType, id, attributes));
                case Operator.Remainder:
                    var remainderLeft = Execute(tokens[1], zoom, geometryType, id, attributes);
                    var remainderRight = Execute(tokens[2], zoom, geometryType, id, attributes);
                    return remainderLeft % remainderRight;
                case Operator.Power:
                    var powLeft = Execute(tokens[1], zoom, geometryType, id, attributes);
                    var powRight = Execute(tokens[2], zoom, geometryType, id, attributes);
                    return Math.Pow(powLeft, powRight);
                case Operator.Plus:
                    var plusLeft = Execute(tokens[1], zoom, geometryType, id, attributes);
                    var plusRight = Execute(tokens[2], zoom, geometryType, id, attributes);
                    return plusLeft + plusRight;
                case Operator.Abs:
                    var absTarget = Execute(tokens[1], zoom, geometryType, id, attributes);
                    return Math.Ceiling(absTarget);
                case Operator.Acos:
                    var acosTarget = Execute(tokens[1], zoom, geometryType, id, attributes);
                    return Math.Ceiling(acosTarget);
                case Operator.Asin:
                    var asinTarget = Execute(tokens[1], zoom, geometryType, id, attributes);
                    return Math.Ceiling(asinTarget);
                case Operator.Atan:
                    var atanTarget = Execute(tokens[1], zoom, geometryType, id, attributes);
                    return Math.Atan(atanTarget);
                case Operator.Ceil:
                    var ceilTarget = Execute(tokens[1], zoom, geometryType, id, attributes);
                    return Math.Ceiling(ceilTarget);
                case Operator.Cos:
                    var cosTarget = Execute(tokens[1], zoom, geometryType, id, attributes);
                    return Math.Cos(cosTarget);
                case Operator.E:
                    return Math.E;
                case Operator.Floor:
                    var floorTarget = Execute(tokens[1], zoom, geometryType, id, attributes);
                    return Math.Floor(floorTarget);
                case Operator.Hash:
                    break;
                case Operator.Ln:
                    break;
                case Operator.Ln2:
                    throw new NotImplementedException();
                case Operator.Log10:
                    var log10Target = Execute(tokens[1], zoom, geometryType, id, attributes);
                    return Math.Log10(log10Target);
                case Operator.Log2:
                    break;
                case Operator.Max:
                    var maxArr = tokens.Skip(1).Select(t => Execute(t, zoom, geometryType, id, attributes));
                    return maxArr.Max();
                case Operator.Min:
                    var minArr = tokens.Skip(1).Select(t => Execute(t, zoom, geometryType, id, attributes));
                    return minArr.Min();
                case Operator.Pi:
                    return Math.PI;
                case Operator.Random:
                    break;
                case Operator.Round:
                    var roundTarget = Execute(tokens[1], zoom, geometryType, id, attributes);
                    return Math.Round((double)roundTarget);
                case Operator.Sin:
                    var sinTarget = Execute(tokens[1], zoom, geometryType, id, attributes);
                    return Math.Sqrt(sinTarget);
                case Operator.Sqrt:
                    var sqrtTarget = Execute(tokens[1], zoom, geometryType, id, attributes);
                    return Math.Sqrt(sqrtTarget);
                case Operator.Tan:
                    var tanTarget = Execute(tokens[1], zoom, geometryType, id, attributes);
                    return Math.Tan(tanTarget);
                case Operator.Zoom:
                    return (double)zoom;
                default:
                    break;
            }
            throw new NotImplementedException();
        }
        internal enum InterpolationType
        {
            Linear,
            Exponential,
            CubicBezier
        }
        private static dynamic ExecuteInterpolateExp(JToken[] tokens, int zoom, string geometryType, object id, Dictionary<string, dynamic> attributes = null)
        {
            InterpolationType interpolationType = ParseInterpolationType(tokens[1].ToArray()[0].ToString());
            switch (interpolationType)
            {
                case InterpolationType.Linear:
                    return ExecuteLinearInterpolation(tokens, zoom, geometryType, id, attributes);
                case InterpolationType.Exponential:
                    return ExecuteExponentialInterpolate(tokens, zoom, geometryType, id, attributes);
                case InterpolationType.CubicBezier:
                    return ExecuteCubicBezierInterpolate(tokens, zoom, geometryType, id, attributes);
                default:
                    throw new ArgumentException();
            }
        }

        private static dynamic ExecuteLinearInterpolation(JToken[] tokens, int zoom, string geometryType, object id, Dictionary<string, dynamic> attributes = null)
        {
            double input = Execute(tokens[2], zoom, geometryType, id, attributes);
            var linearChunks = tokens.Skip(3).Chunk(2);
            double minInput;
            double maxInput;
            double minOutput;
            double maxOutput;
            var stops = linearChunks.Select(chunk =>
            {
                var inputStop = Execute(chunk[0], zoom, geometryType, id, attributes);
                var outputStop = Execute(chunk[1], zoom, geometryType, id, attributes);
                return (inputStop, outputStop);
            });
            minInput = stops.Min(x => x.inputStop);
            maxInput = stops.Max(x => x.inputStop);
            minOutput = stops.Min(x => x.outputStop);
            maxOutput = stops.Max(x => x.outputStop);

            var res = minOutput + (maxOutput - minOutput) * (input - minInput) / (maxInput - minInput);
            return res;
        }

        private static dynamic ExecuteCubicBezierInterpolate(JToken[] tokens, int zoom, string geometryType, object id, Dictionary<string, dynamic> attributes = null)
        {
            throw new NotImplementedException();
        }

        private static dynamic ExecuteExponentialInterpolate(JToken[] tokens, int zoom, string geometryType, object id, Dictionary<string, dynamic> attributes = null)
        {
            throw new NotImplementedException();
        }

        private static InterpolationType ParseInterpolationType(string interpolateType)
        {
            switch (interpolateType)
            {
                case "linear": return InterpolationType.Linear;
                case "exponential": return InterpolationType.Exponential;
                case "cubic-bezier": return InterpolationType.CubicBezier;
                default:
                    throw new ArgumentException();
            }
        }

        private static dynamic ToStringExp(int zoom, string geometryType, object id, Dictionary<string, object> attributes, dynamic res)
        {
            if (res == null)
            {
                return "";
            }
            var resType = res.GetType();
            if (resType == typeof(bool))
            {
                return res.ToString().ToLower();
            }
            else if (resType.IsValueType || resType == typeof(string))
            {
                return res.ToString();
            }
            return JsonConvert.SerializeObject(res);
        }
    }
}
