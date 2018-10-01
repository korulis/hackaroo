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

        [Fact]
        public void GraphyBlobs_Exceptions()
        {
            var blobs = new List<Blob> { BlobBuilder.Build("a"), BlobBuilder.Build("a") };

            Assert.Throws<ArgumentException>("blobs", () => Blob.Graphy(blobs));
        }

        [Fact]
        public void GraphyBlobs_Disjoint()
        {
            var blobs = new List<Blob> { BlobBuilder.Build("a"), BlobBuilder.Build("b") };

            Blob.Graphy(blobs);

            Assert.Empty(blobs[0].BigBrothers);
            Assert.Empty(blobs[1].BigBrothers);
        }

        [Fact]
        public void GraphyBlobs()
        {
            var blobs = new List<Blob> { BlobBuilder.Build("aa"), BlobBuilder.Build("a") };

            Blob.Graphy(blobs);

            Assert.Equal("a", blobs[0].CharPool);
            Assert.Empty(blobs[1].BigBrothers);
            Assert.Single(blobs[0].BigBrothers);
            Assert.Equal(blobs[1], blobs[0].BigBrothers.Single());
        }


    }
}
