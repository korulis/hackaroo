using System.Collections.Generic;

namespace RabbitHunter
{
    public class Memo
    {
        private readonly IDictionary<string, bool?> _dict;

        public Memo()
        {
            _dict = new Dictionary<string, bool?>();
        }

        public void AddSolution(WordEquivalencyClassComposition wordEquivalencyClassComposition)
        {
            _dict.Add(wordEquivalencyClassComposition.CharPool, true);
        }

        public void AddDeadEnd(WordEquivalencyClassComposition wordEquivalencyClassComposition)
        {
            _dict.Add(wordEquivalencyClassComposition.CharPool, null);
        }

        public bool Contains(string value)
        {
            return _dict.ContainsKey(value);
        }

        public bool? this[string value] => _dict[value];
    }
}