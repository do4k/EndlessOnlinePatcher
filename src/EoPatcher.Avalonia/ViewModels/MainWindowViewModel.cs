using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EoPatcher.Services;
using EoPatcher.Services.VersionFetchers;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EoPatcher.Avalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _statusText = "Ready to patch.";

    [ObservableProperty]
    private bool _isBusy = false;

    [RelayCommand]
    private async Task CheckAndPatch()
    {
        if (IsBusy) return;
        IsBusy = true;
        StatusText = "Checking for updates...";

        try
        {
            await Task.Run(async () =>
            {
                var serverVersionFetcher = new ServerVersionFetcher();
                var remoteVersionResult = serverVersionFetcher.Get(); 

                if (remoteVersionResult.IsT1)
                {
                    UpdateStatus($"Error fetching remote version: {remoteVersionResult.AsT1.Value}");
                    return;
                }

                var remoteVersion = remoteVersionResult.AsT0;
                UpdateStatus($"Remote version: {remoteVersion}");

                var localRepo = new LocalVersionRepository();
                var localVersion = localRepo.Get();
                UpdateStatus($"Local version: {localVersion}");

                if (localVersion < remoteVersion)
                {
                    UpdateStatus("Update found! Starting patch...");
                    
                    Action<string> statusCallback = (msg) => UpdateStatus(msg);

                    using var orchestrator = new PatchOrchestrator(statusCallback);
                    var patchResult = await orchestrator.Patch(remoteVersion);

                    if (patchResult.IsT0)
                    {
                        UpdateStatus($"Successfully updated to {remoteVersion}!");
                    }
                    else
                    {
                        UpdateStatus($"Patch failed: {patchResult.AsT1.Value}");
                    }
                }
                else
                {
                    UpdateStatus("Client is up to date.");
                }
            });
        }
        catch (Exception ex)
        {
            StatusText = $"An unexpected error occurred: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void UpdateStatus(string message)
    {
        StatusText = message;
    }
}
