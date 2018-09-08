using System;
using System.Collections.Generic;
using System.Linq;

namespace RabbitHunter
{
    public class CompositionAlternatives : AnagramBuilder
    {
        public bool IsDeadend;
        private readonly IList<WordEquivalencyClassComposition> _listOfCompositionAlternatives;
        public string CharPool { get; }

        public CompositionAlternatives(WordEquivalencyClassComposition WordEquivalencyClassComposition)
        {
            IsDeadend = WordEquivalencyClassComposition.IsDeadend;
            CharPool = WordEquivalencyClassComposition.CharPool;
            _listOfCompositionAlternatives = new List<WordEquivalencyClassComposition> { WordEquivalencyClassComposition };
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
    }
}