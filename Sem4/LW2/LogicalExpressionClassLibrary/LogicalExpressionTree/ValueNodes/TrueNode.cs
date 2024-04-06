namespace LogicalExpressionClassLibrary.LogicalExpressionTree.ValueNodes
{
    public sealed class TrueNode : TreeNode
    {
        private static readonly TrueNode _instance = new();
        private TrueNode() : base(null, null) { }
        public static TrueNode GetInstance() => _instance;
        protected override bool Evaluate() => true;
        public override string ToString() => $"{(char)LogicalSymbols.True}";
    }
}