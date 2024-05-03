using HashTableClassLibrary.Hash;
using HashTableClassLibrary.Tree;
using LabLogger;
using System.Collections;

namespace HashTableClassLibrary
{
    public class HashTable<TKey, TValue> : IEnumerable<BalancedTreeNode<TKey, TValue>> where TKey : IComparable<TKey>
    {
        private readonly IHasher _hasher;

        private BalancedTree<TKey, TValue>[] _rows = [];
        private int _count = 0;
        private int _capacity = 10;

        public HashTable(IHasher? hasher = default)
        {
            hasher ??= new StringHasher();

            _hasher = hasher;

            Rebuild(_capacity);
        }

        #region Public methods
        public bool Empty => _count == 0;
        public int Count => _count;
        public void AddOrUpdate(TKey key, TValue value)
        {
            if (key is null)
            {
                return;
            }
            // Grow
            if (_count >= _capacity)
            {
                Rebuild(_capacity * 2);
            }

            var address = GetAddress(key);

            if (_rows[address]!.AddOrUpdate(key, value))
            {
                _count++;
            }
        }
        public void Remove(TKey key)
        {
            if (key is null)
            {
                return;
            }

            var address = GetAddress(key);

            if (_rows[address].Remove(key))
            {
                _count--;
            }

            // Shrink
            if (_count < _capacity / 2)
            {
                Rebuild(_capacity / 2);
            }
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key is null)
            {
                value = default!;
                return false;
            }
            var address = GetAddress(key);

            return _rows[address]!.TryGetValue(key, out value);
        }
        public IEnumerator<BalancedTreeNode<TKey, TValue>> GetEnumerator()
        {
            foreach (var tree in _rows)
            {
                if (!tree.Empty)
                {
                    foreach (var node in tree)
                    {
                        yield return node;
                    }
                }
            }
        }
        #endregion

        #region Private methods

        private void Rebuild(int newCapacity)
        {
            Logger.Log($"Rebuilding table from {_capacity} to {newCapacity}", Logger.Levels.Debug);

            var oldRows = (BalancedTree<TKey, TValue>[])_rows.Clone();

            _rows = new BalancedTree<TKey, TValue>[newCapacity];

            for (int i = 0; i < _rows.Length; i++)
            {
                _rows[i] = new();
            }

            _capacity = newCapacity;
            _count = 0;

            foreach (var row in oldRows)
            {
                foreach (var treeNode in row)
                {
                    AddOrUpdate(treeNode.Key, treeNode.Value);
                }
            }
        }
        private int GetAddress(TKey key) => _hasher.GetHash(key!) % _capacity;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}