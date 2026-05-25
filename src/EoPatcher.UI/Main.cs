using EoPatcher.Services;
using EoPatcher.Services.VersionFetchers;
using System.Media;
using System.Reflection;

namespace EoPatcher.UI;

public partial class Main : Form
{
    private bool _patching = false;
    private bool _dragging;
    private Point _mouseDownLocation;
    private Version _serverVersion;
    private readonly SoundPlayer _sndClickDown = new(Properties.Resources.click_down);
    private readonly SoundPlayer _sndClickUp = new(Properties.Resources.click_up);

    private readonly ILocalVersionRepository _localVersionRepository = new LocalVersionRepository();
    private readonly IServerVersionFetcher _serverVersionFetcher = new ServerVersionFetcher();

    public Main()
    {
        InitializeComponent();
    }

    private async void Main_Shown(object sender, EventArgs e)
    {
        string version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "0.0.0.0";
        lblTitle.Text = $"Endless Online Patcher v{version}";

        SetPatchText("Getting local version...");
        var localVersion = _localVersionRepository.Get();
        SetPatchText("Getting remote version...");

        var result = await Task.Run(() => _serverVersionFetcher.Get());

        result.Switch(v =>
            {
                _serverVersion = v;
                if (localVersion == v)
                {
                    SetPatchText($"You are already up to date with the latest version v{localVersion}");
                    pbxLaunch.Visible = true;
                }
                else
                {
                    SetPatchText($"A new version of the client is available{Environment.NewLine}(v{localVersion} -> v{_serverVersion})");
                    pbxPatch.Visible = true;
                    pbxSkip.Visible = true;
                }
                pbxExit.Visible = true;
            },
            err =>
            {
                SetPatchText(err.Value);
                pbxExit.Visible = true;
                pbxPatch.Visible = false;
                pbxSkip.Visible = false;
                pbxLaunch.Visible = true;
            });
    }

    private void Main_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _dragging = true;
            _mouseDownLocation = new Point(e.X, e.Y);
        }
    }

    private void Main_MouseUp(object sender, MouseEventArgs e)
    {
        _dragging = false;
    }

    private void Main_MouseMove(object sender, MouseEventArgs e)
    {
        if (_dragging)
        {
            Location = new Point(Location.X + e.X - _mouseDownLocation.X, Location.Y + e.Y - _mouseDownLocation.Y);
        }
    }

    private void pbxLogout_MouseEnter(object sender, EventArgs e)
    {
        pbxLogout.Image = Properties.Resources.eo_logout_hover;
    }

    private void pbxLogout_MouseLeave(object sender, EventArgs e)
    {
        pbxLogout.Image = Properties.Resources.eo_logout;
    }

    private void pbxLogout_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void pbxPatch_MouseEnter(object sender, EventArgs e)
    {
        if (_patching) return;
        pbxPatch.Image = Properties.Resources.eo_patch_hover;
    }

    private void pbxPatch_MouseLeave(object sender, EventArgs e)
    {
        if (_patching) return;
        pbxPatch.Image = Properties.Resources.eo_patch;
    }

    private void pbxLaunch_MouseEnter(object sender, EventArgs e)
    {
        pbxLaunch.Image = Properties.Resources.eo_launch_hover;
    }

    private void pbxLaunch_MouseLeave(object sender, EventArgs e)
    {
        pbxLaunch.Image = Properties.Resources.eo_launch;
    }

    private async void pbxLaunch_Click(object sender, EventArgs e)
    {
        await Interop.Windows.StartEO();
        Close();
    }

    private void pbxExit_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void pbxExit_MouseEnter(object sender, EventArgs e)
    {
        pbxExit.Image = Properties.Resources.eo_exit_hover;
    }

    private void pbxExit_MouseLeave(object sender, EventArgs e)
    {
        pbxExit.Image = Properties.Resources.eo_exit;
    }

    private void pbxSkip_MouseEnter(object sender, EventArgs e)
    {
        pbxSkip.Image = Properties.Resources.skip_hover;
    }

    private void pbxSkip_MouseLeave(object sender, EventArgs e)
    {
        pbxSkip.Image = Properties.Resources.skip;
    }

    delegate void SetPatchTextCallback(string text);
    private void SetPatchText(string text)
    {
        if (lblMessage.InvokeRequired)
        {

            SetPatchTextCallback d = new SetPatchTextCallback(SetPatchText);
            Invoke(d, new object[] { text });
            return;
        }

        lblMessage.Text = text;
        lblMessageHover.SetToolTip(lblMessage, text);
    }

    private async void pbxPatch_MouseClick(object sender, MouseEventArgs e)
    {
        if (_patching) return;

        _patching = true;
        pbxSkip.Visible = false;
        pbxPatch.Image = Properties.Resources.eo_patching;

        using var patcher = new PatchOrchestrator(SetPatchText);

        var result = await patcher.Patch(_serverVersion);
        if (result.IsT1)
            SetPatchText(result.AsT1.Value);

        _patching = false;
        pbxPatch.Visible = false;
        pbxLaunch.Visible = true;
    }

    private void pbxLaunch_MouseDown(object sender, MouseEventArgs e)
    {
        _sndClickDown.Play();
    }

    private void pbxExit_MouseDown(object sender, MouseEventArgs e)
    {
        _sndClickDown.Play();
    }

    private void pbxPatch_MouseDown(object sender, MouseEventArgs e)
    {
        if (_patching) return;
        _sndClickDown.Play();
    }

    private void pbxPatch_MouseUp(object sender, MouseEventArgs e)
    {
        if (_patching) return;
        _sndClickUp.Play();
    }

    private void pbxLaunch_MouseUp(object sender, MouseEventArgs e)
    {
        _sndClickUp.Play();
    }

    private void pbxExit_MouseUp(object sender, MouseEventArgs e)
    {
        _sndClickUp.Play();
    }

    private void pbxLogout_MouseDown(object sender, MouseEventArgs e)
    {
        _sndClickDown.Play();
    }

    private void pbxLogout_MouseUp(object sender, MouseEventArgs e)
    {
        _sndClickUp.Play();
    }

    private void pbxSkip_MouseDown(object sender, MouseEventArgs e)
    {
        _sndClickDown.Play();
    }

    private void pbxSkip_MouseUp(object sender, MouseEventArgs e)
    {
        _sndClickUp.Play();
    }
}
