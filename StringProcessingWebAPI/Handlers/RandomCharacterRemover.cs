using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StringProcessingWebAPI.Handlers
{

    public class RandomCharacterRemover : IRandomCharacterRemover
    {
        public async Task<string> RemoveRandomCharacter(string str)
        {
            try
            {
                int randomIndex = await GetRandomNumberLessThanLength(str.Length);
                if (randomIndex >= 0 && randomIndex < str.Length)
                {
                    string modifiedString = str.Remove(randomIndex, 1);
                    return modifiedString;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении символа: {ex.Message}");
            }

            return str;
        }

        private async Task<int> GetRandomNumberLessThanLength(int maxLength)
        {
            string apiUrl = $"http://www.randomnumberapi.com/api/v1.0/random?max={maxLength}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        int numeric = Int32.Parse(new string(json.Where(char.IsDigit).ToArray()));
                        return numeric;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении случайного числа: {ex.Message}");
            }

            Random rnd = new Random();
            return rnd.Next(maxLength);
        }
    }
}