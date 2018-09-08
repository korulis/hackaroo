using System.Collections.Generic;

namespace RabbitHunter
{
    public class Memo
    {
        private readonly IDictionary<string, CompositionAlternatives> _dict;

        public Memo()
        {
            _dict = new Dictionary<string, CompositionAlternatives>();
        }

        public void AddSolution(WordEquivalencyClassComposition wordEquivalencyClassComposition)
        {
            var charPool = wordEquivalencyClassComposition.CharPool;
            if (!ContainsKey(charPool))
            {
                _dict.Add(charPool, new CompositionAlternatives(wordEquivalencyClassComposition));
            }
            else
            {
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
    }
}