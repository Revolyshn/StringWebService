using Microsoft.AspNetCore.Mvc;
using StringProcessingWebAPI.Handlers;
using System.ComponentModel.DataAnnotations;

namespace StringProcessingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StringProcessingController : Controller
    {
        private IStringProcessHandler _stringHandler;
        private IRandomCharacterRemover _randomCharacterRemover;

        public StringProcessingController(IStringProcessHandler stringHandler, IRandomCharacterRemover randomCharacterRemover)
        {
            _stringHandler = stringHandler;
            _randomCharacterRemover = randomCharacterRemover;
        }

        [HttpGet("process")]
        public async Task<IActionResult> StringHandler([FromQuery, Required, InputString] string inputString, [FromQuery] string sortAlgorithm = "quicksort")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var processedString = _stringHandler.ProcessString(inputString);
            var charCounts = _stringHandler.CountCharacterOccurrences(processedString);
            var longestVowelSubstring = _stringHandler.FindLongestVowelSubstring(processedString);

            // Определяем, какой алгоритм сортировки использовать
            string sortedString = _stringHandler.SortString(processedString, sortAlgorithm.ToLower());

            // Удаление случайного символа
            var modifiedString = await _randomCharacterRemover.RemoveRandomCharacter(processedString);

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

        // Атрибут проверки для валидации строки
        public class InputStringAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var inputString = value as string;
                var stringHandler = (IStringProcessHandler)validationContext.GetService(typeof(IStringProcessHandler));

                if (string.IsNullOrEmpty(inputString) || !stringHandler.CheckIfValidString(inputString))
                {
                    return new ValidationResult("Ошибка: Введены неподходящие символы.");
                }

                return ValidationResult.Success;
            }
        }
    }
}