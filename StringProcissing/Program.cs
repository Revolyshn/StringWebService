using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите строку (введите пустую строку для выхода):");

        while (true)
        {
            string inputString = Console.ReadLine();

            // Проверяем, является ли введённая строка пустой
            if (string.IsNullOrEmpty(inputString))
            {
                break; // Выходим из цикла, если строка пустая
            }

            if (CheckIfValidString(inputString))
            {
                string processedString = ProcessString(inputString);
                Console.WriteLine("Обработанная строка: " + processedString);

                Dictionary<char, int> charCounts = CountCharacterOccurrences(processedString);
                foreach (var kvp in charCounts)
                {
                    Console.WriteLine($"Символ '{kvp.Key}' встречается {kvp.Value} раз.");
                }

                string longestVowelSubstring = FindLongestVowelSubstring(processedString);
                Console.WriteLine($"Самая длинная подстрока, начинающаяся и заканчивающаяся на гласную: {longestVowelSubstring}");

                Console.WriteLine("Выберите алгоритм сортировки:");
                Console.WriteLine("1. Quicksort");
                Console.WriteLine("2. Treesort");
                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Quicksort quicksort = new Quicksort();
                            string sortedStringQuicksort = quicksort.Sort(processedString);
                            Console.WriteLine("Отсортированная обработанная строка (Quicksort): " + sortedStringQuicksort);
                            break;
                        case 2:
                            Treesort treesort = new Treesort();
                            treesort.BuildTree(processedString);
                            string sortedStringTreesort = treesort.GetSortedString();
                            Console.WriteLine("Отсортированная обработанная строка (Treesort): " + sortedStringTreesort);
                            break;
                        default:
                            Console.WriteLine("Некорректный выбор алгоритма.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный ввод.");
                }

                // Получаем случайное число и удаляем символ в строке
                RemoveRandomCharacter(processedString);
            }
            else
            {
                Console.WriteLine("Ошибка: Введены неподходящие символы.");
            }
        }
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
        else // Если длина строки нечетная
        {
            string reversedString = ReverseString(input);
            return reversedString + input;
        }
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

    static async void RemoveRandomCharacter(string str)
    {
        try
        {
            int randomIndex = await GetRandomNumberLessThanLength(str.Length);
            if (randomIndex >= 0 && randomIndex < str.Length)
            {
                string modifiedString = str.Remove(randomIndex, 1);
                Console.WriteLine($"\"Урезанная\" обработанная строка (удалён символ в позиции {randomIndex}): {modifiedString}");
            }
            else
            {
                Console.WriteLine("Ошибка: Получен некорректный индекс для удаления символа.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении символа: {ex.Message}");
        }
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
                    string json = await response.Content.ReadAsStringAsync();
                    int numeric = Int32.Parse(new String(json.Where(char.IsDigit).ToArray()));
                    Console.WriteLine(numeric);
                    return numeric;
                }
                else
                {
                    Console.WriteLine("Неудалось получить случайное число. Используется локальный генератор случайных чисел.");

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

    public class RandomNumberData
    {
        public int Number { get; set; }
    }
}