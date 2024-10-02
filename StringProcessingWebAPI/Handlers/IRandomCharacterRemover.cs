namespace StringProcessingWebAPI.Handlers
{
    public interface IRandomCharacterRemover
    {
        Task<string> RemoveRandomCharacter(string str);
    }
}