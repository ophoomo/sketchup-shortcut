using System.Collections.Generic;
using SketchupShortcut.Shared.Models;
using SketchupShortcut.Shared.Services;

namespace SketchupShortcut.Extension.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        sketchups = SketchupService.GetSketchup();
    }

    public List<Sketchup> sketchups { get; set; } = [];
}
