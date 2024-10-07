using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SketchupShortcut.Shared.Models;

namespace SketchupShortcut.Extension.Views;

public partial class MainWindow : Window
{
    private readonly List<Sketchup> _sketchups = [];

    public MainWindow()
    {
        InitializeComponent();
    }

    public void AddSketchup(object sender, RoutedEventArgs args)
    {
        var btnInstall = this.Find<Button>("Install");
        if (btnInstall == null) return;

        var button = sender as Button;
        if (sender is not Button { CommandParameter: Sketchup sketchup }) return;
        var check = _sketchups.FirstOrDefault(x => x.Name == sketchup.Name);
        if (check is not null)
        {
            _sketchups.Remove(sketchup);
            button?.Classes.Remove("Install");
            button?.Classes.Add("SketchupVersion");
        }
        else
        {
            _sketchups.Add(sketchup);
            button?.Classes.Add("Install");
            button?.Classes.Remove("SketchupVersion");
        }

        btnInstall.IsEnabled = _sketchups.Count != 0;
    }

    public async void ClickInstall(object sender, RoutedEventArgs args)
    {
        var commandLineArgs = Environment.GetCommandLineArgs();
        if (commandLineArgs.Length <= 1) return;
        var pathRbz = commandLineArgs[1];
        if (!File.Exists(pathRbz)) return;
        foreach (var sketchup in _sketchups) ZipFile.ExtractToDirectory(pathRbz, sketchup.ExtensionPath);
        var msBox = MessageBoxManager
            .GetMessageBoxStandard("Extension Shortcut", "Installed Extension Success");
        var result = await msBox.ShowAsPopupAsync(this);
        if (result == ButtonResult.Ok) Close();
    }

    private void ClickUnInstall(object? sender, RoutedEventArgs e)
    {
        var commandLineArgs = Environment.GetCommandLineArgs();
        if (commandLineArgs.Length <= 1) return;
        var pathRbz = commandLineArgs[1];
        if (!File.Exists(pathRbz)) return;
        foreach (var sketchup in _sketchups)
        {
            var archive = ZipFile.OpenRead(pathRbz);
            foreach (var entry in archive.Entries)
            {
                var pathFile = Path.Combine(sketchup.ExtensionPath, entry.FullName);
                if (File.Exists(pathFile)) File.Delete(pathFile);
                else if (Directory.Exists(pathFile)) Directory.Delete(pathFile, true);
            }
        }

        MessageBoxManager
            .GetMessageBoxStandard("Extension Shortcut", "Remove Extension Success").ShowAsPopupAsync(this);
    }
}
