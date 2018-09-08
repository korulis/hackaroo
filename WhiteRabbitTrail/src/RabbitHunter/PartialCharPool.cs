using System.Collections.Generic;

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
            Value = string.Concat(orderredCharPoolsWithWords).Alphabetize();
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