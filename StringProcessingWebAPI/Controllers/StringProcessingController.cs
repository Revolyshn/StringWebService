using Microsoft.AspNetCore.Mvc;

namespace StringProcessingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class StringProcessingController : Controller
    {
        [HttpGet("process")]
        public async Task<IActionResult> StringHandler([FromQuery] string inputString, [FromQuery] string sortAlgorithm = "quicksort")
        {
            if (string.IsNullOrEmpty(inputString) || !CheckIfValidString(inputString))
            {
                return BadRequest(new { message = "Ошибка: Введены неподходящие символы." });
            }

            var processedString = ProcessString(inputString);
            var charCounts = CountCharacterOccurrences(processedString);
            var longestVowelSubstring = FindLongestVowelSubstring(processedString);

            // Определяем, какой алгоритм сортировки использовать
            string sortedString;
            if (sortAlgorithm.ToLower() == "treesort")
            {
                var treesort = new Treesort();
                treesort.BuildTree(processedString);
                sortedString = treesort.GetSortedString();
            }
            else
            {
                // По умолчанию используется Quicksort
                sortedString = new Quicksort().Sort(processedString);
            }

            // Удаление случайного символа
            var modifiedString = await RemoveRandomCharacter(processedString); // Используем await для асинхронного вызова

            var result = new
            {
                ProcessedString = processedString,
                CharCounts = charCounts,
                LongestVowelSubstring = longestVowelSubstring,
                SortedString = sortedString,
                ModifiedString = modifiedString
            };

            return Ok(result);
        }

        static bool CheckIfValidString(string input)
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

        static string ProcessString(string input)
        {
            int length = input.Length;

            if (length % 2 == 0) // Если длина строки четная
            {
                int mid = length / 2;
                string firstHalf = input.Substring(0, mid);
                string secondHalf = input.Substring(mid);

                // Переворачиваем обе подстроки
                string firstHalfReversed = ReverseString(firstHalf);
                string secondHalfReversed = ReverseString(secondHalf);

                // Соединяем обратно
                return firstHalfReversed + secondHalfReversed;
            }

            // Если длина строки нечетная
            string reversedString = ReverseString(input);
            return reversedString + input;

        }

        static string ReverseString(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        static Dictionary<char, int> CountCharacterOccurrences(string str)
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

        static string FindLongestVowelSubstring(string str)
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
                            break; // Найдена первая гласная с конца
                        }
                    }
                }
            }

            return longestSubstring;
        }

        static async Task<string> RemoveRandomCharacter(string str)
        {
            try
            {
                int randomIndex = await GetRandomNumberLessThanLength(str.Length);
                if (randomIndex >= 0 && randomIndex < str.Length)
                {
                    string modifiedString = str.Remove(randomIndex, 1);
                    return modifiedString; // Возвращаем обработанную строку
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении символа: {ex.Message}");
            }

            // В случае ошибки или неправильного индекса возвращаем исходную строку
            return str;
        }

        static async Task<int> GetRandomNumberLessThanLength(int maxLength)
        {
            // Используем API для получения случайного числа меньше maxLength
            string apiUrl = $"http://www.randomnumberapi.com/api/v1.0/random?max={maxLength}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var numeric = Int32.Parse(new String(json.Where(char.IsDigit).ToArray()));
                        return numeric;
                    }
                    else
                    {
                        // В случае ошибки API, генерируем случайное число локально
                        Random rnd = new Random();
                        return rnd.Next(maxLength);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении случайного числа: {ex.Message}");

                // В случае ошибки API, генерируем случайное число локально
                Random rnd = new Random();
                return rnd.Next(maxLength);
            }
        }
    }
}