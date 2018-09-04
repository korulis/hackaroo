using System.Collections.Generic;
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

            public Answer() : this(string.Empty)
            {
                IsCorrect = false;
            }

            public Answer(string value)
            {
                Value = value;
            }
        }


        public string GetDecryptedPhrase(string hash, string targetAnagram)
        {
            var charPool = targetAnagram.Alphabetize();
            var candidateAnswer = new Answer(); //answerType

            foreach (var word in _words)
            {
                if (candidateAnswer.IsCorrect) break;
                candidateAnswer = new Answer(word);

                var remainderCharPool = charPool.SubtractWord(candidateAnswer.Value.Alphabetize());

                if (remainderCharPool == null) continue;

                if (remainderCharPool == string.Empty)
                {
                    if (_encrypter.Hash(candidateAnswer.Value) == hash)
                    {
                        candidateAnswer.IsCorrect = true;

                    }
                    continue;
                }

                foreach (var word2 in _words)
                {
                    if (candidateAnswer.IsCorrect) break;
                    candidateAnswer = new Answer(word + " " + word2);
                    var remainderCharPool2 = charPool.SubtractWord(candidateAnswer.Value.Alphabetize());

                    if (remainderCharPool2 == null) continue;

                    if (remainderCharPool2 == string.Empty)
                    {
                        if (_encrypter.Hash(candidateAnswer.Value) == hash)
                        {
                            candidateAnswer.IsCorrect = true;
                        }
                        continue;
                    }

                    foreach (var word3 in _words)
                    {
                        if (candidateAnswer.IsCorrect) break;
                        candidateAnswer = new Answer(word + " " + word2 + " " + word3);
                        var remainderCharPool3 = charPool.SubtractWord(candidateAnswer.Value.Alphabetize());

                        if (remainderCharPool3 == null) continue;

                        if (remainderCharPool3 == string.Empty)
                        {
                            if (_encrypter.Hash(candidateAnswer.Value) == hash)
                            {
                                candidateAnswer.IsCorrect = true;
                            }
                            continue;
                        }

                        foreach (var word4 in _words)
                        {
                            if (candidateAnswer.IsCorrect) break;
                            candidateAnswer = new Answer(word + " " + word2 + " " + word3 + " " + word4);
                            var remainderCharPool4 = charPool.SubtractWord(candidateAnswer.Value.Alphabetize());

                            if (remainderCharPool4 == null) continue;

                            if (remainderCharPool4 == string.Empty)
                            {
                                if (_encrypter.Hash(candidateAnswer.Value) == hash)
                                {
                                    candidateAnswer.IsCorrect = true;
                                }
                                continue;
                            }
                        }
                    }

                }
            }

            if (candidateAnswer.IsCorrect)
            {
                return candidateAnswer.Value;
            }

            throw new NoPhraseFound("no phrase found");
        }
    }
}
