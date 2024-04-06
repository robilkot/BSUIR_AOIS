using LogicalExpressionClassLibrary;
using LW4;

ConsoleLogger.DebugLevel =
    ConsoleLogger.DebugLevels.Warning
    | ConsoleLogger.DebugLevels.Error
    | ConsoleLogger.DebugLevels.Info
    ;

Console.WriteLine("Variant 3-e");
FirstPart.Execute();
Console.WriteLine();
SecondPart.Execute();