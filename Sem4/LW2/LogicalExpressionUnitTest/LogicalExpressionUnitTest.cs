using LogicalExpressionClassLibrary;
using LogicalExpressionClassLibrary.LogicalExpressionTree;
using LogicalExpressionClassLibrary.LogicalParser;
using System.Diagnostics.CodeAnalysis;

namespace LogicalExpressionUnitTest
{
    //"(A1&(A2|(¬(A3|(A4|A5)))))"
    //"((A|B)&(¬C))"
    //"((¬A)|(¬(B&C)))"
    //"(A&(B&C))",
    //"(A&(B|C))",
    //"(A→(B→C))",
    //"(B|C)",
    //"((A1|(A2&A3))|B1)",
    //"((A1|F)|T)",
    //"((P|(Q&R))|S)",
    //"((A1|(&A3))|B1)",

    // pz 2
    // "(((!P)>P)>(R|Q))"
    // "((((P>R)&(Q>S))&((!P)|(!S)))>((!P)|(!Q)))"

    [ExcludeFromCodeCoverage]
    public class LogicalExpressionUnitTest
    {
        [Theory]
        [InlineData("A123")]
        [InlineData("B")]
        [InlineData("C1")]
        [InlineData("((A1|(A2&A3))|B1)")]
        [InlineData("((A1|F)|T)")]
        [InlineData("(¬(¬A123))")]
        [InlineData("(¬(¬(¬(¬A123))))")]
        [InlineData("((A1|(B2&A3))|B1)")]
        public void RecursiveParsing_shouldEqual(string input)
        {
            ConsoleLogger.DebugLevel = 
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            LogicalExpression expr = new(input, new RecursiveLogicalParser());

            Assert.Equal(input, expr.ToString());
        }

        [Theory]
        [InlineData("((A|B)&(¬C))", "(((((¬A)&B)&(¬C))|((A&(¬B))&(¬C)))|((A&B)&(¬C)))")]
        public void FDNF_shouldEqual(string input, string expected)
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            LogicalExpression expr = new(input);

            var FDNF = expr.FDNF;

            Assert.Equal(expected, FDNF.ToString());
        }

        [Theory]
        [InlineData("((A|B)&(¬C))", "((((((A|B)|C)&((A|B)|(¬C)))&((A|(¬B))|(¬C)))&(((¬A)|B)|(¬C)))&(((¬A)|(¬B))|(¬C)))")]
        public void FCNF_shouldEqual(string input, string expected)
        {
            ConsoleLogger.DebugLevel =
                 ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                 ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            LogicalExpression expr = new(input);

            var FCNF = expr.FCNF;

            Assert.Equal(expected, FCNF.ToString());
        }

        [Theory]
        [InlineData("((A|B)&(¬C))", "(7, 5, 3, 1, 0) &")]
        public void FCNFNumericString_shouldEqual(string input, string expected)
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            LogicalExpression expr = new(input);

            var FCNF = expr.FCNF.ToNFNumericString(NormalForms.FCNF);

            Assert.Equal(expected, FCNF);
        }

        [Theory]
        [InlineData("((A|B)&(¬C))", "(6, 4, 2) |")]
        public void FDNFNumericString_shouldEqual(string input, string expected)
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            LogicalExpression expr = new(input);

            var FDNF = expr.FDNF.ToNFNumericString(NormalForms.FDNF);

            Assert.Equal(expected, FDNF);
        }

        [Theory]
        [InlineData("A123")]
        [InlineData("B")]
        [InlineData("C1")]
        [InlineData("((A1|(A2&A3))|B1)")]
        [InlineData("((A1|F)|T)")]
        [InlineData("(¬(¬A123))")]
        [InlineData("(¬(¬(¬(¬A123))))")]
        [InlineData("((A1|(B2&A3))|B1)")]
        public void GetTruthTable_shouldNotThrow(string input)
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            LogicalExpression expr = new(input);

            var exception = Record.Exception(() => _ = expr.TruthTable);
            Assert.Null(exception);
        }

        [Theory]
        [InlineData("((A1|A2)|A3)", true, true, true, true)]
        [InlineData("((A1|A2)|A3)", false, false, false, false)]
        [InlineData("((A1&A2)|A3)", false, false, true, true)]
        [InlineData("((A1&A2)|A3)", true, true, false, true)]
        [InlineData("((A1&A2)&A3)", true, false, false, false)]
        [InlineData("((A1&A2)&A3)", true, true, true, true)]
        [InlineData("((A1→A2)&A3)", true, false, true, false)]
        [InlineData("((A1→A2)&A3)", false, false, true, true)]
        [InlineData("((A1&A2)~A3)", true, true, true, true)]
        [InlineData("((A1&A2)~A3)", false, false, false, true)]
        [InlineData("((¬A1)&A2)|A3)", false, true, false, true)]
        [InlineData("((¬A1)&A2)|A3)", true, true, false, false)]
        public void Evaluation_shouldEqualExprected(string input, bool A1, bool A2, bool A3, bool result)
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            LogicalExpression expr = new(input);
            expr.SetVariable("A1", A1);
            expr.SetVariable("A2", A2);
            expr.SetVariable("A3", A3);

            var actualResult = expr.Evaluation;

            Assert.Equal(result, actualResult);
        }

        [Theory]
        [InlineData("(1)")]
        [InlineData("(A023)")]
        [InlineData("(A1B)")]
        [InlineData("(A1=B1)")]
        [InlineData("(A1&)")]
        [InlineData("(¬)")]
        [InlineData("(A1&(|C1))")]
        public void Parsing_IncorrectNotation_shouldThrow(string input)
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            Assert.Throws<ArgumentException>(() => new LogicalExpression(input));
        }

        [Fact]
        public void SettingVariable_VariableChangesValue_ShouldResetEvaluation()
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            LogicalExpression expr = new("((A1&A2)~A3)");
            expr.SetVariable("A1", true);
            expr.SetVariable("A2", false);
            expr.SetVariable("A3", false);
            var oldResult = expr.Evaluation;

            expr.SetVariable("A2", true);

            Assert.NotEqual(oldResult, expr.Evaluation);
        }
    }
}