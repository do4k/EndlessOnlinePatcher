using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EoPatcher.Services;
using EoPatcher.Services.VersionFetchers;
using EoPatcher.Models;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace EoPatcher.Avalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _statusText = "Ready to patch.";

    [ObservableProperty]
    private bool _isBusy = false;

    [ObservableProperty]
    private bool _showLaunch;

    [ObservableProperty]
    private bool _showPatch;

    [ObservableProperty]
    private bool _showSkip;

    [ObservableProperty]
    private bool _isPatching;

    [ObservableProperty]
    private bool _isLinux;

    [ObservableProperty]
    private bool _isSettingsVisible;

    [ObservableProperty]
    private string _launchParameters = "";

    [ObservableProperty]
    private bool _isError;

    private readonly SettingsService _settingsService;

    private Version? _remoteVersion;

    public MainWindowViewModel()
    {
        _isLinux = OperatingSystem.IsLinux();
        _settingsService = new SettingsService();
        var settings = _settingsService.Load();
        LaunchParameters = settings.LaunchParameters;

        // Start checking versions immediately
        Task.Run(CheckVersions);
    }

    private async Task CheckVersions()
    {
        IsBusy = true;
        StatusText = "Getting local version...";
        
        var localRepo = new LocalVersionRepository();
        var localVersion = localRepo.Get();

        StatusText = "Getting remote version...";
        var serverVersionFetcher = new ServerVersionFetcher();
        var remoteVersionResult = await Task.Run(() => serverVersionFetcher.Get());

        IsBusy = false;

        if (remoteVersionResult.IsT1)
        {
            UpdateStatus($"Error: {remoteVersionResult.AsT1.Value}");
            ShowLaunch = true; // Allow launch on error?
            return;
        }

        _remoteVersion = remoteVersionResult.AsT0;

        if (localVersion < _remoteVersion)
        {
            UpdateStatus($"New version available (v{localVersion} -> v{_remoteVersion})");
            ShowPatch = true;
            ShowSkip = true;
            ShowLaunch = false;
        }
        else
        {
            UpdateStatus($"Up to date (v{localVersion})");
            ShowLaunch = true;
            ShowPatch = false;
            ShowSkip = false;
        }
    }

    [RelayCommand]
    private void Logout()
    {
        Environment.Exit(0);
    }

    [RelayCommand]
    private void ToggleSettings()
    {
        IsSettingsVisible = !IsSettingsVisible;
        if (!IsSettingsVisible)
        {
            IsError = false;
            _settingsService.Save(new Models.AppSettings { LaunchParameters = LaunchParameters });
        }
    }

    [RelayCommand]
    private async Task Launch()
    {
        StatusText = "Launching...";
        
        if (IsLinux)
        {
            try 
            {
                var exePath = EndlessOnlineDirectory.GetExe();
                var startInfo = new ProcessStartInfo();

                if (!string.IsNullOrWhiteSpace(LaunchParameters))
                {
                    // If wrapper (like proton/wine) is specified
                    // e.g. LaunchParameters="proton run" or "wine"
                    // Command becomes: proton run "path/to/Endless.exe"
                    
                    var parts = LaunchParameters.Split(' ', 2);
                    startInfo.FileName = parts[0];
                    if (parts.Length > 1)
                    {
                        startInfo.Arguments = $"{parts[1]} \"{exePath}\"";
                    }
                    else
                    {
                        startInfo.Arguments = $"\"{exePath}\"";
                    }
                }
                else
                {
                    // Try running directly (maybe via wine is implicit or it's a linux native build?)
                    // Assuming wine is needed if no wrapper specified, or just try executing.
                    // If it's a windows exe on linux, it needs wine.
                    // But let's assume if parameters are empty, we just run the exe (maybe users have binfmt_misc set up)
                    startInfo.FileName = exePath;
                }
                
                startInfo.UseShellExecute = false;
                startInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
                
                Process.Start(startInfo);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                 StatusText = $"Launch failed: {ex.Message}";
                 IsError = true;
                 // Show settings to allow fixing path
                 IsSettingsVisible = true;
            }
        }
        else
        {
            await EoPatcher.Interop.Windows.StartEO();
            Environment.Exit(0);
        }
    }

    [RelayCommand]
    private void Exit()
    {
        Environment.Exit(0);
    }

    [RelayCommand]
    private async Task Patch()
    {
        if (IsPatching || _remoteVersion == null) return;

        IsPatching = true;
        ShowSkip = false; // Hide skip while patching
        // Note: We keep ShowPatch = true so we can see the "Patching..." image
        
        Action<string> statusCallback = (msg) => UpdateStatus(msg);

        using var orchestrator = new PatchOrchestrator(statusCallback);
        var patchResult = await orchestrator.Patch(_remoteVersion);

        IsPatching = false;
        ShowPatch = false;

        if (patchResult.IsT0)
        {
            UpdateStatus($"Updated to {_remoteVersion}!");
            ShowLaunch = true;
        }
        else
        {
            UpdateStatus($"Patch failed: {patchResult.AsT1.Value}");
            ShowLaunch = true; // Allow launch even if patch failed? Or show patch again?
            // Original logic shows launch on error? 
            // "pbxLaunch.Visible = true" in switch e case.
        }
    }

    private void UpdateStatus(string message)
    {
        StatusText = message;
    }
}
