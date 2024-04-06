using LogicalExpressionClassLibrary.LogicalExpressionTree;
using LogicalExpressionClassLibrary.LogicalExpressionTree.OperationNodes;
using LogicalExpressionClassLibrary.LogicalExpressionTree.ValueNodes;

namespace LogicalExpressionClassLibrary.LogicalParser
{
    public abstract class AbstractLogicalParser
    {
        public static readonly Dictionary<LogicalSymbols, Func<TreeNode, TreeNode, TreeNode>> _binaryOperatorsNodes = new()
            {
                { LogicalSymbols.Conjunction, (left, right) => new ConjunctionNode(left, right) },
                { LogicalSymbols.Disjunction, (left, right) => new DisjunctionNode(left, right) },
                { LogicalSymbols.Implication, (left, right) => new ImplicationNode(left, right) },
                { LogicalSymbols.Equality,    (left, right) => new EqualityNode(left, right) },
            };

        protected static readonly Dictionary<LogicalSymbols, TreeNode> _constNodes = new()
            {
                { LogicalSymbols.True, TrueNode.GetInstance() },
                { LogicalSymbols.False, FalseNode.GetInstance() },
            };
        public abstract (TreeNode root, Dictionary<string, List<AtomicFormulaNode>> variables) Parse(string input);
    }
}