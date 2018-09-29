using System.Collections.Generic;
using System.Linq;
using Xunit;
using RabbitHunter.V2;


namespace RabbitHunterTests.V2
{
    public class BlobCompositionTests
    {

        [Fact]
        public void BuildAnagramTest()
        {
            var blobs = new List<Blob> { BlobBuilder.BuildSingletonBlob("a") };
            var sut = new BlobComposition(blobs);

            var actual = sut.BuildAnagrams();

            Assert.Equal(new List<string> { "a" }, actual);
        }
    }
}
