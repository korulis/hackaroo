using System;
using System.Collections.Generic;
using System.Linq;
using RabbitHunter;
using RabbitHunter.V2;

namespace RabbitHunterTests.V2
{
    public class BlobBuilder
    {
        public static Blob Build(params string[] words)
        {
            if (words.Length == 0)
            {
                throw new ArgumentException("should be non empty", nameof(words));
            }

            for (var i = 1; i < words.Length; i++)
            {
                if (words[i - 1].Alphabetize() != words[i].Alphabetize())
                {
                    throw new ArgumentException("Words should be equivalent", nameof(words));
                }
            }

            return new Blob(words[0].Alphabetize(), words.ToList());
        }

    }
}