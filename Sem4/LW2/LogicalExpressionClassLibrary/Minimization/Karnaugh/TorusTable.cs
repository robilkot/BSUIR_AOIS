namespace LogicalExpressionClassLibrary.Minimization.Karnaugh
{
    public class TorusTable(bool[][][] table)
    {
        private readonly bool[][][] _array = table;
        private readonly int _depth = table.Length;
        private readonly int _height = table[0].Length;
        private readonly int _width = table[0][0].Length;

        public int Depth => _depth;
        public int Width => _width;
        public int Height => _height;
        public bool At((int k, int i, int j) point) => At(point.k, point.i, point.j);
        public bool At(int k, int i, int j)
        {
            (int kCoord, int iCoord, int jCoord) = Normalize((k, i, j));
            return _array[kCoord][iCoord][jCoord];
        }

        public (int, int, int) Normalize((int k, int i, int j) point)
        {
            (point.i, point.j) = Normalize2D((point.i, point.j));
            if (Math.Abs(point.k) >= _depth)
                point.k = point.k - (point.k / _depth) * _depth;
            if (point.k < 0)
                point.k = _depth + point.k;
            return (point.k, point.i, point.j);
        }

        private (int, int) Normalize2D((int i, int j) point)
        {
            if (Math.Abs(point.i) >= _height)
                point.i -= (point.i / _height) * _height;
            if (point.i < 0)
                point.i = _height + point.i;

            if (Math.Abs(point.j) >= _width)
                point.j -= (point.j / _width) * _width;
            if (point.j < 0)
                point.j = _width + point.j;

            return (point.i, point.j);
        }
    }
}


