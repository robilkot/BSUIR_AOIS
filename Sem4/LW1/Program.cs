using LW1;
using System.Diagnostics.CodeAnalysis;
using static LW1.Printer;


BinaryFloat float1 = new(ReadFloat());
ShowDetails(float1);

BinaryFloat float2 = new(ReadFloat());
ShowDetails(float2);

BinaryFloat float3 = float1 + float2;
ShowDetails(float3);


BinaryFixed a = new(ReadInt());
ShowDetails(a);

BinaryFixed b = new(ReadInt());
ShowDetails(b);

ShowDetails(a / b);

[ExcludeFromCodeCoverage]
public partial class Program { }