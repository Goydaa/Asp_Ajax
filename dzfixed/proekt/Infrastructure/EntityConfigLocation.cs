namespace WebApplication3.Infrastructure
{
    /// <summary>Путь к файлу entity-config.json (регистрируется в DI из Program.cs).</summary>
    public sealed record EntityConfigLocation(string Path);
}
