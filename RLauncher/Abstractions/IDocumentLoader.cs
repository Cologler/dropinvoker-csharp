using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RLauncher.Exceptions;

namespace RLauncher.Abstractions;

public interface IDocumentLoader<T>
{
    public ValueTask<bool> CanLoadAsync(string extensionName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <exception cref="InvalidRLauncherConfigurationFileException"></exception>
    public ValueTask<T> LoadAsync(string filePath);
}
