using LabLogger;
using LogicalExpressionClassLibrary;
using LogicalExpressionClassLibrary.Minimization;
using LogicalExpressionClassLibrary.Minimization.Strategy;
using System.Diagnostics.CodeAnalysis;

Logger.UseConsoleLogger();

Logger.Level = 0
    | Logger.Levels.Warning
    | Logger.Levels.Error
    | Logger.Levels.Info
    //| Logger.Levels.Debug
    ;

List<string> TestCases = [
    "( ((¬A)&((¬B)&C)) | (((¬A)&B)&C) | ((¬A)&(B&(¬C))) | (A&(B&(¬C))) )",
    "(A→(¬(B|C)))",
    "((A&B)~(¬C))",
    "( ((¬A)&((B)&(C))) | ((A)&((¬B)&(¬C))) | ((A)&((¬B)&(C))) | ((A)&((B)&(¬C))) | ((A)&((B)&(C))) )",
    "(((A)|(¬B))|((¬C)|(D))) & (((¬A)|(B))|((C)|(D))) & (((¬A)|(B))|((¬C)|(D))) & (((¬A)|(¬B))|((C)|(D))) & (((¬A)|(¬B))|((¬C)|(D)))",
    "(A&B&C)",
    "(A&B) | (A&(¬B)) | ((¬A)&B)",
    "(A|B)",
    "((E&A)→(B~(C|D)))",
    "((A|D)→(B~C))",
    "((B→A)~C)",
];

LogicalExpression expr = new(TestCases[0]);

Console.WriteLine(expr.ToString());
//Console.WriteLine(expr.FCNF);
//Console.WriteLine(expr.FDNF);
//Console.WriteLine(expr.ToTruthTableString());
//Console.WriteLine(expr.FCNF.ToTruthTableString());
//Console.WriteLine(expr.FDNF.ToTruthTableString());

//var min1FCNF = expr.Minimize(NormalForms.FCNF, new EvaluationStrategy());
var min2FCNF = expr.Minimize(NormalForms.FCNF, new TableStrategy());
//var min3FCNF = expr.Minimize(NormalForms.FCNF, new TableStrategy());

//var min1FDNF = expr.Minimize(NormalForms.FDNF, new EvaluationStrategy());
//var min2FDNF = expr.Minimize(NormalForms.FDNF, new CombinedStrategy());
//var min3FDNF = expr.Minimize(NormalForms.FDNF, new TableStrategy());

//Console.WriteLine($"Minimized FCNF with {nameof(EvaluationStrategy)}:".PadRight(45) + min1FCNF.ToString());
//Console.WriteLine($"Minimized FCNF with {nameof(CombinedStrategy)}:".PadRight(45) + min2FCNF.ToString());
//Console.WriteLine($"Minimized FCNF with {nameof(TableStrategy)}:".PadRight(45) + min3FCNF.ToString());

//Console.WriteLine($"Minimized FDNF with {nameof(EvaluationStrategy)}:".PadRight(45) + min1FDNF.ToString());
//Console.WriteLine($"Minimized FDNF with {nameof(CombinedStrategy)}:".PadRight(45) + min2FDNF.ToString());
//Console.WriteLine($"Minimized FDNF with {nameof(TableStrategy)}:".PadRight(45) + min3FDNF.ToString());


[ExcludeFromCodeCoverage]
public partial class Program() { }