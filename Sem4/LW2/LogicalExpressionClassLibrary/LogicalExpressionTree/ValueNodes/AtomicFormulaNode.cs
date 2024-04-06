namespace LogicalExpressionClassLibrary.LogicalExpressionTree.ValueNodes
{
    public sealed class AtomicFormulaNode : TreeNode
    {
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (!ValidateName(value))
                {
                    ConsoleLogger.Log($"Invalid atomic formula", ConsoleLogger.DebugLevels.Error);
                    throw new ArgumentException("Invalid atomic formula");
                }

                _name = value;
            }
        }

        private bool _value = false;
        public bool Value

        {
            get => _value;
            set
            {
                ConsoleLogger.Log($"Set {Name} to {value}", ConsoleLogger.DebugLevels.Debug);

                if (_value != value)
                {
                    ClearEvaluation();
                    _value = value;
                }
            }
        }

        public AtomicFormulaNode(string name) : base(null, null)
        {
            Name = name;
        }
        public AtomicFormulaNode(AtomicFormulaNode other) : base(null, null)
        {
            _name = other._name;
        }
        protected override bool Evaluate() => Value;

        private static bool ValidateName(string name)
        {
            // First symbol must be a letter
            if (name.Length == 0 || !char.IsLetter(name[0]))
            {
                return false;
            }
            if (name.Length > 1)
            {
                if (name[1] == '0')
                {
                    return false;
                }

                for (int i = 1; i < name.Length; i++)
                {
                    if (!char.IsDigit(name[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public override string ToString() => Name;
    }
}