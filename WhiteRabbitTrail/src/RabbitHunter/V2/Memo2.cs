using System;
using RabbitHunter.V1;

namespace RabbitHunter.V2
{
    public class Memo2
    {
        public void Add(string anagramCharPool, WordEquivalencyClass wordEquivalencyClass)
        {
            ValidateInput(anagramCharPool, wordEquivalencyClass);
        }

        private static void ValidateInput(string anagramCharPool, WordEquivalencyClass wordEquivalencyClass)
        {
            if (wordEquivalencyClass == null)
            {
                throw new ArgumentException("Can not be null", nameof(wordEquivalencyClass));
            }

            if (string.IsNullOrEmpty(anagramCharPool))
            {
                throw new ArgumentException("Can not be null or empty", nameof(anagramCharPool));
            }
        }
    }
}