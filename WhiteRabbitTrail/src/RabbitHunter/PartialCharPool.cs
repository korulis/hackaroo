using System.Collections.Generic;
using System.Linq;

namespace RabbitHunter
{
    public class PartialCharPool
    {
        public bool IsDeadend;
        public List<CharPoolWithWords> OrderredCharPoolsWithWords { get; }
        public string Value { get; }

        public PartialCharPool(List<CharPoolWithWords> orderredCharPoolsWithWords)
        {
            IsDeadend = true;
            OrderredCharPoolsWithWords = orderredCharPoolsWithWords;
            var values = orderredCharPoolsWithWords.Select(x => x.Value);
            Value = string.Concat(values).Alphabetize();
        }

        public PartialCharPool(PartialCharPool partialCharPool, CharPoolWithWords charPoolWithWords)
        {
            IsDeadend = true;
            OrderredCharPoolsWithWords = new List<CharPoolWithWords>();
            OrderredCharPoolsWithWords.AddRange(partialCharPool.OrderredCharPoolsWithWords);
            OrderredCharPoolsWithWords.Add(charPoolWithWords);

            Value = string.Concat(partialCharPool.Value, charPoolWithWords.Value).Alphabetize();

        }
    }
}