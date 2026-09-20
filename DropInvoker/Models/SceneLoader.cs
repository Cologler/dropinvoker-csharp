using System.IO;
using System.Text.Json;

using DropInvoker.Models.Configurations;

namespace DropInvoker.Models
{
    class SceneLoader(string path)
    {
        public string Name => Path.GetFileNameWithoutExtension(path);

        public Scene Load()
        {
            var text = File.ReadAllText(path);
            var json = JsonSerializer.Deserialize<SceneJson>(text);
            return new Scene(json);
        }
    }
}
