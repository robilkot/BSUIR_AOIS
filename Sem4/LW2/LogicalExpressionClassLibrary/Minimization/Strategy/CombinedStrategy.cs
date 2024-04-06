using LogicalExpressionClassLibrary.LogicalExpressionTree;

namespace LogicalExpressionClassLibrary.Minimization.Strategy
{
    public class CombinedStrategy : IMinimizationStrategy
    {
        public LogicalExpression Minimize(LogicalExpression input, NormalForms form)
        {
            LogicalExpression source = form switch
            {
                NormalForms.FCNF => input.FCNF,
                NormalForms.FDNF => input.FDNF,
                _ => throw new NotImplementedException()
            };

            var merged = source.MergeConstituents(form);

            var constituents = MinimizationHelper.GetConstituents(source, form);
            var implicants = MinimizationHelper.GetConstituents(merged, form);


            Dictionary<string, Dictionary<string, bool>> table = [];

            // rows
            foreach (var implicant in implicants)
            {
                string implicantNotation = implicant.ToString()!;
                
                if (table.TryAdd(implicantNotation, []))
                {
                    // columns
                    foreach (var constituent in constituents)
                    {
                        string constituentNotation = constituent.ToString()!;
                        table[implicantNotation].Add(constituentNotation, constituent.Contains(implicant, form));
                    }
                } else
                {
                    ConsoleLogger.Log("Implicant row already exists in table. Please report", ConsoleLogger.DebugLevels.Warning);
                }
            }

            static bool tableIsValid(Dictionary<string, Dictionary<string, bool>> table)
            {
                Dictionary<string, int> containments = [];

                foreach (var dict in table.Values)
                {
                    foreach (var kvp in dict)
                    {
                        if (containments.ContainsKey(kvp.Key))
                        {
                            containments[kvp.Key] += kvp.Value ? 1 : 0;
                        }
                        else
                        {
                            containments.Add(kvp.Key, kvp.Value ? 1 : 0);
                        }
                    }
                }

                return containments.Values.All(value => value != 0);
            }

            ConsoleLogger.Log($"Minimization table:\n{table.ToTableString()}", ConsoleLogger.DebugLevels.Info);

            Dictionary<string, Dictionary<string, bool>> currentTable = new(table);
            List<TreeNode> oddNodes = [];

            foreach (var implicant in implicants)
            {
                var currentRow = currentTable[implicant.ToString()!];

                currentTable.Remove(implicant.ToString()!);

                if (tableIsValid(currentTable))
                {
                    oddNodes.Add(implicant);

                    ConsoleLogger.Log($"Found odd implicant: {implicant}", ConsoleLogger.DebugLevels.Debug);
                }
                else
                {
                    currentTable.Add(implicant.ToString()!, currentRow);
                }
            }

            if (oddNodes.Count == 0 || oddNodes.Count == implicants.Count)
            {
                ConsoleLogger.Log($"No odd implicants found", ConsoleLogger.DebugLevels.Debug);
            } else
            {
                foreach (var node in oddNodes)
                {
                    ConsoleLogger.Log($"Removing odd implicant {node}", ConsoleLogger.DebugLevels.Debug);
                    implicants.Remove(node);
                }
            }
            HashSet<string> nodes = implicants.Select(i => i.ToString()!).ToHashSet();

            return MinimizationHelper.BuildNFFromStringSet(nodes, form);
        }
    }
}
