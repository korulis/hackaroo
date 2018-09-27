using System;
using System.Collections.Generic;
using System.Linq;
using RabbitHunter.V1;

namespace RabbitHunter.V2
{
    public class CompositionAlternatives2 : AnagramBuilder
    {
        public IReadOnlyList<BlobComposition> TempList => _listOfCompositionAlternatives;

        public bool IsDeadend;
        private readonly List<BlobComposition> _listOfCompositionAlternatives;
        public string CharPool { get; private set; }



        public List<string> BuildAnagrams()
        {
            throw new NotImplementedException();
        }
    }
}