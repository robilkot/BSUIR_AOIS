using System.Collections.Immutable;

namespace DiagonalMatrixClassLibrary
{
    public enum FunctionsSet
    {
        F2,
        F7,
        F8,
        F13
    }

    public static class Functions
    {
        public static readonly Func<bool, bool, bool> F2 =
            (x1, x2) => x1 && !x2;

        public static readonly Func<bool, bool, bool> F7 =
            (x1, x2) => x1 || x2;

        public static readonly Func<bool, bool, bool> F8 =
            (x1, x2) => !(x1 || x2);

        public static readonly Func<bool, bool, bool> F13 =
            (x1, x2) => !x1 || x2;

        static Functions()
        {
            var builder = ImmutableDictionary.CreateBuilder<FunctionsSet, Func<bool, bool, bool>>();

            builder.Add(new(FunctionsSet.F2, F2));
            builder.Add(new(FunctionsSet.F7, F7));
            builder.Add(new(FunctionsSet.F8, F8));
            builder.Add(new(FunctionsSet.F13, F13));

            _kvPairs = builder.ToImmutable();
        }

        private static readonly ImmutableDictionary<FunctionsSet, Func<bool, bool, bool>> _kvPairs;
        public static ImmutableDictionary<FunctionsSet, Func<bool, bool, bool>> KVPairs => _kvPairs;        
    }
}
