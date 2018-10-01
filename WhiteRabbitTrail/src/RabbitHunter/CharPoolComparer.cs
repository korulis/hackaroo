using System.Collections.Generic;

namespace RabbitHunter
{
    public class CharPoolComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (!string.IsNullOrEmpty(x) && !string.IsNullOrEmpty(y) && x.Length != y.Length)
            {
                if (x.Length > y.Length)
                {
                    return 1;
                }
                if (x.Length < y.Length)
                {
                    return -1;
                }
            }

            return string.Compare(x, y);
        }
    }
}