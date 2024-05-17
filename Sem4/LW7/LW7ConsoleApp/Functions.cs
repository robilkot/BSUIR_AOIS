namespace LW7ConsoleApp
{
    public static class Functions
    {
        public static readonly Func<bool, bool, bool> Function1 =
            (bool x1, bool x2) =>
            {
                return x1 && x2;
            };

        public static readonly Func<bool, bool, bool> Function2 =
            (bool x1, bool x2) =>
            {
                return x1 || x2;
            };

        public static readonly Func<bool, bool, bool> Function3 =
            (bool x1, bool x2) =>
            {
                return x1 ^ x2;
            };

        public static readonly Func<bool, bool, bool> Function4 =
            (bool x1, bool x2) =>
            {
                return !x1 && !x2;
            };

        public static readonly Func<bool, bool, bool> Function5 =
            (bool x1, bool x2) =>
            {
                return !x1;
            };
    }
}
