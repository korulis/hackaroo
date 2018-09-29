using System.Collections.Generic;
using System.Globalization;
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
            var unfoldedBlobs = new List<string> { "" };

            foreach (var blob in OrderedBlobs)
            {
                var temp = new List<string>();
                foreach (var unfoldedBlob in unfoldedBlobs)
                {
                    foreach (var word in blob.Words)
                    {
                        var updatedUnfoldedBlob = Add(unfoldedBlob, word);
                        temp.Add(updatedUnfoldedBlob);
                    }
                }
                unfoldedBlobs = temp;

            }

            return unfoldedBlobs;
        }

        private static string Add(string unfoldedBlob, string word)
        {
            if (unfoldedBlob == string.Empty)
            {
                return word;
            }
            return unfoldedBlob + " " + word;
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