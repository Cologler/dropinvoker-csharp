using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using RLauncher.Abstractions;
using RLauncher.Exceptions;

namespace RLauncher.Json;

internal class JsonDocumentLoader : IDocumentLoader<IRunnerData>, IDocumentLoader<ILauncherData>
{
    public ValueTask<bool> CanLoadAsync(string extensionName) 
        => new(extensionName is { } && extensionName.EndsWith(".json", StringComparison.OrdinalIgnoreCase));

    ValueTask<T> LoadAsync<T>(string filePath, ConfigurationFileTypes docType)
    {
        string? content;
        try
        {
            filePath = Path.GetFullPath(filePath);
            content = File.ReadAllText(filePath);
        }
        catch (Exception e)
        {
            throw new InvalidRLauncherConfigurationFileException("Unable read from file", e)
            {
                FileType = docType,
                FileFullPath = filePath
            };
        }

        T? doc;
        try
        {
            doc = JsonSerializer.Deserialize<T>(content);
        }
        catch (JsonException e)
        {
            throw new InvalidRLauncherConfigurationFileException("Unable parse from json", e)
            {
                FileType = docType,
                FileFullPath = filePath,
                FileContent = content
            };
        }

        if (doc is null)
        {
            throw new InvalidRLauncherConfigurationFileException("Unable load from null")
            {
                FileType = docType,
                FileFullPath = filePath,
                FileContent = content
            };
        }

        return new(doc);
    }

    async ValueTask<IRunnerData> IDocumentLoader<IRunnerData>.LoadAsync(string filePath) 
        => await this.LoadAsync<RunnerJson>(filePath, ConfigurationFileTypes.Runner).ConfigureAwait(false);

    async ValueTask<ILauncherData> IDocumentLoader<ILauncherData>.LoadAsync(string filePath)
        => await this.LoadAsync<LauncherJson>(filePath, ConfigurationFileTypes.Launcher).ConfigureAwait(false);

    record RunnerJson : IRunnerData
    {
        [JsonPropertyName(PropertyNames.Executable)]
        public string? Executable { get; set; }

        [JsonPropertyName(PropertyNames.Arguments)]
        public string?[]? Arguments { get; set; }

        [JsonPropertyName(PropertyNames.WorkingDirectory)]
        public string? WorkingDirectory { get; set; }
    }

    record LauncherJson : ILauncherData
    {
        [JsonPropertyName(PropertyNames.Name)]
        public string? Name { get; set; }

        [JsonPropertyName(PropertyNames.Description)]
        public string? Description { get; set; }

        [JsonPropertyName(PropertyNames.Runner)]
        public string? Runner { get; set; }

        [JsonPropertyName(PropertyNames.Template)]
        public string? Template { get; set; }

        [JsonPropertyName(PropertyNames.Arguments)]
        public string?[]? Arguments { get; set; }

        [JsonPropertyName(PropertyNames.WorkingDirectory)]
        public string? WorkingDirectory { get; set; }

        [JsonPropertyName(PropertyNames.Accepts)]
        public string?[]? Accepts { get; set; }
    }
}
