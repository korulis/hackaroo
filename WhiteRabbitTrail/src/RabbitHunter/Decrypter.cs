using System;
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
            public bool IsCorrectLength { get; set; }

            public string Value { get; set; }

            public Answer() : this(partialPhrase: string.Empty, word: string.Empty)
            {
                IsCorrectLength = false;
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

        public class PartialCharPool
        {

            public List<CharPoolWithWords> CharPoolsWithWords { get; }
            public string CharPool { get; }
            public bool IsCharPoolEquivalentToAnagram { get; set; }

            public PartialCharPool(List<CharPoolWithWords> charPoolsWithWords)
            {
                IsCharPoolEquivalentToAnagram = false;
                CharPoolsWithWords = charPoolsWithWords;
                CharPool = string.Concat(charPoolsWithWords);
            }

            public PartialCharPool(PartialCharPool partialCharPool, CharPoolWithWords charPoolWithWords)
            {
                IsCharPoolEquivalentToAnagram = false;

                CharPoolsWithWords = new List<CharPoolWithWords>();
                CharPoolsWithWords.AddRange(partialCharPool.CharPoolsWithWords);
                CharPoolsWithWords.Add(charPoolWithWords);

                CharPool = string.Concat(partialCharPool.CharPool, charPoolWithWords.Value);

            }
        }

        public string GetDecryptedPhrase(string hash, string targetAnagram)
        {
            var anagramCharPool = targetAnagram.Alphabetize();
            var targetAnagramRelevantWords = RemoveIrrelevantWords(_words, anagramCharPool);
            var charPools = CharPoolWithWords.GetCharPools(targetAnagramRelevantWords);

            var candidateAnswer = new PartialCharPool(new List<CharPoolWithWords>()); //answerType

            var candidateBundles = Recursive(candidateAnswer, anagramCharPool, charPools);


            foreach (var bundle in candidateBundles)
            {
                if (bundle.IsCharPoolEquivalentToAnagram) // todo can be removed
                {
                    // construct equivalent phrases
                    var constructionList = new List<string> { "" };
                    foreach (var charPoolWithWords in bundle.CharPoolsWithWords)
                    {
                        var list2 = new List<string>();

                        foreach (var word in charPoolWithWords.Words)
                        {
                            foreach (var phraseUnderConstruction in constructionList)
                            {
                                if (phraseUnderConstruction == "")
                                {
                                    list2.Add(word);
                                }
                                list2.Add(phraseUnderConstruction + " " + word);
                            }
                        }

                        constructionList = list2;
                    }

                    foreach (var phrase in constructionList)
                    {
                        if (_encrypter.Hash(phrase) == hash)
                        {
                            return phrase;
                        }

                    }
                }
            }

            throw new NoPhraseFound("no phrase found");
        }

        // todo maybe can return Ienumerable<Answer> instead
        private List<PartialCharPool> Recursive(PartialCharPool partialCharPool, string anagramCharPool, IList<CharPoolWithWords> charPoolsWithWords)
        {
            if (partialCharPool.IsCharPoolEquivalentToAnagram) return new List<PartialCharPool> { partialCharPool }; //todo this could be removed

            var newCandidates = new List<PartialCharPool>();
            foreach (var newCharPoolWithWords in charPoolsWithWords)
            {
                var newPartialPhrase = new PartialCharPool(partialCharPool, newCharPoolWithWords);

                var remainderCharPool = anagramCharPool.SubtractWord(newPartialPhrase.CharPool.Alphabetize());

                if (remainderCharPool == null)
                {
                    continue;
                }

                if (remainderCharPool == string.Empty)
                {
                    newPartialPhrase.IsCharPoolEquivalentToAnagram = true;
                }

                newCandidates.AddRange(Recursive(newPartialPhrase, anagramCharPool, charPoolsWithWords));
            }
            return newCandidates;
        }

        private List<string> RemoveIrrelevantWords(IEnumerable<string> words, string anagram)
        {
            return words.Where(x => anagram.SubtractWord(x) != null).ToList();
        }

    }
}
