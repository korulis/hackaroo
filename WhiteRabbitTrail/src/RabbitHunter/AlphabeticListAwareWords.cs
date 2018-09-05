using System.Collections.Generic;
using System.Linq;

namespace RabbitHunter
{
    public class AlphabeticListAwareWords
    {
        public string Value { get; }
        public int WordsAhead { get; }

        public AlphabeticListAwareWords(string value, int wordsAhead)
        {
            Value = value;
            WordsAhead = wordsAhead;
        }

        public static List<AlphabeticListAwareWords> GetAlphabeticListAwareWords(List<string> words)
        {
            var result = new List<AlphabeticListAwareWords>();

            for (var index = 0; index < words.Count; index++)
            {
                var word = words[index];
                var wordsAhead = 0;
                for (var innerI = index + 1; innerI < words.Count; innerI++)
                {
                    var wordAhead = words[innerI];
                    if (wordAhead.SubtractChars(word) != null)
                    {
                        wordsAhead++;
                    }
                    else
                    {
                        break;
                    }
                }

                result.Add(new AlphabeticListAwareWords(word, wordsAhead));
            }


            return result.ToList();
        }
    }
}