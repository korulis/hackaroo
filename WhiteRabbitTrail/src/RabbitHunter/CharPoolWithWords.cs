﻿using System.Collections.Generic;
using System.Linq;

namespace RabbitHunter
{
    public class CharPoolWithWords
    {
        public string Value { get; }

        public List<string> Words { get; }

        public CharPoolWithWords(string value, List<string> words)
        {
            Value = value;
            Words = words;
        }

        public static IList<CharPoolWithWords> GetCharPools(List<string> words)
        {
            var result = words
                .GroupBy(x => x.Alphabetize(), x => x)
                .ToDictionary(x => x.Key, x => x.ToList())
                .Select(x => new CharPoolWithWords(x.Key, x.Value))
                .ToList();

            return result;
        }
    }
}