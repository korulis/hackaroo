using System;
using System.Linq;
using RabbitHunter.V2;

namespace RabbitHunterTests.V2
{
    public static class AlternativesBuilder
    {
        public static CompositionAlternatives2 Build(params BlobComposition[] compositions)
        {
            if (compositions.Length == 0)
            {
                throw new ArgumentException("Should be non empty", nameof(compositions));
            }

            return new CompositionAlternatives2(compositions[0].OrderedBlobs.ToArray()[0].CharPool, compositions.ToList());
        }
    }
}