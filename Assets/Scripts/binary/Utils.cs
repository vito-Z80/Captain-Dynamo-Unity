using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace binary
{
    public class Utils
    {
        private const string NumPattern = @"\d+";

        public static List<int> ExtractNumbers(string input)
        {
            var regex = new Regex(NumPattern);
            var numbers = new List<int>();
            foreach (Match match in regex.Matches(input))
            {
                if (int.TryParse(match.Value, out var number))
                {
                    numbers.Add(number);
                }
            }
            return numbers;
        }
    }
}