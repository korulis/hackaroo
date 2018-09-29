using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using RabbitHunter.V1;
using RabbitHunter.V2;
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
            var blobDictionary = Blob.FromWordList(targetAnagramRelevantWords);


            var alternatives = RecursiveShrinking(anagramCharPool, blobDictionary, new Memo2(), 1);

            var anagramCandidates = alternatives.BuildAnagrams();

            var answers = new List<string>();

            foreach (var anagramCandidate in anagramCandidates)
            {
                if (_encrypter.Hash(anagramCandidate) == hash)
                {
                    answers.Add(anagramCandidate);
                }
            }

            return string.Join(" & ", answers);
        }

        private CompositionAlternatives2 RecursiveShrinking(
            string anagramCharPool,
            IList<Blob> dictionary,
            Memo2 memo,
            int level)
        {
            if (memo.Has(anagramCharPool))
            {
                return memo.Get(anagramCharPool);
            }

            foreach (var wordEquivalencyClass in dictionary)
            {
                var difference = anagramCharPool.SubtractChars(wordEquivalencyClass.CharPool);


                switch (difference)
                {
                    case null: //negative
                        break;
                    case "": // solution
                        var solution = new BlobComposition(new List<Blob> { wordEquivalencyClass });
                        memo.Add(anagramCharPool, solution);
                        break;
                    default: //inconclusive
                        var sols = CompositionAlternatives2.GetCombined(RecursiveShrinking(difference, dictionary, memo, level++), wordEquivalencyClass);
                        if (sols.IsDeadend)
                        {
                            memo.AddDeadEnd(anagramCharPool);
                        }
                        else
                        {
                            memo.AddMultiple(anagramCharPool, sols);
                        }
                        break;
                }

            }

            if (!memo.Has(anagramCharPool))
            {
                memo.AddDeadEnd(anagramCharPool);
            }

            return memo.Get(anagramCharPool);
        }

        public string GetDecryptedPhrase2(string hash, string targetAnagram)
        {
            var anagramCharPool = targetAnagram.Alphabetize();
            var targetAnagramRelevantWords = RemoveIrrelevantWords(_words, anagramCharPool);
            var equivalencyClasses = WordEquivalencyClass.FromWordList(targetAnagramRelevantWords);

            var candidateAnswer = new WordEquivalencyClassComposition(new List<WordEquivalencyClass>()); //answerType

            var memo = new Memo(anagramCharPool);

            var candidateBundles2 = Recursive(candidateAnswer, anagramCharPool, equivalencyClasses, memo, 0);

            var candidateBundles = memo.Solutions.TempList;

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
