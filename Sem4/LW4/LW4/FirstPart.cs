using LogicalExpressionClassLibrary;
using LogicalExpressionClassLibrary.Minimization.Strategy;
using LogicalExpressionClassLibrary.Minimization;
using System.Diagnostics.CodeAnalysis;

namespace LW4
{
    [ExcludeFromCodeCoverage]
    public static class FirstPart
    {
        public static void Execute()
        {
            Console.WriteLine("--- First part ---");
            Console.WriteLine("One-bit binary adder w. 3 inputs");

            // Set notation for formulas based on desired truth table
            // http://edu.tsu.ru/eor/resourse/436/html/16.html
            //
            string ODS3_Res_Source =
                "((¬A)&(¬B)&(P)) | " +
                "((¬A)&(B)&(¬P)) | " +
                "((A)&(¬B)&(¬P)) | " +
                "((A)&(B)&(P))";

            string ODS3_Tran_Source =
                "((¬A)&(B)&(P)) | " +
                "((A)&(¬B)&(P)) | " +
                "((A)&(B)&(¬P)) | " +
                "((A)&(B)&(P))";

            // Build formulas
            LogicalExpression ODS3_Res = new(ODS3_Res_Source);
            LogicalExpression ODS3_Tran = new(ODS3_Tran_Source);

            Console.WriteLine($"Source formula for Res: {ODS3_Res}");
            Console.WriteLine(ODS3_Res.ToTruthTableString());
            Console.WriteLine($"Source formula for Tran: {ODS3_Tran}");
            Console.WriteLine(ODS3_Tran.ToTruthTableString());

            // Minimize formulas
            IMinimizationStrategy minimizationStrategy = new TableStrategy();
            NormalForms normalForm = NormalForms.FCNF;
            LogicalExpression ODS3_Res_Minimized = ODS3_Res.Minimize(normalForm, minimizationStrategy);
            LogicalExpression ODS3_Tran_Minimized = ODS3_Tran.Minimize(normalForm, minimizationStrategy);

            Console.WriteLine($"Minimized formula for Res: {ODS3_Res_Minimized}");
            Console.WriteLine($"Minimized formula for Tran: {ODS3_Tran_Minimized}");
        }
    }
}
