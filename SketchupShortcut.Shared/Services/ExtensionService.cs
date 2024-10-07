using SketchupShortcut.Shared.Models;

namespace SketchupShortcut.Shared.Services;

public class ExtensionService
{
    public static List<Extension> GetExtensioList(Sketchup sketchup)
    {
        List<Extension> extensions = [];
        string[] filePaths = Directory.GetFiles(sketchup.ExtensionPath, "*.rb");
        foreach (string filePath in filePaths)
        {
            var rbName = Path.GetFileNameWithoutExtension(filePath);
            var folderPath = Path.Combine(sketchup.ExtensionPath, rbName);
            extensions.Add(new Extension
            {
                Name = rbName,
                RbPath = filePath,
                FolderPath = Directory.Exists(folderPath) ? folderPath : null,
            });
        }
        return extensions;
    }

    public static void RemoveExtension(Extension extension)
    {
        if (File.Exists(extension.RbPath)) File.Delete(extension.RbPath);
        if (extension.FolderPath != null && Directory.Exists(extension.FolderPath)) Directory.Delete(extension.FolderPath, true);
    }
}
