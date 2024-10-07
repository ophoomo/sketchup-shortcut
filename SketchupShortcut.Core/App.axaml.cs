using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;
using SketchupShortcut.Core.ViewModels;
using SketchupShortcut.Core.Views;
using SketchupShortcut.Shared.Models;
using SketchupShortcut.Shared.Services;

namespace SketchupShortcut.Core;

public class App : Application
{
    private MainWindow? _mainWindow;
    private List<Sketchup> _sketchups = [];

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            await InitMainViewModelAsync();
            desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
            RegisterTrayIcon();
        }

#if DEBUG
        if (_mainWindow == null)
            _mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
        _mainWindow.WindowState = WindowState.Normal;
        _mainWindow.Show();
#endif

        base.OnFrameworkInitializationCompleted();
    }

    // InitMainViewModelAsync is method load data from disk
    private async Task InitMainViewModelAsync()
    {
        _sketchups = SketchupService.GetSketchup();
    }

    // RegisterTrayIcon is method control tray system
    private void RegisterTrayIcon()
    {
        var listMenu = new NativeMenu();

        foreach (var sketchup in _sketchups)
            listMenu.Items.Add(new NativeMenuItem
            {
                Header = sketchup.Name,
                Command = ReactiveCommand.Create(() => SketchupService.OpenSketchup(sketchup))
            });

        if (_sketchups.Count > 0) listMenu.Items.Add(new NativeMenuItemSeparator());
        listMenu.Items.Add(new NativeMenuItem
        {
            Header = "Exit",
            Command = ReactiveCommand.Create(ExitApplication)
        });

        var trayIcon = new TrayIcon
        {
            IsVisible = true,
            ToolTipText = "Sketchup Shortcut",
            Icon = new WindowIcon(
                new Bitmap(AssetLoader.Open(new Uri("avares://SketchupShortcut/Assets/app.ico")))),
            Command = ReactiveCommand.Create(ShowApplication),
            Menu = listMenu
        };
    }

    // ShowApplication is method check state and show application
    private void ShowApplication()
    {
        if (_mainWindow == null)
            _mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
        _mainWindow.WindowState = WindowState.Normal;
        // _mainWindow.Show();
    }

    // ExitApplication is method exit program
    private void ExitApplication()
    {
        Environment.Exit(0);
    }
}
