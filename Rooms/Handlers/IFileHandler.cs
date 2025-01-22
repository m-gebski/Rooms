namespace Rooms.Handlers
{
    public interface IFileHandler
    {
        public T ReadJsonFile<T>(string filename);
        public string GetFileContents(string filename);
    }
}