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

        public string GetDecryptedPhrase(string hash, string targetAnagram)
        {
            var anagramCharPool = targetAnagram.Alphabetize();
            var targetAnagramRelevantWords = RemoveIrrelevantWords(_words, anagramCharPool);
            var charPoolsToStringLists = CharPoolWithWords.GetDictionary(targetAnagramRelevantWords);

            var candidateAnswer = new PartialCharPool(new List<CharPoolWithWords>()); //answerType

            var memo = new Memo();

            var candidateBundles = Recursive(candidateAnswer, anagramCharPool, charPoolsToStringLists, memo);


            foreach (var bundle in candidateBundles)
            {
                // construct equivalent phrases
                var equivalentPhrases = new List<string> { "" };
                foreach (var charPoolWithWords in bundle.OrderredCharPoolsWithWords)
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
            IDictionary<string, List<string>> charPoolToWordListsList,
            Memo memo)
        {
            var newCandidates = new List<PartialCharPool>();

            foreach (var tuple in charPoolToWordListsList)
            {
                var newPartialPhraseCharPool = new PartialCharPool(partialCharPool, new CharPoolWithWords(tuple.Key, tuple.Value));

                // check against memo
                if (memo.Contains(newPartialPhraseCharPool.Value))
                {
                    //we have been here before...
                    var val = memo[newPartialPhraseCharPool.Value];
                    //its a dead end!
                    if (val == null)
                    {
                        continue;
                    }
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
                    newPartialPhraseCharPool.IsDeadend = false;
                    newCandidates.Add(newPartialPhraseCharPool);
                    partialCharPool.IsDeadend = false;
                    continue;
                }

                // inconclusive
                if (remainder.Length > 0)
                {

                    var candidates = Recursive(newPartialPhraseCharPool, anagramCharPool, charPoolToWordListsList, memo);
                    if (candidates.Count == 0)
                    {
                        partialCharPool.IsDeadend = partialCharPool.IsDeadend && true;
                    }
                    else
                    {
                        newCandidates.AddRange(candidates);
                        partialCharPool.IsDeadend = partialCharPool.IsDeadend && false;
                    }
                    continue;
                }
            }

            if (partialCharPool.IsDeadend)
            {
                memo.AddDeadEnd(partialCharPool);
            }
            else
            {
                //memo.AddSolution(partialCharPool);
            }

            return newCandidates;
        }
        private List<string> RemoveIrrelevantWords(IEnumerable<string> words, string anagram)
        {
            return words.Where(x => anagram.SubtractChars(x) != null).ToList();
        }

    }

    public class Memo
    {
        private readonly IDictionary<string, bool?> _dict;

        public Memo()
        {
            _dict = new Dictionary<string, bool?>();
        }

        public void AddSolution(PartialCharPool partialCharPool)
        {
            _dict.Add(partialCharPool.Value, true);
        }

        public void AddDeadEnd(PartialCharPool partialCharPool)
        {
            _dict.Add(partialCharPool.Value, null);
        }

        public bool Contains(string value)
        {
            return _dict.ContainsKey(value);
        }

        public bool? this[string value] => _dict[value];
    }
}
