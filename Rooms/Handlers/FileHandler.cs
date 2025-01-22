using System.Text.Json;

namespace Rooms.Handlers
{
    public class FileHandler
    {
        private JsonSerializerOptions _serializerOptions;

        public FileHandler()
        {
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            
        }
        
        public T ReadJsonFile<T>(string filename)
        {
            try
            {
                string fileContents = GetFileContents(filename);

                var deserializedData = JsonSerializer.Deserialize<T>(fileContents, _serializerOptions) ?? throw new NullReferenceException();
                return deserializedData;
            }
            catch
            {
                Console.WriteLine("Exception occured when reading file {0}", filename);
                throw;
            }
        }

        public string GetFileContents(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException();
            }

            var fileContents = File.ReadAllText(filename);

            if (string.IsNullOrWhiteSpace(fileContents))
            {
                throw new InvalidOperationException();
            }

            return fileContents;
        }
    }
}
