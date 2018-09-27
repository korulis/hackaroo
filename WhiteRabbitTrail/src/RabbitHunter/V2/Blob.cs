using System.Collections.Generic;
using System.Linq;
using RabbitHunter.V1;

namespace RabbitHunter.V2
{
    public class Blob
    {
        public string CharPool { get; }

        public IReadOnlyCollection<string> Words { get; }

        public Blob(string charPool, IReadOnlyCollection<string> words)
        {
            CharPool = charPool;
            Words = words;
        }
    }
}