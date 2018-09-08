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
            public string Value { get; }

            public PartialCharPool(List<CharPoolWithWords> charPoolsWithWords)
            {
                CharPoolsWithWords = charPoolsWithWords;
                Value = string.Concat(charPoolsWithWords).Alphabetize();
            }

            public PartialCharPool(PartialCharPool partialCharPool, CharPoolWithWords charPoolWithWords)
            {
                CharPoolsWithWords = new List<CharPoolWithWords>();
                CharPoolsWithWords.AddRange(partialCharPool.CharPoolsWithWords);
                CharPoolsWithWords.Add(charPoolWithWords);

                Value = string.Concat(partialCharPool.Value, charPoolWithWords.Value).Alphabetize();

            }
        }

        public string GetDecryptedPhrase(string hash, string targetAnagram)
        {
            var anagramCharPool = targetAnagram.Alphabetize();
            var targetAnagramRelevantWords = RemoveIrrelevantWords(_words, anagramCharPool);
            var charPoolsToStringLists = CharPoolWithWords.GetDictionary(targetAnagramRelevantWords);

            var candidateAnswer = new PartialCharPool(new List<CharPoolWithWords>()); //answerType

            var deadEnds = new Dictionary<string,bool>();

            var candidateBundles = Recursive(candidateAnswer, anagramCharPool, charPoolsToStringLists, deadEnds);


            foreach (var bundle in candidateBundles)
            {
                // construct equivalent phrases
                var equivalentPhrases = new List<string> { "" };
                foreach (var charPoolWithWords in bundle.CharPoolsWithWords)
                {
                    var tempList = new List<string>();

                    foreach (var word in charPoolWithWords.Words)
                    {
                        foreach (var phraseUnderConstruction in equivalentPhrases)
                        {
                            if (phraseUnderConstruction == "")
                            {
                                tempList.Add(word);
                            }
                            tempList.Add(phraseUnderConstruction + " " + word);
                        }
                    }

                    equivalentPhrases = tempList;
                }

                foreach (var phrase in equivalentPhrases)
                {
                    if (_encrypter.Hash(phrase) == hash)
                    {
                        return phrase;
                    }

                }
            }


            throw new NoPhraseFound("no phrase found");
        }

        // todo maybe can return Ienumerable<Answer> instead
        private List<PartialCharPool> Recursive(
            PartialCharPool partialCharPool,
            string anagramCharPool,
            IDictionary<string, List<string>> charPoolsToWordLists,
            IDictionary<string,bool> deadEnds)
        {
            var newCandidates = new List<PartialCharPool>();
            bool partialCharPoolIsDeadEnd = true;

            foreach (var tuple in charPoolsToWordLists)
            {
                var newPartialPhraseCharPool = new PartialCharPool(partialCharPool, new CharPoolWithWords(tuple.Key, tuple.Value));

                if (deadEnds.ContainsKey(newPartialPhraseCharPool.Value) )
                {
                    continue;
                }

                var remainder = anagramCharPool.SubtractChars(newPartialPhraseCharPool.Value.Alphabetize());

                // not a fit
                if (remainder == null)
                {
                    continue;
                }

                // a fit
                if (remainder.Length == 0)
                {
                    var candidates = new List<PartialCharPool>() {newPartialPhraseCharPool};
                    if (candidates.Count == 0)
                    {
                        partialCharPoolIsDeadEnd = true;
                    }
                    else
                    {
                        newCandidates.AddRange(candidates);
                        partialCharPoolIsDeadEnd = false;
                    }
                    continue;
                }

                // inconclusive
                if (remainder.Length > 0)
                {

                    var candidates = Recursive(newPartialPhraseCharPool, anagramCharPool, charPoolsToWordLists, deadEnds);
                    if (candidates.Count == 0)
                    {
                        partialCharPoolIsDeadEnd = true;
                    }
                    else
                    {
                        newCandidates.AddRange(candidates);
                        partialCharPoolIsDeadEnd = false;
                    }
                    continue;
                }


            }

            if (partialCharPoolIsDeadEnd)
            {
                deadEnds.Add(partialCharPool.Value, true);
            }

            return newCandidates;
        }

        private List<string> RemoveIrrelevantWords(IEnumerable<string> words, string anagram)
        {
            return words.Where(x => anagram.SubtractChars(x) != null).ToList();
        }

    }
}
