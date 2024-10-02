namespace StringProcessingWebAPI.Handlers
{
    public interface IStringProcessHandler
    {
        string ProcessString(string input);
        Dictionary<char, int> CountCharacterOccurrences(string str);
        string FindLongestVowelSubstring(string str);
        bool CheckIfValidString(string input);
        string SortString(string str, string sortAlgorithm);
    }
}