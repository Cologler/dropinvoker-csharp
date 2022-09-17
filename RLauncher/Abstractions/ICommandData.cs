namespace RLauncher.Abstractions
{
    public interface ICommandData
    {
        string? Name { get; }

        string? Description { get; }

        string? Runner { get; }

        string?[]? Arguments { get; }

        string? WorkingDirectory { get; }

        string?[]? Accepts { get; }
    }
}
