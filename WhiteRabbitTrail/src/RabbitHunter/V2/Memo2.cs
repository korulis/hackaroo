using System;
using System.Collections.Generic;
using RabbitHunter.V1;

namespace RabbitHunter.V2
{
    public class Memo2
    {
        public void Add(string anagramCharPool, BlobComposition blob)
        {
            ValidateInput(anagramCharPool, blob);
        }

        public void AddMultiple(string anagramCharPool, List<BlobComposition> wordEquivalencyClasses)
        {
            wordEquivalencyClasses.ForEach(x=> Add(anagramCharPool,x));
        }



        private static void ValidateInput(string anagramCharPool, BlobComposition blob)
        {
            if (blob == null)
            {
                throw new ArgumentException("Can not be null", nameof(blob));
            }

            if (string.IsNullOrEmpty(anagramCharPool))
            {
                throw new ArgumentException("Can not be null or empty", nameof(anagramCharPool));
            }
        }

        public List<BlobComposition> Get(string anagramCharPool)
        {
            throw new NotImplementedException();
        }
    }
}