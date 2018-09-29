using System;
using System.Collections.Generic;
using System.Linq;

namespace RabbitHunter.V2
{
    public class Memo2
    {
        private readonly Dictionary<string, CompositionAlternatives2> _dict;

        public Memo2()
        {
            _dict = new Dictionary<string, CompositionAlternatives2>();
        }

        //todo test
        public void Add(string anagramCharPool, BlobComposition composition)
        {
            ValidateInput(anagramCharPool, composition);

            if (_dict.ContainsKey(anagramCharPool))
            {
                var alternatives = _dict[anagramCharPool];
                alternatives.Add(composition);
            }
            else
            {
                _dict.Add(anagramCharPool, new CompositionAlternatives2(anagramCharPool, new List<BlobComposition> { composition }));
            }
        }

        //todo test
        public bool Has(string anagramCharPool)
        {
            return _dict.ContainsKey(anagramCharPool);
        }

        //todo test
        public CompositionAlternatives2 Get(string anagramCharPool)
        {
            return _dict[anagramCharPool];
        }

        //todo test
        public void AddMultiple(string anagramCharPool, CompositionAlternatives2 alternatives)
        {
            ValidateInputMultiple(anagramCharPool, alternatives);

            if (_dict.ContainsKey(anagramCharPool))
            {
                var els = _dict[anagramCharPool];
                _dict[anagramCharPool] = new CompositionAlternatives2(alternatives, els);
            }
            else
            {
                _dict.Add(anagramCharPool, alternatives);
            }
        }

        //todo test
        private void ValidateInputMultiple(string anagramCharPool, CompositionAlternatives2 alternatives)
        {
            if (alternatives == null)
            {
                throw new ArgumentException("Can not be null", nameof(alternatives));
            }

            if (string.IsNullOrEmpty(anagramCharPool))
            {
                throw new ArgumentException("Can not be null or empty", nameof(anagramCharPool));
            }
        }


        private static void ValidateInput(string anagramCharPool, BlobComposition blob)
        {
            if (blob == null)
            {
                throw new ArgumentException("Can not be null", nameof(blob));
            }

            if (string.IsNullOrEmpty(anagramCharPool))
            {
                throw new ArgumentException("Can not be null or empty", nameof(anagramCharPool));
            }
        }

        public void AddDeadEnd(string anagramCharPool)
        {
            _dict.Add(anagramCharPool, CompositionAlternatives2.DeadEnd);
        }
    }
}