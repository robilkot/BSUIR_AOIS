using HashTableClassLibrary;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace HashTableUnitTests
{
    [ExcludeFromCodeCoverage]
    public class HashTableUnitTests
    {
        [Fact]
        public void AddOrUpdate_CountShouldChange()
        {
            HashTable<string, string> hashTable = new();

            List<(string, string)> kvPairs = [];

            int expectedCount = 0;

            foreach (var key in new List<string>() { "ab", "aB", "Ab", "AB" })
            {
                foreach (var c in Enumerable.Range(0, 4))
                {
                    kvPairs.Add((key + c.ToString(), c.ToString()));
                    expectedCount++;
                }
            }

            foreach (var kvp in kvPairs)
            {
                hashTable.AddOrUpdate(kvp.Item1, kvp.Item2);
            }

            Assert.Equal(expectedCount, hashTable.Count);
        }
        [Fact]
        public void AddOrUpdate_EmptyKey_ShouldNotAdd()
        {
            HashTable<string, string> table = new();

            table.AddOrUpdate("ab", "a");
            table.AddOrUpdate(null!, "b");
            table.AddOrUpdate("cd", "c");

            Assert.Equal(2, table.Count);
        }
        [Fact]
        public void Remove_EmptyKey_ShouldNotRemove()
        {
            HashTable<string, string> table = new();
            table.AddOrUpdate("ab", "a");
            table.AddOrUpdate("bc", "b");
            table.AddOrUpdate("cd", "c");

            table.Remove(null!);

            Assert.Equal(3, table.Count);
        }
        [Fact]
        public void AddOrUpdate_Empty_ShouldReturnTrue()
        {
            HashTable<string, string> table = new();

            Assert.True(table.Empty);
        }
        [Fact]
        public void Foreach_Empty_ShouldNotThrow()
        {
            HashTable<string, string> table = new();

            var exception = Record.Exception(() =>
            {
                foreach (var item in table)
                {
                    _ = item;
                }
            });

            Assert.Null(exception);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(15)]
        [InlineData(50)]
        public void Foreach_Count_ShouldEqual(int expectedCount)
        {
            HashTable<string, string> table = new();
            int counter = 0;
            foreach (var item in Enumerable.Range(0, expectedCount))
            {
                table.AddOrUpdate($"{item}", "test");
            }

            foreach (var item in table)
            {
                counter++;
            }

            Assert.Equal(expectedCount, counter);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(16)]
        [InlineData(50)]
        [InlineData(100)]
        public void Remove_CountShouldEqual(int countToRemove)
        {
            HashTable<string, string> table = new();

            foreach (var item in Enumerable.Range(0, countToRemove + 1))
            {
                table.AddOrUpdate($"{item}", "test");
            }

            foreach (var item in Enumerable.Range(0, countToRemove))
            {
                table.Remove($"{item}");
            }

            Assert.Equal(1, table.Count);
        }

        [Theory]
        [InlineData("d")]
        [InlineData("e")]
        [InlineData(null)]
        public void Remove_InvalidData_ShouldNotChangeCount(string data)
        {
            HashTable<string, string> table = new();
            table.AddOrUpdate("a", "a");
            table.AddOrUpdate("b", "a");
            table.AddOrUpdate("c", "a");
            var oldCount = table.Count;

            table.Remove(data);

            Assert.Equal(oldCount, table.Count);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData("c")]
        public void TryGetValue_ShouldGetElement(string data)
        {
            HashTable<string, string> table = new();

            table.AddOrUpdate("a", "a");
            table.AddOrUpdate("b", "a");
            table.AddOrUpdate("c", "a");
            table.AddOrUpdate(data, "a");

            Assert.True(table.TryGetValue(data, out _));
        }

        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData(null)]
        public void TryGetValue_InvalidData_ShouldNotGetElement(string data)
        {
            HashTable<string, string> table = new();

            table.AddOrUpdate(null!, "a");
            table.AddOrUpdate("1", "a");
            table.AddOrUpdate("2", "a");

            Assert.False(table.TryGetValue(data, out _));
        }
    }
}