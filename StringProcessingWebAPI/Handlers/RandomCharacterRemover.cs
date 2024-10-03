using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StringProcessingWebAPI.Handlers
{

    public class RandomCharacterRemover : IRandomCharacterRemover
    {
        private readonly IConfiguration _configuration;

        public RandomCharacterRemover(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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
            var randomNumUrl = _configuration.GetSection("RandomApi").Get<string>()+maxLength;


            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(randomNumUrl);
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