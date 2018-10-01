using System;
using System.Collections.Generic;
using System.Linq;

namespace RabbitHunter.V2
{
    public class Blob
    {
        public List<Blob> BigBrothers { get; }

        public string CharPool { get; }

        private readonly List<string> _words;

        public IReadOnlyCollection<string> Words => _words;

        public Blob(string charPool, List<string> words)
        {
            BigBrothers = new List<Blob>();
            // not checking words count and whether they are equivalent due to optimisation reasons.
            if (string.IsNullOrEmpty(charPool))
            {
                throw new ArgumentException("Char pool can not be null or empty", nameof(charPool));
            }
            if (words == null)
            {
                throw new ArgumentException("Words can not be null", nameof(words));
            }

            CharPool = charPool;
            _words = words;
        }

        private static IDictionary<string, List<string>> GetDictionary(List<string> words)
        {
            var result = words
                .GroupBy(x => x.Alphabetize(), x => x)
                .ToDictionary(x => x.Key, x => x.ToList())
                //.Select(x => new WordEquivalencyClass(x.Key, x.CharPool))
                //.ToList()
                ;

            return result;
        }

        //todo test
        public static IList<Blob> FromWordList(List<string> targetAnagramRelevantWords)
        {
            return GetDictionary(targetAnagramRelevantWords).Select(x => new Blob(x.Key, x.Value)).ToList();
        }

        public static void Graphy(List<Blob> blobs)
        {
            ValidateForGraphy(blobs);

            foreach (var littleBrother in blobs)
            {
                foreach (var brother in blobs)
                {
                    var diff = brother.CharPool.SubtractChars(littleBrother.CharPool);
                    if (!string.IsNullOrEmpty(diff))
                    {
                        littleBrother.BigBrothers.Add(brother);
                    }
                }
            }
        }

        private static void ValidateForGraphy(List<Blob> blobs)
        {
            blobs.Sort((blob1, blob2) => CharPoolComparer.CompareCharPools(blob1.CharPool, blob2.CharPool));

            for (int i = 1; i < blobs.Count; i++)
            {
                if (blobs[i - 1].CharPool.SubtractChars(blobs[i].CharPool) == string.Empty)
                {
                    throw new ArgumentException("All blobs must have different char pools.", nameof(blobs));
                }
            }
        }
    }
}