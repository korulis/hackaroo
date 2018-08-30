using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RabbitHunter
{
    public class Decrypter
    {
        private readonly IEnumerable<string> _words;

        public Decrypter(IEnumerable<string> words)
        {
            _words = words;
        }

        public string GetDecryptedPhrase(string hash, string targetAnagram)
        {
            var sortedAnagram = Alphabetize(targetAnagram);

            foreach (var word in _words)
            {
                var sortedWord = Alphabetize(word);

                if (sortedWord == sortedAnagram)
                {
                    return word;
                }
            }

            throw new Exception("no phrase found");
        }

        private static string Alphabetize(string targetAnagram)
        {
            var alphabetizedAnagram = targetAnagram.ToCharArray().ToList();
            alphabetizedAnagram.Sort();
            return new string(alphabetizedAnagram.ToArray());
        }
    }
}
