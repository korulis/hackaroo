using System.Collections.Generic;

namespace RabbitHunter
{
    public class PartialCharPool
    {
        public List<CharPoolWithWords> OrderredCharPoolsWithWords { get; }
        public string Value { get; }

        public PartialCharPool(List<CharPoolWithWords> orderredCharPoolsWithWords)
        {
            OrderredCharPoolsWithWords = orderredCharPoolsWithWords;
            Value = string.Concat(orderredCharPoolsWithWords).Alphabetize();
        }

        public PartialCharPool(PartialCharPool partialCharPool, CharPoolWithWords charPoolWithWords)
        {
            OrderredCharPoolsWithWords = new List<CharPoolWithWords>();
            OrderredCharPoolsWithWords.AddRange(partialCharPool.OrderredCharPoolsWithWords);
            OrderredCharPoolsWithWords.Add(charPoolWithWords);

            Value = string.Concat(partialCharPool.Value, charPoolWithWords.Value).Alphabetize();

        }
    }
}