using EndlessOnlinePatcher.Models;
using System.Media;

namespace EndlessOnlinePatcher;

internal sealed class ConfigForm : Form
{
    private readonly SoundPlayer _sndClickDown;
    private readonly SoundPlayer _sndClickUp;
    private bool _dragging;
    private Point _mouseDownLocation;

    private TextBox _txtHost = null!;
    private NumericUpDown _nudPort = null!;
    private NumericUpDown _nudMusic = null!;
    private CheckBox _chkSound = null!;
    private CheckBox _chkSfx = null!;
    private CheckBox _chkWasd = null!;

    public ConfigForm(SoundPlayer clickDown, SoundPlayer clickUp)
    {
        _sndClickDown = clickDown;
        _sndClickUp = clickUp;
        BuildForm();
    }

    private void BuildForm()
    {
        var settings = EoConfigReader.ReadSettings();

        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.Black;
        ClientSize = new Size(1280, 720);
        FormBorderStyle = FormBorderStyle.None;
        KeyPreview = true;
        MaximumSize = ClientSize;
        MinimumSize = ClientSize;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Endless Online Patcher — Settings";

        MouseDown += Drag_MouseDown;
        MouseMove += Drag_MouseMove;
        MouseUp += Drag_MouseUp;

        // ── Panel ──────────────────────────────────────────────────────────
        var panel = new Panel
        {
            BackgroundImage = Properties.Resources.eo_popup,
            BackgroundImageLayout = ImageLayout.Stretch,
            Location = new Point(240, 110),
            Size = new Size(800, 500),
        };
        panel.MouseDown += Drag_MouseDown;
        panel.MouseMove += Drag_MouseMove;
        panel.MouseUp += Drag_MouseUp;
        Controls.Add(panel);

        // × close
        var pbxClose = new PictureBox
        {
            BackColor = Color.Transparent,
            Image = Properties.Resources.eo_logout,
            Location = new Point(762, 16),
            Size = new Size(33, 21),
            SizeMode = PictureBoxSizeMode.Zoom,
            TabStop = false,
        };
        pbxClose.MouseEnter += (_, _) => pbxClose.Image = Properties.Resources.eo_logout_hover;
        pbxClose.MouseLeave += (_, _) => pbxClose.Image = Properties.Resources.eo_logout;
        pbxClose.MouseDown += (_, _) => _sndClickDown.Play();
        pbxClose.MouseUp += (_, _) => _sndClickUp.Play();
        pbxClose.Click += (_, _) => Close();
        panel.Controls.Add(pbxClose);

        // Title
        var lblTitle = MakeLabel("Settings", 22, 20, 700, 38, 20F);
        lblTitle.MouseDown += Drag_MouseDown;
        lblTitle.MouseMove += Drag_MouseMove;
        lblTitle.MouseUp += Drag_MouseUp;
        panel.Controls.Add(lblTitle);

        // ── Connection ─────────────────────────────────────────────────────
        panel.Controls.Add(MakeSectionLabel("Connection", 22, 78));
        panel.Controls.Add(MakeLabel("Host:", 22, 122, autoSize: true));
        _txtHost = MakeTextBox(settings.Host, 100, 118, 470);
        panel.Controls.Add(_txtHost);
        panel.Controls.Add(MakeLabel("Port:", 590, 122, autoSize: true));
        _nudPort = MakeSpinner(settings.Port, 1, 65535, 645, 118, 110);
        panel.Controls.Add(_nudPort);

        // ── Sound ──────────────────────────────────────────────────────────
        panel.Controls.Add(MakeSectionLabel("Sound", 22, 173));
        panel.Controls.Add(MakeLabel("Music Volume (0–100):", 22, 218, autoSize: true));
        _nudMusic = MakeSpinner(settings.MusicVolume, 0, 100, 270, 214, 90);
        panel.Controls.Add(_nudMusic);
        _chkSound = MakeCheckBox("Sound", settings.SoundEnabled, 22, 265);
        _chkSfx = MakeCheckBox("SFX", settings.SfxEnabled, 160, 265);
        panel.Controls.Add(_chkSound);
        panel.Controls.Add(_chkSfx);

        // ── Controls ───────────────────────────────────────────────────────
        panel.Controls.Add(MakeSectionLabel("Controls", 22, 310));
        _chkWasd = MakeCheckBox("WASD Keys (disables typing WASD in chat)", settings.WasdKeys, 22, 355);
        panel.Controls.Add(_chkWasd);

        // ── Buttons ────────────────────────────────────────────────────────
        var btnCancel = MakeActionButton(Properties.Resources.eo_exit, Properties.Resources.eo_exit_hover, 33, 398);
        btnCancel.Click += (_, _) => Close();
        btnCancel.MouseDown += (_, _) => _sndClickDown.Play();
        btnCancel.MouseUp += (_, _) => _sndClickUp.Play();
        panel.Controls.Add(btnCancel);

        var btnSave = MakeActionButton(Properties.Resources.eo_ok, Properties.Resources.eo_ok_hover, 494, 398);
        btnSave.Click += (_, _) => Save();
        btnSave.MouseDown += (_, _) => _sndClickDown.Play();
        btnSave.MouseUp += (_, _) => _sndClickUp.Play();
        panel.Controls.Add(btnSave);
        btnSave.Focus();
    }

    private void Save()
    {
        if (string.IsNullOrWhiteSpace(_txtHost.Text))
        {
            MessageBox.Show("Host cannot be empty.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _txtHost.Focus();
            return;
        }

        try
        {
            EoConfigWriter.WriteSettings(new EoSettings
            {
                Host = _txtHost.Text.Trim(),
                Port = (int)_nudPort.Value,
                MusicVolume = (int)_nudMusic.Value,
                SoundEnabled = _chkSound.Checked,
                SfxEnabled = _chkSfx.Checked,
                WasdKeys = _chkWasd.Checked,
            });
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not save settings:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Escape) { Close(); return true; }
        if (keyData == Keys.Enter)  { Save();  return true; }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    // ── Drag ───────────────────────────────────────────────────────────────

    private void Drag_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _dragging = true;
            _mouseDownLocation = ((Control)sender!).PointToScreen(new Point(e.X, e.Y));
        }
    }

    private void Drag_MouseUp(object? sender, MouseEventArgs e) => _dragging = false;

    private void Drag_MouseMove(object? sender, MouseEventArgs e)
    {
        if (!_dragging) return;
        var screen = ((Control)sender!).PointToScreen(new Point(e.X, e.Y));
        Location = new Point(Location.X + screen.X - _mouseDownLocation.X, Location.Y + screen.Y - _mouseDownLocation.Y);
        _mouseDownLocation = screen;
    }

    // ── Factory helpers ────────────────────────────────────────────────────

    private static Label MakeLabel(string text, int x, int y, int width = 0, int height = 0, float fontSize = 13F, bool autoSize = false)
    {
        var lbl = new Label
        {
            Text = text,
            ForeColor = Color.LemonChiffon,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", fontSize),
            Location = new Point(x, y),
            AutoSize = autoSize,
        };
        if (!autoSize) lbl.Size = new Size(width, height);
        return lbl;
    }

    private static Label MakeSectionLabel(string text, int x, int y)
    {
        return new Label
        {
            Text = text,
            ForeColor = Color.Gold,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            Location = new Point(x, y),
            AutoSize = true,
        };
    }

    private static TextBox MakeTextBox(string value, int x, int y, int width)
    {
        return new TextBox
        {
            Text = value,
            BackColor = Color.FromArgb(25, 25, 35),
            ForeColor = Color.LemonChiffon,
            Font = new Font("Segoe UI", 13F),
            BorderStyle = BorderStyle.FixedSingle,
            Location = new Point(x, y),
            Size = new Size(width, 30),
        };
    }

    private static NumericUpDown MakeSpinner(int value, int min, int max, int x, int y, int width)
    {
        return new NumericUpDown
        {
            Value = value,
            Minimum = min,
            Maximum = max,
            BackColor = Color.FromArgb(25, 25, 35),
            ForeColor = Color.LemonChiffon,
            Font = new Font("Segoe UI", 13F),
            BorderStyle = BorderStyle.FixedSingle,
            Location = new Point(x, y),
            Size = new Size(width, 30),
        };
    }

    private static CheckBox MakeCheckBox(string text, bool isChecked, int x, int y)
    {
        return new CheckBox
        {
            Text = text,
            Checked = isChecked,
            ForeColor = Color.LemonChiffon,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 13F),
            Location = new Point(x, y),
            AutoSize = true,
        };
    }

    private static EoButton MakeActionButton(Image normal, Image hover, int x, int y)
    {
        return new EoButton
        {
            NormalImage = normal,
            HoverImage = hover,
            BackgroundImage = normal,
            Location = new Point(x, y),
            Size = new Size(273, 90),
            TabStop = true,
        };
    }
}
