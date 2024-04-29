using LabLogger;
using LogicalExpressionClassLibrary.LogicalExpressionTree;
using LogicalExpressionClassLibrary.LogicalExpressionTree.OperationNodes;
using System.Security;

namespace LogicalExpressionClassLibrary.Minimization
{
    public static class MinimizationHelper
    {
        public static Dictionary<string, bool> VariablesInvertion(TreeNode constituent, NormalForms form)
        {
            Dictionary<string, bool> result = [];

            var currentNode = constituent;

            Type targetType = form switch
            {
                NormalForms.FDNF => typeof(ConjunctionNode),
                NormalForms.FCNF => typeof(DisjunctionNode),
                _ => throw new NotImplementedException(),
            };
            while (currentNode!.GetType() == targetType)
            {
                if (currentNode.Right is NegationNode)
                {
                    result.Add(currentNode.Right!.Left!.ToString()!, false);
                }
                else
                {
                    result.Add(currentNode.Right!.ToString()!, true);
                }

                currentNode = currentNode.Left;
            }

            if (currentNode is NegationNode)
            {
                result.Add(currentNode.Left!.ToString()!, false);
            }
            else
            {
                result.Add(currentNode.ToString()!, true);
            }

            return result;
        }
        public static HashSet<(string variable, bool inverted)> GetVariables(TreeNode constituent, NormalForms form)
        {
            HashSet<(string variable, bool inverted)> result = [];

            var currentNode = constituent;

            Type targetType = form switch
            {
                NormalForms.FDNF => typeof(ConjunctionNode),
                NormalForms.FCNF => typeof(DisjunctionNode),
                _ => throw new NotImplementedException(),
            };
            while (currentNode!.GetType() == targetType)
            {
                if (currentNode.Right is NegationNode)
                {
                    result.Add((currentNode.Right!.Left!.ToString()!, true));
                }
                else
                {
                    result.Add((currentNode.Right!.ToString()!, false));
                }

                currentNode = currentNode.Left;
            }

            if (currentNode is NegationNode)
            {
                result.Add((currentNode.Left!.ToString()!, true));
            }
            else
            {
                result.Add((currentNode.ToString()!, false));
            }

            return result;
        }
        public static List<TreeNode> GetConstituents(LogicalExpression input, NormalForms form)
        {
            List<TreeNode> constituents = [];

            var currentNode = input.Root;

            Type targetType = form switch
            {
                NormalForms.FDNF => typeof(DisjunctionNode),
                NormalForms.FCNF => typeof(ConjunctionNode),
                _ => throw new NotImplementedException()
            };
            while (currentNode!.GetType() == targetType)
            {
                constituents.Add(currentNode.Right!);
                currentNode = currentNode.Left;
            }

            constituents.Add(currentNode!);

            return constituents;
        }
        public static LogicalExpression BuildNFFromStringSet(HashSet<string> constituents, NormalForms form)
        {
            if(constituents.Count == 0)
            {
                throw new InvalidOperationException($"Cannot build {form}");
            }
            LogicalExpression result = LogicalExpression.Empty;

            Func<TreeNode, TreeNode> func = form switch
            {
                NormalForms.FCNF => (node) => new ConjunctionNode(null, node),
                NormalForms.FDNF => (node) => new DisjunctionNode(null, node),
                _ => throw new NotImplementedException()
            };

            TreeNode root = null!;
            TreeNode current = root;

            foreach (var constituent in constituents)
            {
                LogicalExpression node = new(constituent);

                // Set variables dictionary for new expression based on variables used in constituents
                foreach (var variable in node.Variables)
                {
                    if(result.Variables.TryGetValue(variable.Key, out var existingValue))
                    {
                        foreach(var newNode in variable.Value)
                        {
                            if(!existingValue.Contains(newNode))
                            {
                                existingValue.Add(newNode);
                            }
                        }
                    } else
                    {
                        result.Variables.Add(variable.Key, variable.Value);
                    }
                }

                if (root is null)
                {
                    root = func(node.Root!);
                    current = root;
                }
                else
                {
                    current.Left = func(node.Root!);
                    current.Left.Parent = current;
                    current.Right.Parent = current;
                    current = current.Left;
                }

            }

            if (current.Parent is not null)
            {
                current.Parent!.Left = current.Right;
                current.Right!.Parent = current.Parent;
            }
            else
            {
                current = current.Right!;
                root = current;
            }

            result.Root = root;
            return result;
        }
        public static LogicalExpression MergeConstituents(this LogicalExpression input, NormalForms form)
        {
            static List<(TreeNode, TreeNode)> formPairs(List<TreeNode> constituents)
            {
                List<(TreeNode, TreeNode)> pairs = [];
                HashSet<TreeNode> seenNodes = [];

                foreach (var node1 in constituents)
                {
                    foreach (var node2 in constituents)
                    {
                        if (!seenNodes.Contains(node2))
                        {
                            if (node1 != node2)
                            {
                                pairs.Add((node1, node2));
                                seenNodes.Add(node1);
                            }
                        }
                    }
                }

                return pairs;
            }

            for (int identicalVariablesCount = input.Variables.Count - 1; identicalVariablesCount > 0; identicalVariablesCount--)
            {
                var constituents = GetConstituents(input, form);

                var pairs = formPairs(constituents);

                HashSet<string> mergedConstituents = [];
                HashSet<string> yetNotProcessedConstituents = constituents.Select(c => c.ToString()).ToHashSet()!;

                foreach (var pair in pairs)
                {
                    var variables1 = VariablesInvertion(pair.Item1, form);
                    var variables2 = VariablesInvertion(pair.Item2, form);

                    // Continue if trying to merge two different sized constituents
                    if (variables1.Count != variables2.Count) continue;

                    string? differentVariable = null;
                    bool pairNotSuitableForMerge = false;

                    foreach (var variable1 in variables1)
                    {
                        if (variables2.TryGetValue(variable1.Key, out bool secondVal))
                        {
                            if (secondVal != variable1.Value)
                            {
                                if (differentVariable is not null)
                                {
                                    pairNotSuitableForMerge = true;
                                    break;
                                }

                                differentVariable = variable1.Key;
                            }
                        }
                        else
                        {
                            // If we have pair of same sized constituents but with different variables
                            pairNotSuitableForMerge = true;
                            break;
                        }
                    }

                    if (pairNotSuitableForMerge || differentVariable is null) continue;

                    // todo: figure out why this line breaks everything
                    //if (!(yetNotProcessedConstituents.Contains(pair.Item1.ToString()!)
                    //    && yetNotProcessedConstituents.Contains(pair.Item2.ToString()!))) continue;

                    string substringToSearch = string.Empty;

                    if (variables1[differentVariable!])
                    {
                        substringToSearch = differentVariable!;
                    }
                    else
                    {
                        substringToSearch =
                            ((char)LogicalSymbols.LeftBracket).ToString() +
                            (char)LogicalSymbols.Negation +
                            differentVariable +
                            (char)LogicalSymbols.RightBracket;
                    }

                    var item1String = pair.Item1.ToString();
                    int foundPosition = item1String!.IndexOf(substringToSearch)!;
                    string pairToAdd = item1String!.Replace(substringToSearch, string.Empty);

                    // Remove operator if found
                    char innerOperand = form switch
                    {
                        NormalForms.FCNF => (char)LogicalSymbols.Disjunction,
                        NormalForms.FDNF => (char)LogicalSymbols.Conjunction,
                        _ => throw new NotImplementedException()
                    };
                    // todo: remove odd brackets
                    if (pairToAdd[foundPosition] == innerOperand)
                    {
                        pairToAdd = pairToAdd.Remove(foundPosition, 1);
                    }
                    else if (pairToAdd[foundPosition - 1] == innerOperand)
                    {
                        pairToAdd = pairToAdd.Remove(foundPosition - 1, 1);
                    }

                    mergedConstituents.Add(pairToAdd);

                    yetNotProcessedConstituents.Remove(pair.Item1.ToString()!);
                    yetNotProcessedConstituents.Remove(pair.Item2.ToString()!);

                    Logger.Log($"Merged {pair.Item1} and {pair.Item2} to {pairToAdd}", Logger.Levels.Info);
                }
                foreach (var constituent in yetNotProcessedConstituents)
                {
                    mergedConstituents.Add(constituent);
                }

                input = BuildNFFromStringSet(mergedConstituents, form);
            }

            Logger.Log($"Expression after merging: {input}", Logger.Levels.Info);

            return input;
        }
    }
}
