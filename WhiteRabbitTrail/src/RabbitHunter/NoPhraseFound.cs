using System;

namespace RabbitHunter
{
    public class NoPhraseFound : Exception
    {
        public NoPhraseFound(string message) : base(message)
        {
            
        }
    }
}