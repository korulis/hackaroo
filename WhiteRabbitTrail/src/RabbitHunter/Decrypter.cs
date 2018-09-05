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

        public class Answer
        {
            public bool IsCorrect { get; set; }

            public string Value { get; set; }

            public Answer() : this(partialPhrase: string.Empty, word: string.Empty)
            {
                IsCorrect = false;
            }

            public Answer(string partialPhrase, string word)
            {
                if (string.IsNullOrEmpty(partialPhrase))
                {
                    Value = word;
                }
                else
                {
                    Value = partialPhrase + " " + word;
                }
            }
        }


        public string GetDecryptedPhrase(string hash, string targetAnagram)
        {
            var charPool = targetAnagram.Alphabetize();
            var targetAnagramRelevantWords = RemoveIrrelevantWords(_words, charPool);
            var charPools = CharPoolWithWords.GetCharPools(targetAnagramRelevantWords);

            var candidateAnswer = new Answer(); //answerType

            candidateAnswer = Recursive(hash, candidateAnswer, charPool, charPools);

            if (candidateAnswer.IsCorrect)
            {
                return candidateAnswer.Value;
            }

            throw new NoPhraseFound("no phrase found");
        }

        private Answer Recursive(string hash, Answer candidateAnswer, string charPool, IList<CharPoolWithWords> charPools)
        {
            var partialPhrase = candidateAnswer.Value;
            foreach (var word in charPools)
            {
                if (candidateAnswer.IsCorrect) break;
                candidateAnswer = new Answer(partialPhrase, word.Value);

                var remainderCharPool = charPool.SubtractWord(candidateAnswer.Value.Alphabetize());

                if (remainderCharPool == null)
                {
                    continue;
                }

                if (remainderCharPool == string.Empty)
                {
                    if (_encrypter.Hash(candidateAnswer.Value) == hash)
                    {
                        candidateAnswer.IsCorrect = true;
                    }
                    continue;
                }

                candidateAnswer = Recursive(hash, candidateAnswer, charPool, charPools);
            }
            return candidateAnswer;
        }

        private List<string> RemoveIrrelevantWords(IEnumerable<string> words, string anagram)
        {
            return words.Where(x => anagram.SubtractWord(x) != null).ToList();
        }

    }
}
