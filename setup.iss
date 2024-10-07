[Setup]
AppName=SketchupShortcut
AppVersion=Alpha Test
DefaultDirName={pf}\SketchupShortcut
DefaultGroupName=SketchupShortcut
OutputDir=Output
OutputBaseFilename=setup
Compression=lzma
SolidCompression=yes
DisableWelcomePage=no

[Files]
Source: ".\SketchupShortcut.Core\bin\Release\net8.0\win-x64\publish\*"; DestDir: "{app}\SketchupShortcut"; Excludes: "*.pdb"; Flags: ignoreversion; Components: core;
Source: ".\SketchupShortcut.Extension\bin\Release\net8.0\win-x64\publish\*"; DestDir: "{app}\ExtensionShortcut"; Excludes: "*.pdb"; Flags: ignoreversion; Components: extension;
Source: "..\SketchupConvertVersion\SketchupConvertVersion\bin\Release\net8.0-windows\publish\win-x64\*"; DestDir: "{app}\SketchupConvertVersion"; Excludes: "*.pdb, *.config"; Flags: ignoreversion; Components: convertversion;

[Components]
Name: "core"; Description: "Install Sketchup Shortcut"; Types: full compact custom; Flags: fixed;
Name: "extension"; Description: "Install Extension Shortcut"; Types: full compact;
Name: "convertversion"; Description: "Install Sketchup Convert Version"; Types: full;

[Registry]
Root: HKCU; Subkey: "Software\Classes\.rbz"; ValueType: string; ValueData: "ExtensionShortcut.RBZ"; Flags: uninsdeletevalue; Components: extension;
Root: HKCU; Subkey: "Software\Classes\ExtensionShortcut.RBZ"; ValueType: string; ValueData: "RBZ File"; Flags: uninsdeletekey; Components: extension;
Root: HKCU; Subkey: "Software\Classes\ExtensionShortcut.RBZ\shell\open\command"; ValueType: string; ValueData: """{app}\ExtensionShortcut\ExtensionShortcut.exe"" ""%1"""; Flags: uninsdeletekey; Components: extension;
Root: HKCU; Subkey: "Software\Classes\ExtensionShortcut.RBZ\shell\open"; ValueType: string; ValueData: "Open with ExtensionShortcut"; Flags: uninsdeletekey; Components: extension;
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "SketchupShortcut"; ValueData: "{app}\SketchupShortcut\SketchupShortcut.exe"; Flags: uninsdeletevalue; Components: core;

[Run]
Filename: "{app}\SketchupShortcut\SketchupShortcut.exe"; Description: "Run Sketchup Shortcut"; Flags: nowait postinstall skipifsilent

[Icons]
Name: "{group}\SketchupShortcut"; Filename: "{app}\SketchupShortcut\SketchupShortcutexe"
Name: "{userdesktop}\SketchupShortcut"; Filename: "{app}\SketchupShortcut\SketchupShortcut.exe";
Name: "{group}\SketchupConvertVersion"; Filename: "{app}\SketchupConvertVersion\SketchupConvertVersion.exe"; Components: convertversion
Name: "{userdesktop}\SketchupConvertVersion"; Filename: "{app}\SketchupConvertVersion\SketchupConvertVersion.exe"; Components: convertversion
