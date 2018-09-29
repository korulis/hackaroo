using System;
using System.Linq;
using RabbitHunter.V2;

namespace RabbitHunterTests.V2
{
    public static class CompositionBuilder
    {
        public static BlobComposition Build(params Blob[] blobs)
        {
            if (blobs.Length == 0)
            {
                throw new ArgumentException("Should be non empty", nameof(blobs));
            }

            return new BlobComposition(blobs.ToList());
        }
    }
}