using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RabbitHunter;
using Xunit;
using RabbitHunter.V2;


namespace RabbitHunterTests.V2
{
    public class CompositionAlternatives2Tests
    {

        [Theory]
        [MemberData(nameof(PrefixConstructorData))]
        public void CompositionAlternatives2_PrefixConstructor(CompositionAlternatives2 prefixes, Blob suffix, string[] expectedBlobae)
        {
            var actual = CompositionAlternatives2.GetCombined(prefixes, suffix);

            var iEnumerable = actual.BlobCompositions.Select(x => string.Join(",", x.OrderedBlobs.Select(y => y.CharPool))).ToList();
            Assert.Equal(expectedBlobae, iEnumerable);
        }

        public static IEnumerable<object[]> PrefixConstructorData => new[]
        {
            new object[]
            {
                AlternativesBuilder.Build(CompositionBuilder.Build(BlobBuilder.Build("ac"))),
                BlobBuilder.BuildSingletonBlob("b"),
                new[] {"ac,b"}
            },
            new object[]
            {
                AlternativesBuilder.Build(CompositionBuilder.Build(BlobBuilder.Build("a"),BlobBuilder.Build("c"))),
                BlobBuilder.BuildSingletonBlob("z"),
                new[] {"a,c,z"}
            },

            new object[]
            {
                AlternativesBuilder.Build(
                    CompositionBuilder.Build(BlobBuilder.Build("rt")),
                    CompositionBuilder.Build(BlobBuilder.Build("ac")),
                    CompositionBuilder.Build(BlobBuilder.Build("a"),BlobBuilder.Build("c"))
                    ),
                BlobBuilder.BuildSingletonBlob("z"),
                new[] {"rt,z","ac,z","a,c,z"}
            },
        };

        [Fact]
        public void CompositionAlternatives2_BlobCompositionsConstructor_CharpoolNotNull()
        {
            var blobCompositions = new List<BlobComposition> { CompositionBuilder.Build(BlobBuilder.Build("a")) };

            Assert.Throws<ArgumentException>("charPool", () => new CompositionAlternatives2(null, blobCompositions));
        }

        [Fact]
        public void CompositionAlternatives2_BlobCompositionsConstructor_CharpoolNotEmpty()
        {
            var blobCompositions = new List<BlobComposition> { CompositionBuilder.Build(BlobBuilder.Build("a")) };

            Assert.Throws<ArgumentException>("charPool", () => new CompositionAlternatives2("", blobCompositions));
        }

        [Fact]
        public void CompositionAlternatives2_BlobCompositionsConstructor_BlobCompositionsNotNull()
        {
            Assert.Throws<ArgumentException>("blobCompositions", () => new CompositionAlternatives2("a", null));
        }


        [Fact]
        public void CompositionAlternatives2_PrefixConstructor_PrefixArgumentValidation()
        {
            Assert.Throws<ArgumentException>("prefixes", () => CompositionAlternatives2.GetCombined(null, BlobBuilder.BuildSingletonBlob("a")));
        }

        [Fact]
        public void CompositionAlternatives2_PrefixConstructor_SuffixArgumentValidation()
        {
            var prefixes = new CompositionAlternatives2("a",
                new List<BlobComposition> { new BlobComposition(new List<Blob> { BlobBuilder.BuildSingletonBlob("a") }) });

            Blob badSuffix = null;

            Assert.Throws<ArgumentException>("suffix", () => CompositionAlternatives2.GetCombined(prefixes, badSuffix));
        }


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

            var actual = CompositionAlternatives2.GetCombined(prefixes, suffix);

            Assert.Equal("ade", actual.CharPool);

            Assert.Equal(actual.BlobCompositions.Count, prefixes.BlobCompositions.Count);

            for (var i = 0; i < prefixes.BlobCompositions.Count; i++)
            {
                Assert.Equal(actual.BlobCompositions[i].OrderedBlobs.First().CharPool, prefixes.BlobCompositions[i].OrderedBlobs.First().CharPool); //blobs are equal
                Assert.Equal(actual.BlobCompositions[i].OrderedBlobs.Last().CharPool, suffix.CharPool); //blobs are equal
            }

        }
    }
}
