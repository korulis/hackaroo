using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RabbitHunter
{
    public class CompositionAlternatives : AnagramBuilder
    {
        public List<WordEquivalencyClassComposition> TempList => _listOfCompositionAlternatives;


        public bool IsDeadend;
        private readonly List<WordEquivalencyClassComposition> _listOfCompositionAlternatives;
        public string CharPool { get; private set; }

        public CompositionAlternatives(WordEquivalencyClassComposition WordEquivalencyClassComposition)
        {
            IsDeadend = WordEquivalencyClassComposition.IsDeadend;
            CharPool = WordEquivalencyClassComposition.CharPool;
            _listOfCompositionAlternatives = new List<WordEquivalencyClassComposition> { WordEquivalencyClassComposition };
        }

        public static CompositionAlternatives NotDeadend(WordEquivalencyClassComposition el)
        {
            return new CompositionAlternatives(el)
            {
                IsDeadend = false
            };
        }


        public void AddAlternative(WordEquivalencyClassComposition alternative)
        {
            _listOfCompositionAlternatives.Add(alternative);

            // with validation?
            if (true)
            {
                if (_listOfCompositionAlternatives.Any(x => x.IsDeadend != alternative.IsDeadend))
                {
                    throw new ArgumentException("Composition Alternatives have do not agree on their DeadEnd status.");
                }

                if (_listOfCompositionAlternatives.Any(x => x.CharPool != alternative.CharPool))
                {
                    throw new ArgumentException("Composition Alternatives have do not agree on their CharPool.");
                }
            }
        }

        public List<string> BuildAnagrams()
        {
            throw new NotImplementedException();
        }

        public void AddFromIncomplete(WordEquivalencyClassComposition inComplete)
        {
            // todo 
            // 1 it chould be that i am adding a complete. make sure I add one isntead of zero
            // 2 i do not add the tails... im just adding more of the same alternatives.
            // 3 ...?

            var listOfAlternativesThatStartsTheSame = GetSequencesThatStartTheSame(inComplete);
            var overlainCompleteAlternatives = OverlayWithIncomplete(listOfAlternativesThatStartsTheSame, inComplete);

            _listOfCompositionAlternatives.AddRange(overlainCompleteAlternatives);
        }

        private IEnumerable<WordEquivalencyClassComposition> OverlayWithIncomplete(
            IEnumerable<WordEquivalencyClassComposition> listOfAlternativesThatStartsTheSame,
            WordEquivalencyClassComposition inComplete)
        {
            var sequenceOfClassesOfIncomplete = inComplete.OrderedListOfWordEquivalencyClasses.ToList();

            var result = listOfAlternativesThatStartsTheSame.Select(alternative =>
            {

                List<WordEquivalencyClass> newList = sequenceOfClassesOfIncomplete.ToList();
                //newList = alternative.OrderedListOfWordEquivalencyClasses
                //    .Select((@class, i) => i < sequenceOfClasses.Count ? sequenceOfClasses[i] : @class).ToList();

                var alternativeCompositionClasses = alternative.OrderedListOfWordEquivalencyClasses.ToList();

                //todo optimize this loop
                for (int i = 0; i < alternativeCompositionClasses.Count; i++)
                {
                    var incrementalCharPool = alternativeCompositionClasses.Take(i + 1).Select(x=>x.CharPool).Aggregate(string.Concat);
                    if (inComplete.CharPool.SubtractChars(incrementalCharPool) == null )
                    {
                        newList.Add(alternativeCompositionClasses[i]);
                    }
                }


                return new WordEquivalencyClassComposition(newList) { IsDeadend = false };
            }).ToList();

            return result;
        }

        private List<WordEquivalencyClassComposition> GetSequencesThatStartTheSame(WordEquivalencyClassComposition inComplete)
        {
            var sequenceOfClassesCharPoll = inComplete.OrderedListOfWordEquivalencyClasses.Select(x => x.CharPool).ToList();

            var result = new List<WordEquivalencyClassComposition>();

            foreach (var composition in _listOfCompositionAlternatives) { 

                var sequenceOfClasses2 = composition.OrderedListOfWordEquivalencyClasses.ToList();

                var buildUpCharPool = "";
                for (int i = 0; i < sequenceOfClasses2.Count; i++)
                {
                    var classs = sequenceOfClasses2[i];
                    buildUpCharPool = string.Concat(buildUpCharPool, classs.CharPool);
                    var diff = inComplete.CharPool.SubtractChars(buildUpCharPool);
                    if (diff == null)
                    {
                        break;
                    }
                    if (diff.Length == 0)
                    {
                        result.Add(composition);
                    }
                    if (diff.Length > 0)
                    {
                        continue;
                    }
                }

                //var charpool = sequenceOfClassesCharPools2.Take(sequenceOfClassesCharPoll.Count).Aggregate(string.Concat).Alphabetize();
                //if (charpool == inComplete.CharPool)
                //{
                //    result.Add(compositionAlternative);
                //}
            }

            return result;
        }
    }
}