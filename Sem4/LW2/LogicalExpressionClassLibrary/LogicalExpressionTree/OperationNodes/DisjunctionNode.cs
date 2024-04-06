namespace LogicalExpressionClassLibrary.LogicalExpressionTree.OperationNodes
{
    public sealed class DisjunctionNode(TreeNode? left, TreeNode? right) : TreeNode(left, right)
    {
        protected override bool Evaluate() => Left!.Evaluation || Right!.Evaluation;
        public override string ToString()
            => $"{(char)LogicalSymbols.LeftBracket}{Left}{(char)LogicalSymbols.Disjunction}{Right}{(char)LogicalSymbols.RightBracket}";
    }
}