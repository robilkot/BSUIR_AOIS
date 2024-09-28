using LogicalExpressionClassLibrary;
using LogicalExpressionClassLibrary.Minimization;
using LogicalExpressionClassLibrary.Minimization.Strategy;
using System.Diagnostics.CodeAnalysis;

namespace LW4
{
    [ExcludeFromCodeCoverage]
    public static class SecondPart_1
    {
        public static void Execute()
        {
            Console.WriteLine("--- Second part ---");
            Console.WriteLine("From BCD 8241 to BCD 8241+1");

            // Set notation for formulas based on desired truth table
            // A B C D  Y0 Y1 Y2 Y3
            // 0 0 0 0  0  0  0  1
            // 0 0 0 1  0  0  1  0
            // 0 0 1 0  0  0  1  1
            // 0 0 1 1  0  1  0  0
            // 0 1 0 0  0  1  0  1
            // 0 1 0 1  0  1  1  0
            // 0 1 1 0  0  1  1  1
            // 0 1 1 1  1  0  0  0
            // 1 0 0 0  1  0  0  1
            // 1 0 0 1  1  0  1  0
            // 1 0 1 0  -  -  -  -
            // 1 0 1 1  -  -  -  - 
            // 1 1 0 0  -  -  -  -
            // 1 1 0 1  -  -  -  -
            // 1 1 1 0  -  -  -  -
            // 1 1 1 1  -  -  -  -
            //
            string[] BCD8241_9_Source = [
                // Y0
                "((¬A)&( B)&( C)&( D)) | " +
                "(( A)&(¬B)&(¬C)&(¬D)) | " +
                "(( A)&(¬B)&(¬C)&( D))",

                // Y1
                "((¬A)&(¬B)&( C)&( D)) | " +
                "((¬A)&( B)&(¬C)&(¬D)) | " +
                "((¬A)&( B)&(¬C)&( D)) | " +
                "((¬A)&( B)&( C)&(¬D))",

                // Y2
                "((¬A)&(¬B)&(¬C)&( D)) | " +
                "((¬A)&(¬B)&( C)&(¬D)) | " +
                "((¬A)&( B)&(¬C)&( D)) | " +
                "((¬A)&( B)&( C)&(¬D)) | " +
                "(( A)&(¬B)&(¬C)&( D))",

                // Y3
                "((¬A)&(¬B)&(¬C)&(¬D)) | " +
                "((¬A)&(¬B)&( C)&(¬D)) | " +
                "((¬A)&( B)&(¬C)&(¬D)) | " +
                "((¬A)&( B)&( C)&(¬D)) | " +
                "(( A)&(¬B)&(¬C)&(¬D))",
            ];

            // Build formulas
            List<LogicalExpression> BCD8241_9 = BCD8241_9_Source.Select(str => new LogicalExpression(str)).ToList();

            for (int i = 0; i < BCD8241_9.Count; i++)
            {
                Console.WriteLine($"Source formula for Y{i}: {BCD8241_9[i]}");
            }

            // Minimize formulas
            IMinimizationStrategy minimizationStrategy = new CombinedStrategy();
            NormalForms normalForm = NormalForms.FDNF;
            List<LogicalExpression> BCD8241_9_Minimized = BCD8241_9.Select(expr => expr.Minimize(normalForm, minimizationStrategy)).ToList();

            for (int i = 0; i < BCD8241_9_Minimized.Count; i++)
            {
                Console.WriteLine($"Minimized {normalForm} for Y{i}: {BCD8241_9_Minimized[i]}");
            }
        }
    }
}
