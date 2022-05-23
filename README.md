# MapBoxExpression

Simple library can help you parse MapBox Expression to C# lambda.

# Demos
```csharp
var expJson = "[\"interpolate\",[ \"linear\" ],[ \"zoom\" ],0,10,10,0]";
var result = Exp.Execute(expToken, zoom, geometryType, id, attributes);
Assert.AreEqual(5,result);
```

