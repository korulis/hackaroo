using System;
using System.Collections.Generic;
using RabbitHunter.V2;
using Xunit;

namespace RabbitHunterTests.V2
{
    public class MemoTests
    {
        private readonly Memo2 _sut;

        public MemoTests()
        {
            _sut = new Memo2();
        }

        [Fact]
        public void CanNotAddNull()
        {
            Assert.Throws<ArgumentException>(() => _sut.Add("a", null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ValidKey(string key)
        {
            Assert.Throws<ArgumentException>(() => _sut.Add(key, GetValidBlobComposition(key)));
        }

        private static BlobComposition GetValidBlobComposition(string key)
        {
            return new BlobComposition(
                new List<Blob> {
                    new Blob(key, new List<string>())});
        }
    }
}
