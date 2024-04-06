namespace LogicalExpressionClassLibrary.LogicalExpressionTree
{
    public abstract class TreeNode
    {
        private TreeNode? _right = null;
        public TreeNode? Right
        {
            get => _right;
            set
            {
                ClearEvaluation();
                _right = value;
            }
        }
        private TreeNode? _left = null;
        public TreeNode? Left
        {
            get => _left;
            set
            {
                ClearEvaluation();
                _left = value;
            }
        }
        public TreeNode? Parent { get; set; } = null;
        // Serves caching purpose and is used in truth tables
        private bool? _evaluation = null;
        public bool Evaluation
        {
            get
            {
                if (_evaluation == null)
                {
                    _evaluation = Evaluate();

                    ConsoleLogger.Log($"Evaluate {ToString()}", ConsoleLogger.DebugLevels.Debug);
                }
                return _evaluation.Value;
            }
        }
        public TreeNode(TreeNode? left, TreeNode? right)
        {
            Right = right;
            Left = left;
        }
        protected abstract bool Evaluate();
        protected void ClearEvaluation()
        {
            if (_evaluation != null)
            {
                string logStringAddition = Parent is null ? string.Empty : $" with parent {Parent}";
                ConsoleLogger.Log($"Clear {GetType().Name}" + logStringAddition, ConsoleLogger.DebugLevels.Debug);

                _evaluation = null;
                // If value is outdated then upper tree part is also outdated
                Parent?.ClearEvaluation();
            }
        }
    }
}
