﻿using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

using RLauncher.Abstractions;
using RLauncher.Exceptions;

using YamlDotNet.Serialization;

namespace RLauncher.Yaml;

internal class YamlDocumentLoader : IDocumentLoader<IRunnerData>, IDocumentLoader<ICommandData>
{
    public ValueTask<bool> CanLoadAsync(string extensionName) 
        => new(extensionName is { } && (extensionName.EndsWith(".yaml", StringComparison.OrdinalIgnoreCase) || extensionName.EndsWith(".yml", StringComparison.OrdinalIgnoreCase)));

    ValueTask<T> LoadAsync<T>(string filePath, ConfigurationFileType docType)
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
            doc = new Deserializer().Deserialize<T>(content);
        }
        catch (JsonException e)
        {
            throw new InvalidRLauncherConfigurationFileException("Unable parse from yaml", e)
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
        => await this.LoadAsync<RunnerData>(filePath, ConfigurationFileType.Runner).ConfigureAwait(false);

    async ValueTask<ICommandData> IDocumentLoader<ICommandData>.LoadAsync(string filePath)
        => await this.LoadAsync<CommandData>(filePath, ConfigurationFileType.Command).ConfigureAwait(false);

    record RunnerData : IRunnerData
    {
        [YamlMember(Alias = PropertyNames.Executable, ApplyNamingConventions = false)]
        public string? Executable { get; set; }

        [YamlMember(Alias = PropertyNames.Arguments, ApplyNamingConventions = false)]
        public string?[]? Arguments { get; set; }

        [YamlMember(Alias = PropertyNames.WorkingDirectory, ApplyNamingConventions = false)]
        public string? WorkingDirectory { get; set; }
    }

    record CommandData : ICommandData
    {
        [YamlMember(Alias = PropertyNames.Name, ApplyNamingConventions = false)]
        public string? Name { get; set; }

        [YamlMember(Alias = PropertyNames.Description, ApplyNamingConventions = false)]
        public string? Description { get; set; }

        [YamlMember(Alias = PropertyNames.Runner, ApplyNamingConventions = false)]
        public string? Runner { get; set; }

        [YamlMember(Alias = PropertyNames.Arguments, ApplyNamingConventions = false)]
        public string?[]? Arguments { get; set; }

        [YamlMember(Alias = PropertyNames.WorkingDirectory, ApplyNamingConventions = false)]
        public string? WorkingDirectory { get; set; }

        [YamlMember(Alias = PropertyNames.Accepts, ApplyNamingConventions = false)]
        public string?[]? Accepts { get; set; }
    }
}
