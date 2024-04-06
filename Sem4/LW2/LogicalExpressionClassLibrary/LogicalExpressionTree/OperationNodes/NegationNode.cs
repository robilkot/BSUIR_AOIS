using System.Xml.Linq;

namespace LogicalExpressionClassLibrary.LogicalExpressionTree.OperationNodes
{
    public sealed class NegationNode(TreeNode? left) : TreeNode(left, null)
    {
        protected override bool Evaluate() => !Left!.Evaluation;
        public override string ToString()
            => $"{(char)LogicalSymbols.LeftBracket}{(char)LogicalSymbols.Negation}{Left}{(char)LogicalSymbols.RightBracket}";
    }
}
