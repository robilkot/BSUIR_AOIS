using HashTableClassLibrary;
using HashTableClassLibrary.Hash;
using System.Diagnostics.CodeAnalysis;

namespace HashTableUnitTests
{
    [ExcludeFromCodeCoverage]
    public class StringHasherUnitTests
    {
        [Theory]
        [InlineData("aa", "aA")]
        [InlineData("Ab", "aB")]
        public void GetHash_Collision_HashesShouldEqual(string data1, string data2)
        {
            StringHasher hasher = new();

            var hash1 = hasher.GetHash(data1);
            var hash2 = hasher.GetHash(data2);

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData(2.3)]
        public void GetHash_InvalidObjectType_ShouldThrow(object data)
        {
            StringHasher hasher = new();

            Assert.Throws<NotImplementedException>(() =>
            {
                var hash1 = hasher.GetHash(data);
            });
        }
    }
}