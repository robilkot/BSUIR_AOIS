using LogicalExpressionClassLibrary.LogicalExpressionTree.ValueNodes;

namespace LogicalExpressionClassLibrary.Minimization.Karnaugh
{
    class Karnaugh3 : Karnaugh
    {
        private readonly string[][][] _template =
        [
            [
                ["011", "111"],
                ["001", "101"]
            ],
            [
                ["010", "110"],
                ["000", "100"]
            ]
        ];

        private readonly (int, int, int)[][] _2dTemplate =
        [
            [(1, 1, 0), (0, 1, 0), (0, 0, 0), (1, 0, 0),],
            [(1, 1, 1), (0, 1, 1), (0, 0, 1), (1, 0, 1),],
        ];

        protected override bool IsZone((int k, int i, int j) point, int pos)
        {
            return pos switch
            {
                0 => 1 == point.j && 0 <= point.i && point.i <= 1 && 0 <= point.k && point.k <= 1,
                1 => 0 <= point.j && point.j <= 1 && point.i == 0 && 0 <= point.k && point.k <= 1,
                2 => 0 <= point.j && point.j <= 1 && 0 <= point.i && point.i <= 1 && 0 == point.k,
                _ => false,
            };
        }

        public Karnaugh3(LogicalExpression expression, NormalForms form)
            : base(form)
        {
            var helperTruthTable = GenerateAdditionalTruthTable(expression);

            _vars = expression.Variables.Keys.ToList();
            _zone = CreateZone();
            bool[][][] array = new bool[_template.Length][][];
            for (int k = 0; k < _template.Length; k++)
            {
                array[k] = new bool[_template[k].Length][];
                for (int i = 0; i < _template[k].Length; i++)
                {
                    array[k][i] = new bool[_template[k][i].Length];
                    for (int j = 0; j < _template[k][i].Length; j++)
                    {
                        string key = _template[k][i][j];
                        array[k][i][j] = helperTruthTable[key];
                    }
                }
            }
            _table = new TorusTable(array);
            _areas = GetAreas();
        }
        protected override void Traverse(List<List<(int, int, int)>> result, (int k, int i, int j) point)
        {
            Traverse1(result, point);
            Traverse2(result, point);
            Traverse3(result, point);
        }

        public override void PrintTable()
        {
            var variableValues = new List<string>() { { "0" }, { "1" } };
            foreach (var x in _areas)
            {
                string vars = string.Empty;

                foreach (var y in _vars)
                    vars += y.ToString();

                Console.Write(vars.Insert(1, "\\"));
                Console.WriteLine("   00  01  11  10");
                Console.WriteLine("----------------------------------------");
                for (int i = 0; i < _2dTemplate.Length; i++)
                {
                    Console.Write($"{string.Empty.PadLeft(vars.Length)}{variableValues[i]} | ");
                    for (int j = 0; j < _2dTemplate[0].Length; j++)
                    {
                        Console.ForegroundColor = x.Contains(_2dTemplate[i][j]) ? SelectedAreasColor : ConsoleColor.White;
                        Console.Write($"{(_table.At(_2dTemplate[i][j]) ? '1' : '0')}   ");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}
