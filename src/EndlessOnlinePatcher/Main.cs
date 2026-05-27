using EndlessOnlinePatcher.Services;

using System.Media;
using System.Reflection;

namespace EndlessOnlinePatcher;

public partial class Main : Form
{
    private bool _patching = false;
    private bool _dragging;
    private Point _mouseDownLocation;
    private Version? _serverVersion;
    private readonly SoundPlayer _sndClickDown = new(Properties.Resources.click_down);
    private readonly SoundPlayer _sndClickUp = new(Properties.Resources.click_up);

    private int _dotCount;
    private int _animPercent;
    private string _animVerb = "Downloading";
    private readonly System.Windows.Forms.Timer _animTimer = new() { Interval = 400 };

    private readonly ILocalVersionRepository _localVersionRepository = new LocalVersionRepository();
    private readonly IServerVersionFetcher _serverVersionFetcher = new ServerVersionFetcher();

    public Main()
    {
        InitializeComponent();
        _animTimer.Tick += (_, _) =>
        {
            _dotCount = (_dotCount % 3) + 1;
            lblMessage.Text = $"{_animVerb}{new string('.', _dotCount)} {_animPercent}%";
        };
    }

    private async void Main_Shown(object sender, EventArgs e)
    {
        string version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "0.0.0.0";
        lblTitle.Text = $"Endless Online Patcher v{version}";

        SetPatchText("Getting local version...");
        var localVersion = _localVersionRepository.Get();
        SetPatchText("Getting remote version...");

        var result = await Task.Run(() => _serverVersionFetcher.Get());

        result.Switch(
            v =>
            {
                _serverVersion = v;
                if (localVersion == v)
                {
                    SetPatchText($"You are already up to date with the latest version v{localVersion}");
                    btnLaunch.Visible = true;
                    btnLaunch.Focus();
                }
                else
                {
                    SetPatchText($"A new version of the client is available{Environment.NewLine}(v{localVersion} -> v{_serverVersion})");
                    btnPatch.Visible = true;
                    btnSkip.Visible = true;
                    btnPatch.Focus();
                }
                btnExit.Visible = true;
            },
            err =>
            {
                SetPatchText(err.Value);
                btnExit.Visible = true;
                btnLaunch.Visible = true;
                btnLaunch.Focus();
            });
    }

    // --- Keyboard / gamepad navigation ---

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Escape:
                Close();
                return true;
            case Keys.Enter:
                (ActiveControl as Button)?.PerformClick();
                return true;
            case Keys.Up:
            case Keys.Left:
                MoveFocus(-1);
                return true;
            case Keys.Down:
            case Keys.Right:
                MoveFocus(1);
                return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    // Kept for KeyPreview wiring in the designer (ProcessCmdKey handles the real work).
    private void Main_KeyDown(object sender, KeyEventArgs e) { }

    private void MoveFocus(int direction)
    {
        // Tab order mirrors the visual layout: Exit → Skip → Patch/Launch
        var focusable = new Control[] { btnExit, btnSkip, btnPatch, btnLaunch }
            .Where(b => b.Visible && b.Enabled)
            .ToList();

        if (focusable.Count == 0) return;

        var currentIdx = focusable.FindIndex(b => b.Focused);
        var startIdx = currentIdx < 0 ? 0 : currentIdx;
        var nextIdx = (startIdx + direction + focusable.Count) % focusable.Count;
        focusable[nextIdx].Focus();
    }

    // --- Window dragging (borderless form) ---

    private void Main_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _dragging = true;
            _mouseDownLocation = ((Control)sender).PointToScreen(new Point(e.X, e.Y));
        }
    }

    private void Main_MouseUp(object sender, MouseEventArgs e) => _dragging = false;

    private void Main_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_dragging) return;
        var screen = ((Control)sender).PointToScreen(new Point(e.X, e.Y));
        Location = new Point(Location.X + screen.X - _mouseDownLocation.X, Location.Y + screen.Y - _mouseDownLocation.Y);
        _mouseDownLocation = screen;
    }

    // --- Logout (×) button in corner ---

    private void pbxLogout_MouseEnter(object sender, EventArgs e) => pbxLogout.Image = Properties.Resources.eo_logout_hover;
    private void pbxLogout_MouseLeave(object sender, EventArgs e) => pbxLogout.Image = Properties.Resources.eo_logout;
    private void pbxLogout_Click(object sender, EventArgs e) => Close();
    private void pbxLogout_MouseDown(object sender, MouseEventArgs e) => _sndClickDown.Play();
    private void pbxLogout_MouseUp(object sender, MouseEventArgs e) => _sndClickUp.Play();

    // --- Launch / Skip ---

    private async void btnLaunch_Click(object sender, EventArgs e)
    {
        await Interop.Windows.StartEO();
        Close();
    }

    private void btnLaunch_MouseDown(object sender, MouseEventArgs e) => _sndClickDown.Play();
    private void btnLaunch_MouseUp(object sender, MouseEventArgs e) => _sndClickUp.Play();

    // --- Exit ---

    private void btnExit_Click(object sender, EventArgs e) => Close();
    private void btnExit_MouseDown(object sender, MouseEventArgs e) => _sndClickDown.Play();
    private void btnExit_MouseUp(object sender, MouseEventArgs e) => _sndClickUp.Play();

    // --- Skip ---

    private void btnSkip_MouseDown(object sender, MouseEventArgs e) => _sndClickDown.Play();
    private void btnSkip_MouseUp(object sender, MouseEventArgs e) => _sndClickUp.Play();

    // --- Patch ---

    private async void btnPatch_Click(object sender, EventArgs e)
    {
        if (_patching || _serverVersion is null) return;

        _patching = true;
        btnPatch.Locked = true;
        btnSkip.Visible = false;
        btnPatch.BackgroundImage = Properties.Resources.eo_patching;
        _animPercent = 0;
        _animVerb = "Downloading";
        _dotCount = 0;
        _animTimer.Start();

        try
        {
            using var patcher = new PatchOrchestrator(SetPatchText);
            var result = await patcher.Patch(_serverVersion);
            if (result.IsT1)
                SetPatchText(result.AsT1.Value);
        }
        finally
        {
            _animTimer.Stop();
            _patching = false;
            btnPatch.Locked = false;
            btnPatch.BackgroundImage = Properties.Resources.eo_patch;
            btnPatch.Visible = false;
            btnLaunch.Visible = true;
            btnLaunch.Focus();
        }
    }

    private void btnPatch_MouseDown(object sender, MouseEventArgs e)
    {
        if (_patching) return;
        _sndClickDown.Play();
    }

    private void btnPatch_MouseUp(object sender, MouseEventArgs e)
    {
        if (_patching) return;
        _sndClickUp.Play();
    }

    // --- Status label / progress bar ---

    private void SetPatchText(string text)
    {
        if (lblMessage.InvokeRequired)
        {
            Invoke(new Action<string>(SetPatchText), text);
            return;
        }

        lblMessageHover.SetToolTip(lblMessage, text);

        var percentIdx = text.IndexOf('%');
        if (percentIdx > 0)
        {
            var spaceIdx = text.LastIndexOf(' ', percentIdx - 1);
            if (spaceIdx >= 0 && int.TryParse(text[(spaceIdx + 1)..percentIdx], out var percent))
            {
                _animPercent = percent;
                _animVerb = text[..spaceIdx];
                return;
            }
        }

        lblMessage.Text = text;
    }
}
