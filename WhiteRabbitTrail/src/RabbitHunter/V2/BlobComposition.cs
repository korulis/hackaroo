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

        private BlobComposition(BlobComposition composition, Blob suffix)
        {
            IsDeadend = false;

            var blobs = composition.OrderedBlobs.ToList();
            blobs.Add(suffix);
            OrderedBlobs = blobs;
        }

        public List<string> BuildAnagrams()
        {
            throw new System.NotImplementedException();
        }

        public static List<BlobComposition> MakeConcatenated(List<BlobComposition> prefixes, Blob suffix)
        {
            var result = prefixes.Select(x => new BlobComposition(x, suffix)).ToList();

            return result;
        }
    }
}