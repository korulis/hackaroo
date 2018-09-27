using System;
using System.Collections.Generic;
using RabbitHunter.V1;
using RabbitHunter.V2;
using Xunit;

namespace RabbitHunterTests
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
            Assert.Throws<ArgumentException>(() => _sut.Add("", new WordEquivalencyClass(key, new List<string>())));
        }
    }
}
