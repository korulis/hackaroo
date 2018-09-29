using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using RabbitHunter.V2;


namespace RabbitHunterTests.V2
{
    public class BlobTests
    {
        [Fact]
        public void Blob_InvalidStates_1()
        {
            Assert.Throws<ArgumentException>("charPool", () => new Blob(null, new List<string> { "a" }));
        }
        [Fact]
        public void Blob_InvalidStates_2()
        {
            Assert.Throws<ArgumentException>("charPool", () => new Blob("", new List<string> { "a" }));
        }
        [Fact]
        public void Blob_InvalidStates_3()
        {
            Assert.Throws<ArgumentException>("words", () => new Blob("ab", null));
        }

    }
}
