using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RLauncher.Exceptions;

public class InvalidRLauncherConfigurationFileException : Exception
{
    public InvalidRLauncherConfigurationFileException()
    {
    }

    public InvalidRLauncherConfigurationFileException(string? message) : base(message)
    {
    }

    public InvalidRLauncherConfigurationFileException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidRLauncherConfigurationFileException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ConfigurationFileType FileType { get; set; }

    public string? FileFullPath { get; set; }

    public string? FileContent { get; set; }
}

