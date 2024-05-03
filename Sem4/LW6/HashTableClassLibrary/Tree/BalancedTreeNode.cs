using System.Collections;

namespace HashTableClassLibrary.Tree
{
    public class BalancedTreeNode<TKey, TValue>(TKey key, TValue value) : IEnumerable<BalancedTreeNode<TKey, TValue>> where TKey : IComparable<TKey>
    {
        public BalancedTreeNode<TKey, TValue>? Left = null;
        public BalancedTreeNode<TKey, TValue>? Right = null;

        public TValue Value = value;
        public TKey Key = key;

        public IEnumerator<BalancedTreeNode<TKey, TValue>> GetEnumerator()
        {
            yield return this;

            if (Left is not null)
            {
                foreach (var item in Left)
                    yield return item;
            }

            if (Right is not null)
            {
                foreach (var item in Right)
                    yield return item;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
