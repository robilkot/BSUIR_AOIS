namespace HashTableClassLibrary.Hash
{
    public class StringHasher : IHasher
    {
        const int FirstSymbolsAmount = 2;
        const int AlphabetSymbolsAmount = 'z' - 'a' + 1;
        public int GetHash(object value)
        {
            if (value is not string str)
            {
                throw new NotImplementedException();
            }

            string toHash = str.ToLowerInvariant();

            int hash = 0;

            for (int i = 0; i < Math.Min(FirstSymbolsAmount, str.Length); i++)
            {
                int power = (int)Math.Pow(AlphabetSymbolsAmount, FirstSymbolsAmount - i - 1);
                int currentDigit = (toHash[i] - 'a') * power;
                hash += currentDigit;
            }

            return Math.Abs(hash);
        }
    }
}
