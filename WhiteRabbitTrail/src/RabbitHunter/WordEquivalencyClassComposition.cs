using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RabbitHunter
{
    public class WordEquivalencyClassComposition : AnagramBuilder
    {
        public bool IsDeadend;
        public IReadOnlyCollection<WordEquivalencyClass> OrderedListOdWordEquivalencyClasses { get; }
        public string CharPool { get; }

        public WordEquivalencyClassComposition(IReadOnlyCollection<WordEquivalencyClass> orderedListOdWordEquivalencyClasses)
        {
            IsDeadend = true;
            OrderedListOdWordEquivalencyClasses = orderedListOdWordEquivalencyClasses;
            var values = orderedListOdWordEquivalencyClasses.Select(x => x.CharPool);
            CharPool = string.Concat(values).Alphabetize();
        }

        public WordEquivalencyClassComposition(WordEquivalencyClassComposition wordEquivalencyClassComposition, WordEquivalencyClass wordEquivalencyClass)
        {
            IsDeadend = true;

            var tempList = new List<WordEquivalencyClass>();
            tempList.AddRange(wordEquivalencyClassComposition.OrderedListOdWordEquivalencyClasses);
            tempList.Add(wordEquivalencyClass);
            OrderedListOdWordEquivalencyClasses = new ReadOnlyCollection<WordEquivalencyClass>(tempList);

            CharPool = string.Concat(wordEquivalencyClassComposition.CharPool, wordEquivalencyClass.CharPool).Alphabetize();

        }

        public List<string> BuildAnagrams()
        {
            throw new System.NotImplementedException();
        }
    }
}