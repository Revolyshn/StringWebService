using StringProcessingWebAPI.Sorting;

namespace StringProcessingWebAPI.Handlers
{
    public class StringProcessHandler : IStringProcessHandler
    {
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
            foreach (char c in input)
            {
                if (!char.IsLetter(c) || !char.IsLower(c) || c < 'a' || c > 'z')
                {
                    return false;
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