using System;
using System.Collections.Generic;
using System.Linq;

namespace RabbitHunter.V1
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
                    throw new ArgumentException("Composition BlobCompositions have do not agree on their DeadEnd status.");
                }

                if (_listOfCompositionAlternatives.Any(x => x.CharPool != alternative.CharPool))
                {
                    throw new ArgumentException("Composition BlobCompositions have do not agree on their CharPool.");
                }
            }
        }

        public IEnumerable<string> BuildAnagrams()
        {
            throw new NotImplementedException();
        }

        public void AddFromIncomplete(WordEquivalencyClassComposition inComplete)
        {
            var overlainCompleteAlternatives = OverlayWithIncomplete(inComplete);

            _listOfCompositionAlternatives.AddRange(overlainCompleteAlternatives);
        }

        private IEnumerable<WordEquivalencyClassComposition> OverlayWithIncomplete(
            WordEquivalencyClassComposition inComplete)
        {
            var sequenceOfClassesOfIncomplete = inComplete.OrderedListOfWordEquivalencyClasses.ToList();

            var result = _listOfCompositionAlternatives.Select(alternative =>
            {

                List<WordEquivalencyClass> newList = sequenceOfClassesOfIncomplete.ToList();

                var compositionClassSequence = alternative.OrderedListOfWordEquivalencyClasses.ToList();

                //todo optimize this loop
                var allowAdding = false;
                for (int i = 0; i < compositionClassSequence.Count; i++)
                {
                    var incrementalCharPool = compositionClassSequence.Take(i + 1).Select(x => x.CharPool).Aggregate(string.Concat);
                    if (inComplete.CharPool.SubtractChars(incrementalCharPool) == null)
                    {
                        if (allowAdding)
                        {
                            newList.Add(compositionClassSequence[i]);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    if (inComplete.CharPool.SubtractChars(incrementalCharPool) == string.Empty)
                    {
                        allowAdding = true;
                    }
                }


                return new WordEquivalencyClassComposition(newList) { IsDeadend = false };
            }).Where(x => x != null).ToList();

            return result;
        }
    }


}