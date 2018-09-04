using System.Linq;

namespace RabbitHunter
{
    public static class SortedStringExtensions
    {
        public static string Alphabetize(this string targetAnagram)
        {
            var noBlanks = targetAnagram.Replace(" ", "");
            var alphabetizedAnagram = noBlanks.ToCharArray().ToList();
            alphabetizedAnagram.Sort();
            var array = alphabetizedAnagram.ToArray();
            return new string(array);
        }

        public static string SubtractWord(this string inital, string input)
        {
            var initialAsList = inital.ToArray().ToList();
            var inputArray = input.ToArray();
            if (inputArray.Contains(' ')) return null;
            if (input.Length > inital.Length) return null;

            foreach (var inChar in inputArray)
            {
                var success = initialAsList.Remove(inChar);
                if (!success)
                {
                    return null;
                }
            }

            // initialAsList.Sort();
            // return new string(initialAsList.ToArray());
            return new string(initialAsList.ToArray()).Alphabetize();
        }
    }
}