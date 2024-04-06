using System.Drawing;

namespace LogicalExpressionClassLibrary.Minimization.Karnaugh
{
    public abstract class Karnaugh(NormalForms normalForm)
    {
        protected static readonly ConsoleColor SelectedAreasColor = ConsoleColor.Green;
        private const int MaxVariables = 5;
        protected TorusTable _table = null!;
        protected List<List<(int, int, int)>> _areas = null!;
        protected List<string> _vars = null!;
        protected NormalForms _form = normalForm;
        protected bool TargetValue
        {
            get => _form switch
            {
                NormalForms.FDNF => true,
                NormalForms.FCNF => false,
                _ => throw new NotImplementedException(),
            };
        }

        public static Karnaugh CreateKarnaugh(LogicalExpression expression, NormalForms form)
        {
            return expression.Variables.Count switch
            {
                0 => throw new InvalidOperationException("No variables in expression"),
                1 => new Karnaugh1(expression, form),
                2 => new Karnaugh2(expression, form),
                3 => new Karnaugh3(expression, form),
                4 => new Karnaugh4(expression, form),
                5 => new Karnaugh5(expression, form),
                _ => throw new NotImplementedException("Karnaugh maps are not implemented for more than 5 variables"),
            };
        }

        protected List<List<(int, int, int)>> _zone = null!;
        protected List<List<(int, int, int)>> CreateZone()
        {
            List<List<(int, int, int)>> zone = [];

            for (int pos = 0; pos < MaxVariables; pos++)
            {
                List<(int, int, int)> sublist = [];

                for (int k = 0; k <= 1; k++)
                {
                    for (int i = 0; i <= 3; i++)
                    {
                        for (int j = 0; j <= 3; j++)
                        {
                            if (IsZone((k, i, j), pos))
                            {
                                sublist.Add((k, i, j));
                            }
                        }
                    }
                }
                zone.Add(sublist);
            }
            return zone;
        }

        protected abstract bool IsZone((int k, int i, int j) point, int pos);

        protected List<List<(int, int, int)>> GetAreas()
        {
            var result = new List<List<(int, int, int)>>();

            for (int k = 0; k < _table.Depth; k++)
            {
                for (int i = 0; i < _table.Height; i++)
                {
                    for (int j = 0; j < _table.Width; j++)
                    {
                        if (_table.At((k, i, j)) == TargetValue)
                        {
                            result.Add([(k, i, j)]);
                            Traverse(result, (k, i, j));
                        }
                    }
                }
            }
            return RemoveSubsets(result);
        }
        protected static Dictionary<string, bool> GenerateAdditionalTruthTable(LogicalExpression expression)
        {
            Dictionary<string, bool> result = [];

            foreach (var row in expression.TruthTable)
            {
                string currentKey = string.Empty;

                foreach (var variable in expression.Variables.Keys)
                {
                    currentKey += row[variable] ? '1' : '0';
                }

                result.TryAdd(currentKey, row[expression.ToString()]);
            }

            return result;
        }

        public HashSet<string> GetConstituents()
        {
            var result = new HashSet<string>();

            foreach (var area in _areas)
            {
                result.Add(GetConstituentString(area));
            }
            return result;
        }

        protected string GetConstituentString(List<(int, int, int)> point)
        {
            string constituent = string.Empty;
            char innerOperator = _form switch
            {
                NormalForms.FCNF => (char)LogicalSymbols.Disjunction,
                NormalForms.FDNF => (char)LogicalSymbols.Conjunction,
                _ => throw new NotImplementedException()
            };

            bool? isInZone;

            for (int i = 0; i < _vars.Count; i++)
            {
                isInZone = IsInZone(point, _zone[i]);
                if (isInZone is not null)
                {
                    if (_form == NormalForms.FCNF)
                    {
                        isInZone = !isInZone;
                    }

                    string temp = _vars[i];
                    if (!isInZone.Value)
                    {
                        temp = (char)LogicalSymbols.Negation + temp;
                    }
                    temp = $"{(char)LogicalSymbols.LeftBracket}{temp}{(char)LogicalSymbols.RightBracket}";

                    if (constituent.Length == 0)
                    {
                        constituent = temp;
                    }
                    else
                    {
                        constituent = $"{(char)LogicalSymbols.LeftBracket}{temp}{innerOperator}{constituent}{(char)LogicalSymbols.RightBracket}";
                    }
                }
            }

            return constituent;
        }

        protected static bool? IsInZone(List<(int, int, int)> area, List<(int, int, int)> zone)
        {
            if (IsSubset(area, zone))
                return true;
            if (HaveNoCommonElements(area, zone))
                return false;
            return null;
        }
        public abstract void PrintTable();

        protected static List<List<(int, int, int)>> RemoveSubsets(List<List<(int, int, int)>> lists)
        {
            var result = new List<List<(int, int, int)>>();
            for (int i = 0; i < lists.Count; i++)
            {
                bool isSubset = false;
                for (int j = 0; j < lists.Count; j++)
                {
                    if (i == j)
                        continue;
                    if (i > j && lists[j].Count == lists[i].Count && lists[i].All(lists[j].Contains))
                    {
                        if (i < j)
                        {
                            isSubset = true;
                            continue;
                        }
                        else continue;
                    }
                    if (IsSubset(lists[i], lists[j]))
                    {
                        isSubset = true;
                        break;
                    }
                }
                if (!isSubset)
                    result.Add(lists[i]);
            }

            int count;
            do
            {
                var dict = new Dictionary<(int, int, int), int>();
                foreach (var i in result)
                {
                    foreach (var j in i)
                    {
                        if (dict.TryGetValue(j, out int value))
                            dict[j] = ++value;
                        else
                            dict[j] = 1;
                    }
                }
                count = result.Count;
                List<(int, int, int)>? to_delete = null;
                foreach (var i in result)
                {
                    bool isSubset = true;
                    foreach (var j in i)
                    {
                        if (dict[j] == 1)
                        {
                            isSubset = false;
                            break;
                        }
                    }
                    if (isSubset)
                    {
                        to_delete = i;
                        break;
                    }
                }
                if (to_delete != null)
                {
                    result.Remove(to_delete);
                }
            }
            while (count != result.Count);
            return result;
        }
        private static bool IsSubset(List<(int, int, int)> list1, List<(int, int, int)> list2) => list1.All(list2.Contains);
        private static bool HaveNoCommonElements(List<(int, int, int)> list1, List<(int, int, int)> list2) => !list2.Any(list1.Contains);
        protected List<(int, int, int)> RemoveDoubles(List<(int, int, int)> rectangle)
        {
            rectangle = rectangle.Distinct().ToList();

            var normalizedRectangle = new List<(int, int, int)>();

            foreach (var item in rectangle)
            {
                var normalizedItem = _table.Normalize(item);
                normalizedRectangle.Add(normalizedItem);
            }

            return normalizedRectangle;
        }

        protected bool RectangleIsValid(List<(int, int, int)> rectanglePoints) => !rectanglePoints.Any(p => _table.At(p) != TargetValue);
        protected static List<(int, int, int)> Increase((int k, int i, int j) coords, int offset_k, int offset_i, int offset_j)
        {
            var result = new List<(int, int, int)>();

            for (int k = coords.k; k < coords.k + offset_k; k++)
            {
                for (int i = coords.i; i < coords.i + offset_i; i++)
                {
                    for (int j = coords.j; j < coords.j + offset_j; j++)
                    {
                        result.Add((k, i, j));
                    }
                }
            }
            return result;
        }

        protected abstract void Traverse(List<List<(int, int, int)>> result, (int k, int i, int j) point);

        protected void Traverse1(List<List<(int, int, int)>> result, (int k, int i, int j) point)
        {
            (int k, int i, int j)[] coords =
            [
                (1, 1, 2),
            ];

            TraverseHelper(coords, result, point);
        }

        protected void Traverse2(List<List<(int, int, int)>> result, (int k, int i, int j) point)
        {
            (int k, int i, int j)[] coords =
            [
                (1, 2, 1),
                (1, 2, 2),
            ];

            TraverseHelper(coords, result, point);
        }

        protected void Traverse3(List<List<(int, int, int)>> result, (int k, int i, int j) point)
        {
            (int k, int i, int j)[] coords =
            [
                (2,1,1),
                (2,2,1),
                (2,1,2),
                (2,2,2),
            ];

            TraverseHelper(coords, result, point);
        }

        protected void Traverse4(List<List<(int, int, int)>> result, (int k, int i, int j) point)
        {
            (int k, int i, int j)[] coords =
            [
                (1, 1, 4),
                (1, 2, 4),
                (2, 1, 4),
                (2, 2, 4),
            ];

            TraverseHelper(coords, result, point);
        }

        protected void Traverse5(List<List<(int, int, int)>> result, (int k, int i, int j) point)
        {
            (int k, int i, int j)[] coords =
            [
                (1, 4, 1),
                (1, 4, 2),
                (2, 4, 1),
                (2, 4, 2),
                (2, 4, 4),
            ];

            TraverseHelper(coords, result, point);
        }

        private void TraverseHelper((int k, int i, int j)[] coords, List<List<(int, int, int)>> result, (int k, int i, int j) point)
        {
            foreach (var (k, i, j) in coords)
            {
                List<(int, int, int)> temp;
                temp = RemoveDoubles(Increase((point.k, point.i, point.j), k, i, j));
                if (RectangleIsValid(temp))
                    result.Add(temp);
            }
        }
    }
}