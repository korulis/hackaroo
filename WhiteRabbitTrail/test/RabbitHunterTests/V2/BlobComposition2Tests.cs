using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;
using RabbitHunter.V2;


namespace RabbitHunterTests.V2
{
    public class BlobComposition2Tests
    {
        [Fact]
        public void MakeConcatenated_LastElementIsSuffix()
        {
            var suffix = BuildSingletonBlob("a");

            var prefixes = 
                new List<BlobComposition>()
                {
                    new BlobComposition(new List<Blob>{BuildSingletonBlob("f")}),
                    new BlobComposition(new List<Blob>{new Blob("de", new List<string> {"de", "ed"})}),
                };

            var actual = BlobComposition.MakeConcatenated(prefixes, suffix);

            Assert.Equal(actual.Count, prefixes.Count);

            for (var i = 0; i < prefixes.Count; i++)
            {
                Assert.Equal(actual[i].OrderedBlobs.First().CharPool, prefixes[i].OrderedBlobs.First().CharPool); //blobs are equal
                Assert.Equal(actual[i].OrderedBlobs.Last().CharPool, suffix.CharPool); //blobs are equal
            }

        }

        private static Blob BuildSingletonBlob(string charPool)
        {
            return new Blob(charPool, new List<string>() { charPool });
        }
    }
}
