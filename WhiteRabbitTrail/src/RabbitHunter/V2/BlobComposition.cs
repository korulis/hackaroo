using System.Collections.Generic;
using System.Linq;

namespace RabbitHunter.V2
{
    public class BlobComposition : AnagramBuilder
    {
        public bool IsDeadend;
        public IReadOnlyCollection<Blob> OrderedBlobs { get; }

        public BlobComposition(List<Blob> list)
        {
            IsDeadend = false;
            OrderedBlobs = list;
        }

        public BlobComposition(BlobComposition composition, Blob suffix)
        {
            IsDeadend = false;

            var blobs = composition.OrderedBlobs.ToList();
            blobs.Add(suffix);
            OrderedBlobs = blobs;
        }

        public IEnumerable<string> BuildAnagrams()
        {
            var phrases = new List<string> {"a"};

            return phrases;
        }

        public static BlobComposition DeadEnd = MakeDeadEnd();

        private static BlobComposition MakeDeadEnd()
        {
            var blobComposition = new BlobComposition(new List<Blob>());
            blobComposition.IsDeadend = true;
            return blobComposition;
        }
    }
}