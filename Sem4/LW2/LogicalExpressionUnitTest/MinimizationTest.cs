using LogicalExpressionClassLibrary;
using LogicalExpressionClassLibrary.LogicalExpressionTree;
using LogicalExpressionClassLibrary.LogicalExpressionTree.ValueNodes;
using LogicalExpressionClassLibrary.Minimization;
using LogicalExpressionClassLibrary.Minimization.Strategy;
using System.Diagnostics.CodeAnalysis;

namespace LogicalExpressionUnitTest
{
    [ExcludeFromCodeCoverage]
    public class MinimizationTest
    {
        [Theory]
        [InlineData("( ((¬A)&((¬B)&C)) | (((¬A)&B)&C) | ((¬A)&(B&(¬C))) | (A&(B&(¬C))) )", "(((¬A)&C)|(B&(¬C)))")]
        public void FDNF_MinimizeWithCombinedStrategy_ResultShouldEqual(string input, string expected)
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            var expr = new LogicalExpression(input);

            var minimized = expr.Minimize(NormalForms.FDNF, new CombinedStrategy());

            Assert.Equal(expected, minimized.ToString());
        }

        [Theory]
        [InlineData("( ((¬A)&((B)&(C))) | ((A)&((¬B)&(¬C))) | ((A)&((¬B)&(C))) | ((A)&((B)&(¬C))) | ((A)&((B)&(C))) )", "((A|B)&(A|C))")]
        public void FCNF_MinimizeWithCombinedStrategy_ResultShouldEqual(string input, string expected)
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            var expr = new LogicalExpression(input);

            var minimized = expr.Minimize(NormalForms.FCNF, new CombinedStrategy());

            Assert.Equal(expected, minimized.ToString());
        }

        [Theory]
        [InlineData("( ((¬A)&((¬B)&C)) | (((¬A)&B)&C) | ((¬A)&(B&(¬C))) | (A&(B&(¬C))) )", "(((¬A)&C)|(B&(¬C)))")]
        public void FDNF_MinimizeWithEvaluationStrategy_ResultShouldEqual(string input, string expected)
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            var expr = new LogicalExpression(input);

            var minimized = expr.Minimize(NormalForms.FDNF, new EvaluationStrategy());

            Assert.Equal(expected, minimized.ToString());
        }

        [Theory]
        [InlineData("( ((¬A)&((B)&(C))) | ((A)&((¬B)&(¬C))) | ((A)&((¬B)&(C))) | ((A)&((B)&(¬C))) | ((A)&((B)&(C))) )", "((A|B)&(A|C))")]
        [InlineData("((E&A)→(B~(C|D)))", "(((((((¬E)|(¬A))|(¬B))|C)|D)&((((¬E)|(¬A))|B)|(¬D)))&((((¬E)|(¬A))|B)|(¬C)))")]
        public void FCNF_MinimizeWithEvaluationStrategy_ResultShouldEqual(string input, string expected)
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            var expr = new LogicalExpression(input);

            var minimized = expr.Minimize(NormalForms.FCNF, new EvaluationStrategy());

            Assert.Equal(expected, minimized.ToString());
        }

        [Theory]
        [InlineData("( ((¬A)&((¬B)&C)) | (((¬A)&B)&C) | ((¬A)&(B&(¬C))) | (A&(B&(¬C))) )", "(((¬C)&B)|(C&(¬A)))")]
        [InlineData("((E&A)→(B~(C|D)))", "((((((¬D)&((¬C)&(¬B)))|(¬E))|(¬A))|(C&B))|(D&B))")]
        [InlineData("((A|D)→(B~C))", "((((¬C)&(¬B))|((¬D)&(¬A)))|(C&B))")]
        [InlineData("(A|B)", "(A|B)")]
        [InlineData("(¬A)", "(¬A)")]
        public void FDNF_MinimizeWithTableStrategy_ResultShouldEqual(string input, string expected)
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            var expr = new LogicalExpression(input);

            var minimized = expr.Minimize(NormalForms.FDNF, new TableStrategy());

            Assert.Equal(expected, minimized.ToString());
        }

        [Theory]
        [InlineData("( ((¬A)&((B)&(C))) | ((A)&((¬B)&(¬C))) | ((A)&((¬B)&(C))) | ((A)&((B)&(¬C))) | ((A)&((B)&(C))) )", "((B|A)&(C|A))")]
        [InlineData("((A|D)→(B~C))", "(((((¬C)|(B|(¬A)))&((¬C)|(B|(¬D))))&(C|((¬B)|(¬A))))&(C|((¬B)|(¬D))))")]
        [InlineData("(A|B)", "(B|A)")]
        [InlineData("(¬A)", "(¬A)")]
        public void FCNF_MinimizeWithTableStrategy_ResultShouldEqual(string input, string expected)
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            var expr = new LogicalExpression(input);

            var minimized = expr.Minimize(NormalForms.FCNF, new TableStrategy());

            Assert.Equal(expected, minimized.ToString());
        }
    }
}