using System.Collections.Generic;

namespace RabbitHunter
{
    public interface AnagramBuilder
    {
        IEnumerable<string> BuildAnagrams();
    }
}