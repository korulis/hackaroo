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

        public void AddSolution(PartialCharPool partialCharPool)
        {
            _dict.Add(partialCharPool.Value, true);
        }

        public void AddDeadEnd(PartialCharPool partialCharPool)
        {
            _dict.Add(partialCharPool.Value, null);
        }

        public bool Contains(string value)
        {
            return _dict.ContainsKey(value);
        }

        public bool? this[string value] => _dict[value];
    }
}