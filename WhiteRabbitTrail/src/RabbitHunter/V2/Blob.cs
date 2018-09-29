using System;
using System.Collections.Generic;
using System.Linq;
using RabbitHunter.V1;

namespace RabbitHunter.V2
{
    public class Blob
    {
        public string CharPool { get; }

        private readonly List<string> _words;

        public IReadOnlyCollection<string> Words => _words;

        public Blob(string charPool, List<string> words)
        {
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
    }
}