using LabLogger;
using LW4;

Logger.UseConsoleLogger();

Logger.Level = 0
    | Logger.Levels.Warning
    | Logger.Levels.Error
    | Logger.Levels.Info
    ;

Console.WriteLine("Variant 3-e");
FirstPart.Execute();
Console.WriteLine();
SecondPart.Execute();