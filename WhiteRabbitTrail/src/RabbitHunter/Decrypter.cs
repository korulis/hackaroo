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
            var sortedAnagram = Alphabetize(targetAnagram);

            foreach (var word in _words)
            {
                var sortedWord = Alphabetize(word);

                if (sortedWord == sortedAnagram)
                {
                    var answer = word;
                    if (_encrypter.Hash(answer) == hash)
                    {
                        return answer;
                    }
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
