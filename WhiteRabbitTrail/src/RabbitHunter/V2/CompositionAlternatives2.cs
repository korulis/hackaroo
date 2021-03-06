using System;
using System.Collections.Generic;
using System.Linq;
using RabbitHunter.V1;

namespace RabbitHunter.V2
{
    public class CompositionAlternatives2 : AnagramBuilder
    {
        //todo validate this is not null on any construction
        public IReadOnlyList<BlobComposition> BlobCompositions => _blobCompositions;

        private readonly List<BlobComposition> _blobCompositions;
        public string CharPool { get; }

        public bool IsDeadend => this == DeadEnd;


        public CompositionAlternatives2(string charPool, List<BlobComposition> blobCompositions)
        {
            if (string.IsNullOrEmpty(charPool))
            {
                throw new ArgumentException("Can not be null or empty", nameof(charPool));
            }
            if (blobCompositions == null)
            {
                throw new ArgumentException("Can not be null", nameof(blobCompositions));
            }


            _blobCompositions = blobCompositions;
            CharPool = charPool;
        }

        //todo test
        public CompositionAlternatives2(CompositionAlternatives2 alternatives1, CompositionAlternatives2 alternatives2)
        {
            _blobCompositions = new List<BlobComposition>();
            _blobCompositions.AddRange(alternatives1.BlobCompositions);
            _blobCompositions.AddRange(alternatives2.BlobCompositions);

            CharPool = alternatives1.CharPool;
        }


        //todo test
        public static CompositionAlternatives2 GetCombined(CompositionAlternatives2 prefixes, Blob suffix)
        {
            if (prefixes == null)
            {
                throw new ArgumentException("Prefix composition alternatives can not be null", nameof(prefixes));
            }

            if (suffix == null)
            {
                throw new ArgumentException("Suffix blob can not be null", nameof(suffix));
            }

            if (prefixes.IsDeadend)
            {
                return DeadEnd;
            }

            var blobCompositions = prefixes.BlobCompositions.Select(x => new BlobComposition(x, suffix)).ToList();
            var charPool = string.Concat(prefixes.CharPool, suffix.CharPool).Alphabetize();
            return new CompositionAlternatives2(charPool, blobCompositions);
        }

        private CompositionAlternatives2() { }

        //todo encapsulate in CharPool
        public static CompositionAlternatives2 DeadEnd = new CompositionAlternatives2();

        public IEnumerable<string> BuildAnagrams()
        {
            return _blobCompositions.SelectMany(x => x.BuildAnagrams());
        }

        public void Add(BlobComposition composition)
        {
            _blobCompositions.Add(composition);
        }
    }
}