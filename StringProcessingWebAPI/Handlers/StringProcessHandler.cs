using StringProcessingWebAPI.Sorting;

namespace StringProcessingWebAPI.Handlers
{
    public class StringProcessHandler : IStringProcessHandler
    {

        private readonly IConfiguration _configuration;

        public StringProcessHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ProcessString(string input)
        {
            int length = input.Length;

            if (length % 2 == 0)
            {
                int mid = length / 2;
                string firstHalf = input.Substring(0, mid);
                string secondHalf = input.Substring(mid);

                string firstHalfReversed = ReverseString(firstHalf);
                string secondHalfReversed = ReverseString(secondHalf);

                return firstHalfReversed + secondHalfReversed;
            }

            string reversedString = ReverseString(input);
            return reversedString + input;
        }

        public Dictionary<char, int> CountCharacterOccurrences(string str)
        {
            Dictionary<char, int> charCounts = new Dictionary<char, int>();

            foreach (char c in str)
            {
                if (charCounts.ContainsKey(c))
                {
                    charCounts[c]++;
                }
                else
                {
                    charCounts.Add(c, 1);
                }
            }

            return charCounts;
        }

        public string FindLongestVowelSubstring(string str)
        {
            string longestSubstring = "";
            HashSet<char> vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u', 'y' };

            for (int i = 0; i < str.Length; i++)
            {
                if (vowels.Contains(str[i]))
                {
                    for (int j = str.Length - 1; j > i; j--)
                    {
                        if (vowels.Contains(str[j]))
                        {
                            string substring = str.Substring(i, j - i + 1);
                            if (substring.Length > longestSubstring.Length)
                            {
                                longestSubstring = substring;
                            }
                            break;
                        }
                    }
                }
            }

            return longestSubstring;
        }

        public bool CheckIfValidString(string input)
        {
            // Проверка на допустимые символы
            foreach (char c in input)
            {
                if (!char.IsLetter(c) || !char.IsLower(c) || c < 'a' || c > 'z')
                {
                    return false;
                }
            }

            // Проверка на наличие строки в чёрном списке
            var blackList = _configuration.GetSection("Settings:BlackList").Get<List<string>>();
            foreach (var blackListedWord in blackList)
            {
                if (input.Contains(blackListedWord))
                {
                    return false; // Если строка содержит слово из чёрного списка
                }
            }

            return true;
        }

        private string ReverseString(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public string SortString(string str, string sortAlgorithm)
        {
            if (sortAlgorithm == "treesort")
            {
                var treesort = new Treesort();
                treesort.BuildTree(str);
                return treesort.GetSortedString();
            }
            else
            {
                var quicksort = new Quicksort();
                return quicksort.Sort(str);
            }
        }
    }
}