using System.Collections;

namespace HashTableClassLibrary.Tree
{
    public class BalancedTree<TKey, TValue> : IEnumerable<BalancedTreeNode<TKey, TValue>> where TKey : IComparable<TKey>
    {
        private BalancedTreeNode<TKey, TValue>? _root = null;
        public BalancedTree() { }
        public bool Empty => _root is null;
        public bool AddOrUpdate(TKey key, TValue value) => AddOrUpdate(ref _root, key, value);
        private bool AddOrUpdate(ref BalancedTreeNode<TKey, TValue>? current, TKey key, TValue value)
        {
            if (current == null)
            {
                current = new(key, value);
                return true;
            }
            else if (key.CompareTo(current.Key) < 0)
            {
                var result = AddOrUpdate(ref current.Left, key, value);

                Balance(ref current);

                return result;
            }
            else if (key.CompareTo(current.Key) > 0)
            {
                var result = AddOrUpdate(ref current.Right, key, value);

                Balance(ref current);

                return result;
            }
            else
            {
                // Update existing key-value pair
                current.Value = value;
                return false;
            }
        }
        private void Balance(ref BalancedTreeNode<TKey, TValue>? current)
        {
            if (current is null)
            {
                return;
            }

            int b_factor = GetBalanceFactor(current);

            if (b_factor > 1)
            {
                if (GetBalanceFactor(current.Left) > 0)
                {
                    RotateLL(ref current);
                }
                else
                {
                    RotateLR(ref current);
                }
            }
            else if (b_factor < -1)
            {
                if (GetBalanceFactor(current.Right) > 0)
                {
                    RotateRL(ref current);
                }
                else
                {
                    RotateRR(ref current);
                }
            }
        }
        public bool Remove(TKey target) => Remove(ref _root, target);
        private bool Remove(ref BalancedTreeNode<TKey, TValue>? current, TKey target)
        {
            if (current == null)
            {
                return false;
            }

            bool result;

            // Left subtree
            if (target.CompareTo(current.Key) < 0)
            {
                result = Remove(ref current.Left, target);
            }
            // Right subtree
            else if (target.CompareTo(current.Key) > 0)
            {
                result = Remove(ref current.Right, target);
            }
            // Found key in current
            else
            {
                result = true;

                // Case 1: No child or one child
                if (current.Left == null)
                {
                    current = current.Right;
                }
                else if (current.Right == null)
                {
                    current = current.Left;
                }
                // Case 2: Two children
                else
                {
                    // Find inorder successor (smallest node in the right subtree)
                    var successor = current.Right;
                    while (successor.Left != null)
                    {
                        successor = successor.Left;
                    }

                    // Replace current node with successor
                    current.Key = successor.Key;
                    current.Value = successor.Value;

                    // Remove the successor node
                    Remove(ref current.Right, successor.Key);
                }
            }

            // Update balance factors and perform rotations
            Balance(ref current);

            return result;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_root is not null)
            {
                if (BalancedTree<TKey, TValue>.TryGetValue(key, _root, out var found))
                {
                    value = found.Value;
                    return true;
                }
            }

            value = default!;
            return false;
        }

        private static bool TryGetValue(TKey target, BalancedTreeNode<TKey, TValue> current, out BalancedTreeNode<TKey, TValue> result)
        {
            if (target.CompareTo(current.Key) < 0)
            {
                if (current.Left is not null)
                {
                    return BalancedTree<TKey, TValue>.TryGetValue(target, current.Left, out result);
                }
            }
            else if (target.CompareTo(current.Key) > 0)
            {
                if (current.Right is not null)
                {
                    return BalancedTree<TKey, TValue>.TryGetValue(target, current.Right, out result);
                }
            }
            else
            {
                result = current;
                return true;
            }

            result = default!;
            return false;
        }
        private static int GetHeight(BalancedTreeNode<TKey, TValue>? current)
        {
            if (current is null)
            {
                return 0;
            }

            int height = 0;
            if (current != null)
            {
                int l = GetHeight(current.Left);
                int r = GetHeight(current.Right);
                int m = Math.Max(l, r);
                height = m + 1;
            }
            return height;
        }
        private static int GetBalanceFactor(BalancedTreeNode<TKey, TValue>? current)
        {
            if(current is null)
            {
                return 0;
            }

            int l = GetHeight(current.Left);
            int r = GetHeight(current.Right);

            return l - r;
        }
        private static void RotateRR(ref BalancedTreeNode<TKey, TValue> parent)
        {
            var pivot = parent.Right;
            parent.Right = pivot.Left;
            pivot.Left = parent;
            parent = pivot;
        }
        private static void RotateLL(ref BalancedTreeNode<TKey, TValue> parent)
        {
            var pivot = parent.Left;
            parent.Left = pivot.Right;
            pivot.Right = parent;
            parent = pivot;
        }
        private static void RotateLR(ref BalancedTreeNode<TKey, TValue> parent)
        {
            RotateRR(ref parent.Left);
            RotateLL(ref parent);
        }
        private static void RotateRL(ref BalancedTreeNode<TKey, TValue> parent)
        {
            RotateLL(ref parent.Right);
            RotateRR(ref parent);
        }

        public IEnumerator<BalancedTreeNode<TKey, TValue>> GetEnumerator()
        {
            if (_root is null)
                yield break;
            else
            {
                foreach (var node in _root)
                {
                    yield return node;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
