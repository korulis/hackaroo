using System.Collections.Generic;
using System.Linq;

namespace RabbitHunter
{
    public class WordEquivalencyClass
    {
        public string CharPool { get; }

        public IReadOnlyCollection<string> Words { get; }

        public WordEquivalencyClass(string charPool, IReadOnlyCollection<string> words)
        {
            CharPool = charPool;
            Words = words;
        }

        public static IDictionary<string,List<string>> GetDictionary(List<string> words)
        {
            var result = words
                .GroupBy(x => x.Alphabetize(), x => x)
                .ToDictionary(x => x.Key, x => x.ToList())
                //.Select(x => new WordEquivalencyClass(x.Key, x.CharPool))
                //.ToList()
                ;

            return result;
        }

        public static IList<WordEquivalencyClass> FromWordList(List<string> targetAnagramRelevantWords)
        {
            return GetDictionary(targetAnagramRelevantWords).Select(x => new WordEquivalencyClass(x.Key, x.Value)).ToList();
        }
    }
}