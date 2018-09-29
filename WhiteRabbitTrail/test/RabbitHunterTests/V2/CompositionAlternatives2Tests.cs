using System.Collections.Generic;
using System.Linq;
using Xunit;
using RabbitHunter.V2;


namespace RabbitHunterTests.V2
{
    public class CompositionAlternatives2Tests
    {
        [Fact]
        public void MakeConcatenated_LastElementIsSuffix()
        {
            var suffix = BlobBuilder.BuildSingletonBlob("a");

            var prefixes = 
                new RabbitHunter.V2.CompositionAlternatives2("de",
                new List<BlobComposition>()
                {
                    new BlobComposition(new List<Blob>
                    {
                        BlobBuilder.BuildSingletonBlob("d"), BlobBuilder.BuildSingletonBlob("e")
                    }),
                    new BlobComposition(new List<Blob>{new Blob("de", new List<string> { "de", "ed"})}),
                });

            var actual = new RabbitHunter.V2.CompositionAlternatives2(prefixes, suffix);

            Assert.Equal("ade",actual.CharPool);

            Assert.Equal(actual.BlobCompositions.Count, prefixes.BlobCompositions.Count);

            for (var i = 0; i < prefixes.BlobCompositions.Count; i++)
            {
                Assert.Equal(actual.BlobCompositions[i].OrderedBlobs.First().CharPool, prefixes.BlobCompositions[i].OrderedBlobs.First().CharPool); //blobs are equal
                Assert.Equal(actual.BlobCompositions[i].OrderedBlobs.Last().CharPool, suffix.CharPool); //blobs are equal
            }

        }
    }
}
