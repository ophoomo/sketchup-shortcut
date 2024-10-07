namespace SketchupShortcut.Shared.Models;

public class Sketchup
{
    public string Name { get; init; }
    public string NameVersion { get; init; }
    public string Path { get; init; }
    public string Version { get; init; }
    public string ExtensionPath { get; init; }
    public List<Extension> Extensions { get; init; }
}
