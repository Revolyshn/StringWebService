using System;
using System.Collections.Generic;

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
}