using System;
using System.Collections.Generic;

namespace RabbitHunter
{
    public class Memo
    {
        private readonly string _anagram;
        private readonly IDictionary<string, CompositionAlternatives> _dict;

        public Memo(string anagram)
        {
            _anagram = anagram;
            _dict = new Dictionary<string, CompositionAlternatives>();
        }

        public void AddSolvable(WordEquivalencyClassComposition wordEquivalencyClassComposition)
        {
            var charPool = wordEquivalencyClassComposition.CharPool;
            if (ContainsKey(charPool))
            {
                throw new ArgumentException("I should not be here.");
                return;
            }

            var compositionAlternatives = CompositionAlternatives.NotDeadend(wordEquivalencyClassComposition);  
            _dict.Add(charPool, compositionAlternatives);
        }

        public void AddSolution(WordEquivalencyClassComposition wordEquivalencyClassComposition)
        {
            var charPool = wordEquivalencyClassComposition.CharPool;
            if (!ContainsKey(charPool))
            {
                var compositionAlternatives = new CompositionAlternatives(wordEquivalencyClassComposition);
                _dict.Add(charPool, compositionAlternatives);
            }
            else
            {
                //todo must check if this is duplicate structure that i am adding (optimisation)
                _dict[charPool].AddAlternative(wordEquivalencyClassComposition);
            }

        }

        public void AddDeadEnd(WordEquivalencyClassComposition wordEquivalencyClassComposition)
        {
            _dict.Add(wordEquivalencyClassComposition.CharPool, null);
        }

        public bool ContainsKey(string value)
        {
            return _dict.ContainsKey(value);
        }

        public CompositionAlternatives this[string value] => _dict[value];

        public void GenerateAndAddFullSolution_FromIncomplete(WordEquivalencyClassComposition inComplete)
        {
            try
            {
                Solutions.AddFromIncomplete(inComplete);
            }
            catch (Exception e)
            {
                throw new ArgumentException("I should not be here 3");
            }
        }

        public CompositionAlternatives Solutions => _dict[_anagram];

        public int SolutionCount => _dict.ContainsKey(_anagram) ? _dict[_anagram].TempList.Count : 0;

        public int Count => _dict.Count;
    }
}