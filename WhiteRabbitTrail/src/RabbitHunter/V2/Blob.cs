using System.Collections.Generic;
using System.Linq;
using RabbitHunter.V1;

namespace RabbitHunter.V2
{
    public class Blob
    {
        public string CharPool { get; }

        public IReadOnlyCollection<string> Words { get; }

        public Blob(string charPool, IReadOnlyCollection<string> words)
        {
            CharPool = charPool;
            Words = words;
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