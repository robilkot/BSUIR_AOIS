using LogicalExpressionClassLibrary.LogicalExpressionTree;
using LogicalExpressionClassLibrary.LogicalExpressionTree.OperationNodes;
using LogicalExpressionClassLibrary.LogicalExpressionTree.ValueNodes;

namespace LogicalExpressionClassLibrary.LogicalParser
{
    public sealed class RecursiveLogicalParser : AbstractLogicalParser
    {
        private readonly Dictionary<string, List<AtomicFormulaNode>> _variables = [];
        private static string ExtractVariableName(string input, ref int runningIndex)
        {
            int beginIndex = runningIndex;

            // Account for letter-only atomic formulas
            if (beginIndex == input.Length - 1)
            {
                return input[beginIndex..(beginIndex + 1)];
            }

            int endIndex = beginIndex;

            if (input[++endIndex] == '0')
            {
                throw new ArgumentException("Atomic formula's first digit can't be zero");
            }

            while (endIndex < input.Length && char.IsDigit(input[endIndex]))
            {
                endIndex++;
            }

            // Account for outer increment in parser for loop
            runningIndex = endIndex - 1;

            return input[beginIndex..endIndex];
        }
        public override (TreeNode, Dictionary<string, List<AtomicFormulaNode>>) Parse(string input)
        {
            _variables.Clear();

            int runningIndex = 0;

            var root = BuildFormulaTree(input, ref runningIndex, null);

            return (root, new(_variables));
        }
        
        private TreeNode BuildFormulaTree(string input, ref int i, TreeNode? parentNode)
        {
            TreeNode toReturn = null!;

            int indent = 0;

            for (; i < input.Length; i++)
            {
                // Encountered symbol is variable or const
                if (char.IsLetter(input[i]))
                {
                    var variableName = ExtractVariableName(input, ref i);
                 
                    if(toReturn is not null)
                    {
                        throw new ArgumentException($"Unexpected variable '{variableName}' in expression notation");
                    }

                    if (variableName.Length == 1 && Enum.IsDefined(typeof(LogicalSymbols), (int)variableName[0]))
                    {
                        toReturn = _constNodes[(LogicalSymbols)variableName[0]];
                    }
                    else
                    {
                        var atomicFormulaNode = new AtomicFormulaNode(variableName);

                        // Store variable name association with node for expression
                        if (_variables.TryGetValue(variableName, out var alreadyCreatedList))
                        {
                            alreadyCreatedList.Add(atomicFormulaNode);
                        }
                        else
                        {
                            _variables.Add(variableName, [atomicFormulaNode]);
                        }

                        toReturn = atomicFormulaNode;
                    }
                }

                // Encountered symbol is unary (negation) operator
                else if (input[i] == (char)LogicalSymbols.Negation)
                {
                    i++;

                    // Create object here to pass it as argument further. Child is set below
                    toReturn = new NegationNode(null);

                    var childFormula = BuildFormulaTree(input, ref i, toReturn);

                    toReturn.Left = childFormula;

                    break;
                }

                // Encountered symbol is binary operator
                else if (_binaryOperatorsNodes.TryGetValue((LogicalSymbols)input[i], out var nodeGenerator))
                {
                    if (toReturn == null)
                    {
                        string exceptionCommentary = input.Insert(i, "__?__");
                        throw new ArgumentException($"Missing operand before '{input[i]}': {exceptionCommentary}");
                    }

                    i++;

                    var newNode = nodeGenerator(toReturn, null!);

                    toReturn.Parent = newNode;

                    toReturn = newNode;

                    toReturn.Right = BuildFormulaTree(input, ref i, toReturn);

                    break;
                }

                else if (input[i] == (char)LogicalSymbols.LeftBracket)
                {
                    indent++;

                    if (indent > 0)
                    {
                        i++;

                        var newNode = BuildFormulaTree(input, ref i, toReturn);

                        toReturn = newNode;
                    }
                }
                else if (input[i] == (char)LogicalSymbols.RightBracket)
                {
                    indent--;

                    // Stop parsing inner expression
                    if (indent <= 0)
                    {
                        break;
                    }
                }
                else if (input[i] == ' ')
                {
                    continue;
                }
                else
                {
                    throw new ArgumentException($"Unexpected token '{input[i]}' in expression notation");
                }
            }

            if(toReturn == null)
            {
                throw new ArgumentException("Missing token in expression notation");
            }
            toReturn.Parent = parentNode;

            return toReturn;
        }
    }
}
