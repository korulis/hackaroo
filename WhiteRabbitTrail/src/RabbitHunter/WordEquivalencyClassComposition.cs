using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RabbitHunter
{
    public class WordEquivalencyClassComposition : AnagramBuilder
    {
        public bool IsDeadend;
        public IReadOnlyCollection<WordEquivalencyClass> OrderedListOfWordEquivalencyClasses { get; }
        public string CharPool { get; }

        public WordEquivalencyClassComposition(IReadOnlyCollection<WordEquivalencyClass> orderedListOfWordEquivalencyClasses)
        {
            IsDeadend = true;
            OrderedListOfWordEquivalencyClasses = orderedListOfWordEquivalencyClasses;
            var values = orderedListOfWordEquivalencyClasses.Select(x => x.CharPool);
            CharPool = string.Concat(values).Alphabetize();
        }

        public WordEquivalencyClassComposition(WordEquivalencyClassComposition wordEquivalencyClassComposition, WordEquivalencyClass wordEquivalencyClass)
        {
            IsDeadend = true;

            var tempList = new List<WordEquivalencyClass>();
            tempList.AddRange(wordEquivalencyClassComposition.OrderedListOfWordEquivalencyClasses);
            tempList.Add(wordEquivalencyClass);
            OrderedListOfWordEquivalencyClasses = new ReadOnlyCollection<WordEquivalencyClass>(tempList);

            CharPool = string.Concat(wordEquivalencyClassComposition.CharPool, wordEquivalencyClass.CharPool).Alphabetize();

        }

        public List<string> BuildAnagrams()
        {
            throw new System.NotImplementedException();
        }
    }
}