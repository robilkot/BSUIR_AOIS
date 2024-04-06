using LogicalExpressionClassLibrary;
using LogicalExpressionClassLibrary.LogicalExpressionTree;
using LogicalExpressionClassLibrary.LogicalExpressionTree.ValueNodes;
using System.Diagnostics.CodeAnalysis;

namespace LogicalExpressionUnitTest
{
    [ExcludeFromCodeCoverage]
    public class TreeNodeTest
    {
        [Theory]
        [InlineData("A023")]
        [InlineData("1B")]
        [InlineData("")]
        [InlineData("(")]
        public void AtomicFormulaNode_IncorrectName_shouldThrow(string input)
        {
            ConsoleLogger.DebugLevel =
                ConsoleLogger.DebugLevels.Debug | ConsoleLogger.DebugLevels.Info |
                ConsoleLogger.DebugLevels.Warning | ConsoleLogger.DebugLevels.Error;

            Assert.Throws<ArgumentException>(() => new AtomicFormulaNode(input));
        }
    }
}