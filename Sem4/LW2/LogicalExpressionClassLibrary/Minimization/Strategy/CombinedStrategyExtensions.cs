using LogicalExpressionClassLibrary.LogicalExpressionTree;
using System.Text;

namespace LogicalExpressionClassLibrary.Minimization.Strategy
{
    public static class CombinedStrategyExtensions
    {
        public static bool Contains(this TreeNode constituent, TreeNode implicant, NormalForms form)
        {
            var vars1 = MinimizationHelper.VariablesInvertion(constituent, form);
            var vars2 = MinimizationHelper.VariablesInvertion(implicant, form);

            foreach (var var2 in vars2)
            {
                if (vars1.TryGetValue(var2.Key, out bool val1))
                {
                    if (val1 != vars2[var2.Key])
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static string ToTableString(this Dictionary<string, Dictionary<string, bool>> table)
        {
            const int ColumnWidth = 20;

            StringBuilder builder = new();

            int columnCount = table.Values.First().Keys.Count;
            string separator = string.Empty.PadRight((columnCount + 1) * (ColumnWidth + 1) + 1, '-') + '\n';

            builder
                .Append(separator)
                .Append('|')
                .Append(string.Empty.PadRight(ColumnWidth))
                .Append('|');

            foreach (var constituent in table.Values.First().Keys)
            {
                builder.Append(constituent.PadRight(ColumnWidth)[..ColumnWidth] + '|');
            }
            builder.Append('\n').Append(separator);

            foreach (var kvp in table)
            {
                string key = '|' + kvp.Key.ToString().PadRight(ColumnWidth)[..ColumnWidth];
                builder.Append(key + '|');

                foreach (var v in kvp.Value.Values)
                {
                    string columnValue =
                        (string.Empty.PadRight(ColumnWidth / 2 - 1) +
                        (v ? "X" : null)).PadRight(ColumnWidth)[..ColumnWidth] + '|';
                    builder.Append(columnValue);
                }
                builder.Append('\n');
            }

            builder.Append(separator);
            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }
    }
}
