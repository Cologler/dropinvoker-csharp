﻿namespace RLauncher
{
    public interface ILauncherData
    {
        string? Name { get; }

        string? Description { get; }

        string? Runner { get; }

        string? Template { get; }

        string?[]? Arguments { get; }

        string? WorkingDirectory { get; }

        string?[]? Accepts { get; }
    }
}
