namespace LogicalExpressionClassLibrary.LogicalExpressionTree.ValueNodes
{
    public sealed class FalseNode : TreeNode
    {
        private static readonly FalseNode _instance = new();
        private FalseNode() : base(null, null) { }
        public static FalseNode GetInstance() => _instance;
        protected override bool Evaluate() => false;
        public override string ToString() => $"{(char)LogicalSymbols.False}";
    }
}