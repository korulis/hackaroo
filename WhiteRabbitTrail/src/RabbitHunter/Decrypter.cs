﻿using System;
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
            var charPoolsToStringLists = WordEquivalencyClass.GetDictionary(targetAnagramRelevantWords);

            var candidateAnswer = new WordEquivalencyClassComposition(new List<WordEquivalencyClass>()); //answerType

            var memo = new Memo();

            var candidateBundles = Recursive(candidateAnswer, anagramCharPool, charPoolsToStringLists, memo);


            foreach (var bundle in candidateBundles)
            {
                // construct equivalent phrases
                var equivalentPhrases = new List<string> { "" };
                foreach (var charPoolWithWords in bundle.OrderedListOdWordEquivalencyClasses)
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
        private List<WordEquivalencyClassComposition> Recursive(
            WordEquivalencyClassComposition wordEquivalencyClassComposition,
            string anagramCharPool,
            IDictionary<string, List<string>> charPoolToWordListsList,
            Memo memo)
        {
            var newCandidates = new List<WordEquivalencyClassComposition>();

            foreach (var tuple in charPoolToWordListsList)
            {
                var newPartialPhraseCharPool = new WordEquivalencyClassComposition(wordEquivalencyClassComposition, new WordEquivalencyClass(tuple.Key, tuple.Value));

                // check against memo
                if (memo.Contains(newPartialPhraseCharPool.CharPool))
                {
                    //we have been here before...
                    var val = memo[newPartialPhraseCharPool.CharPool];
                    //its a dead end!
                    if (val == null)
                    {
                        continue;
                    }
                }

                var remainder = anagramCharPool.SubtractChars(newPartialPhraseCharPool.CharPool.Alphabetize());

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
                    wordEquivalencyClassComposition.IsDeadend = false;
                    continue;
                }

                // inconclusive
                if (remainder.Length > 0)
                {

                    var candidates = Recursive(newPartialPhraseCharPool, anagramCharPool, charPoolToWordListsList, memo);
                    if (candidates.Count == 0)
                    {
                        wordEquivalencyClassComposition.IsDeadend = wordEquivalencyClassComposition.IsDeadend && true;
                    }
                    else
                    {
                        newCandidates.AddRange(candidates);
                        wordEquivalencyClassComposition.IsDeadend = wordEquivalencyClassComposition.IsDeadend && false;
                    }
                    continue;
                }
            }

            if (wordEquivalencyClassComposition.IsDeadend)
            {
                memo.AddDeadEnd(wordEquivalencyClassComposition);
            }
            else
            {
                //memo.AddSolution(ListOfCompositionAlternatives);
            }

            return newCandidates;
        }

        private List<string> RemoveIrrelevantWords(IEnumerable<string> words, string anagram)
        {
            return words.Where(x => anagram.SubtractChars(x) != null).ToList();
        }
    }
}
