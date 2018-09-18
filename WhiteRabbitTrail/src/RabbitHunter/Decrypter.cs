using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            var equivalencyClasses = WordEquivalencyClass.FromWordList(targetAnagramRelevantWords);

            var candidateAnswer = new WordEquivalencyClassComposition(new List<WordEquivalencyClass>()); //answerType

            var memo = new Memo(anagramCharPool);

            var candidateBundles2 = Recursive(candidateAnswer, anagramCharPool, equivalencyClasses, memo, 0);

            var candidateBundles = memo.Solutions.TempList;


            ////log
            var messages = new List<string>()
            {
                DateTime.Now.ToString(CultureInfo.InvariantCulture),
                memo.SolutionCount + " count of the anagram charPool alternative combinations.",
                memo.Count + " charPools count."
            };
            Log(messages.ToArray());

            foreach (var bundle in candidateBundles)
            {
                // construct equivalent phrases
                var equivalentPhrases = new List<string> { "" };
                foreach (var charPoolWithWords in bundle.OrderedListOfWordEquivalencyClasses)
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

        public static void Log(params string[] messages)
        {
            File.AppendAllLines("C:/asdf.txt", messages);
        }

        private List<WordEquivalencyClassComposition> Recursive(
            WordEquivalencyClassComposition currentWord,
            string anagramCharPool,
            IList<WordEquivalencyClass> equivalencyClasses,
            Memo memo,
            int level)
        {
            var newCandidates = new List<WordEquivalencyClassComposition>();

            foreach (var equivalencyClass in equivalencyClasses)
            {
                var newPartialPhraseComposition = new WordEquivalencyClassComposition(currentWord,
                    new WordEquivalencyClass(equivalencyClass.CharPool, equivalencyClass.Words));

                var remainder = anagramCharPool.SubtractChars(newPartialPhraseComposition.CharPool.Alphabetize());

                // not a fit
                if (remainder == null)
                {
                    newPartialPhraseComposition.IsDeadend = true;
                    if (!memo.ContainsKey(newPartialPhraseComposition.CharPool))
                    {
                        memo.AddDeadEnd(newPartialPhraseComposition);
                    }
                    continue;
                }

                // a fit
                if (remainder.Length == 0)
                {
                    newPartialPhraseComposition.IsDeadend = false;
                    memo.AddSolution(newPartialPhraseComposition);

                    newCandidates.Add(newPartialPhraseComposition);
                    continue;
                }

                // inconclusive
                if (remainder.Length > 0)
                {
                    newPartialPhraseComposition.IsDeadend = false;
                    // check new word against memo
                    if (memo.ContainsKey(newPartialPhraseComposition.CharPool))
                    {
                        //we have been here before...
                        var val = memo[newPartialPhraseComposition.CharPool];
                        if (val == null)
                        {
                            //its a dead end!
                            continue;
                        }
                        else
                        {
                            if (val.IsDeadend == false)
                            {
                                memo.GenerateAndAddFullSolution_FromIncomplete(newPartialPhraseComposition);
                                continue;
                            }
                            else
                            {
                                throw new ArgumentException("I should not be here again!");
                            }
                        }
                    }

                    var countBeforeRecursion = memo.SolutionCount;
                    var candidates = Recursive(newPartialPhraseComposition, anagramCharPool, equivalencyClasses, memo, level + 1);
                    var countAfterRecursion = memo.SolutionCount;

                    if (countBeforeRecursion == countAfterRecursion)
                    {
                        newPartialPhraseComposition.IsDeadend = true;
                        memo.AddDeadEnd(newPartialPhraseComposition);
                    }
                    else
                    {
                        newPartialPhraseComposition.IsDeadend = false;
                        memo.AddSolvable(newPartialPhraseComposition);

                        newCandidates.AddRange(candidates);
                    }
                    continue;
                }
            }

            return newCandidates;
        }

        private List<string> RemoveIrrelevantWords(IEnumerable<string> words, string anagram)
        {
            return words.Where(x => anagram.SubtractChars(x) != null).ToList();
        }
    }
}
