using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RabbitHunterTests;

namespace RabbitHunter
{
    public class Decrypter
    {
        private readonly IEnumerable<string> _words;
        private readonly Encrypter _encrypter;

        public Decrypter(IEnumerable<string> words, Encrypter encrypter)
        {
            _words = words;
            _encrypter = encrypter;
        }

        public string GetDecryptedPhrase(string hash, string targetAnagram)
        {
            var words1 = _words.Clone().ToList();

            var sortedCharPool = Alphabetize(targetAnagram); //type1



            foreach (var word in words1)
            {
                var sortedWord = Alphabetize(word);

                var remainderCharPool = sortedCharPool.Subtract(sortedWord);

                if(remainderCharPool == null) continue;

                if (remainderCharPool == string.Empty)
                {
                    var answer = word;
                    if (_encrypter.Hash(answer) == hash)
                    {
                        return answer;
                    }
                }
            }

            throw new NoPhraseFound("no phrase found");
        }

        public static string Alphabetize(string targetAnagram)
        {
            var alphabetizedAnagram = targetAnagram.ToCharArray().ToList();
            alphabetizedAnagram.Sort();
            return new string(alphabetizedAnagram.Except(new List<char> {' '}).ToArray());
        }
    }


    public static class SortedStringExtensions
    {
        public static string Subtract(this string inital, string input)
        {
            var initialAsList = inital.ToArray().ToList();
            var inputArray = input.ToArray();
            if (input.Length > inital.Length) return null;

            foreach (var inChar in inputArray)
            {
                var success = initialAsList.Remove(inChar);
                if (!success)
                {
                    return null;
                }
            }

            return new string(initialAsList.ToArray());
        }
    }

    public static class Extensions
    {
        public static IEnumerable<T> Clone<T>(this IEnumerable<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone());
        }
    }

}
