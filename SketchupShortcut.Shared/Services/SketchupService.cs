using System.Diagnostics;
using Microsoft.Win32;
using SketchupShortcut.Shared.Models;

namespace SketchupShortcut.Shared.Services;

public class SketchupService
{
    public static List<Sketchup> GetSketchup()
    {
        List<Sketchup> sketchups = new();

        var registryPaths = new[]
        {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
        };

        foreach (var registryPath in registryPaths)
        {
            using var key = Registry.LocalMachine.OpenSubKey(registryPath);
            if (key == null) continue;
            foreach (var subKeyName in key.GetSubKeyNames())
            {
                using var subKey = key.OpenSubKey(subKeyName);
                if (subKey == null) continue;
                var displayName = subKey.GetValue("DisplayName") as string;
                var installLocation = subKey.GetValue("InstallLocation") as string;
                if (string.IsNullOrEmpty(displayName) ||
                    !displayName.Contains("SketchUp", StringComparison.OrdinalIgnoreCase)) continue;
                var version = subKey.GetValue("DisplayVersion") as string;
                var nameVersion = displayName.Split(" ")[1];
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var extensionPath = Path.Combine(appDataPath, "SketchUp", $"SketchUp {nameVersion}", "SketchUp",
                    "Plugins");
                sketchups.Add(new Sketchup
                {
                    Name = displayName,
                    NameVersion = nameVersion,
                    Version = version ?? "Unknown",
                    Path = installLocation ?? $@"C:\Program Files\SketchUp\{displayName}\",
                    ExtensionPath = extensionPath
                });
            }
        }

        return sketchups;
    }

    public static void OpenSketchup(Sketchup sketchup)
    {
        var path = sketchup.Path;

        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine("Path is not available.");
            return;
        }

        var executablePath = Path.Combine(path, "SketchUp.exe");
        if (File.Exists(executablePath))
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                UseShellExecute = true
            };

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening SketchUp: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("SketchUp executable not found at the specified path.");
        }
    }
}
