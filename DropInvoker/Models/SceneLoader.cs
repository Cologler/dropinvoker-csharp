using DropInvoker.Models.Configurations;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DropInvoker.Models
{
    class SceneLoader
    {
        private readonly string _path;

        public SceneLoader(string path)
        {
            this._path = path;
        }

        public string Name => Path.GetFileNameWithoutExtension(this._path);

        public Scene Load()
        {
            var text = File.ReadAllText(this._path);
            var json = JsonSerializer.Deserialize<SceneJson>(text);
            return new Scene(json);
        }
    }
}
