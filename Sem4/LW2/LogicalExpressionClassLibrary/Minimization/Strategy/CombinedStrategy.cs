using LabLogger;
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
                    Logger.Log("Implicant row already exists in table. Please report", Logger.Levels.Warning);
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

            Logger.Log($"Minimization table:\n{table.ToTableString()}", Logger.Levels.Info);

            Dictionary<string, Dictionary<string, bool>> currentTable = new(table);
            List<TreeNode> oddNodes = [];

            foreach (var implicant in implicants)
            {
                var currentRow = currentTable[implicant.ToString()!];

                currentTable.Remove(implicant.ToString()!);

                if (tableIsValid(currentTable))
                {
                    oddNodes.Add(implicant);

                    Logger.Log($"Found odd implicant: {implicant}", Logger.Levels.Debug);
                }
                else
                {
                    currentTable.Add(implicant.ToString()!, currentRow);
                }
            }

            if (oddNodes.Count == 0 || oddNodes.Count == implicants.Count)
            {
                Logger.Log($"No odd implicants found", Logger.Levels.Debug);
            } else
            {
                foreach (var node in oddNodes)
                {
                    Logger.Log($"Removing odd implicant {node}", Logger.Levels.Debug);
                    implicants.Remove(node);
                }
            }
            HashSet<string> nodes = implicants.Select(i => i.ToString()!).ToHashSet();

            return MinimizationHelper.BuildNFFromStringSet(nodes, form);
        }
    }
}
