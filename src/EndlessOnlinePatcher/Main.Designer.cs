namespace EndlessOnlinePatcher;

partial class Main
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
        pnlContent = new Panel();
        lblTitle = new Label();
        lblMessage = new Label();
        pbxLogout = new PictureBox();
        btnPatch = new EoButton();
        btnLaunch = new EoButton();
        btnExit = new EoButton();
        btnSkip = new EoButton();
        prgPatch = new ProgressBar();
        lblMessageHover = new ToolTip(components);
        pnlContent.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pbxLogout).BeginInit();
        SuspendLayout();

        //
        // pnlContent — centred EO popup panel on the black form.
        // NOTE: eo-popup.png is 290x125 source art and will stretch here.
        // A larger background asset would be needed for a production build.
        //
        pnlContent.BackgroundImage = Properties.Resources.eo_popup;
        pnlContent.BackgroundImageLayout = ImageLayout.Stretch;
        pnlContent.Controls.Add(lblTitle);
        pnlContent.Controls.Add(lblMessage);
        pnlContent.Controls.Add(pbxLogout);
        pnlContent.Controls.Add(btnPatch);
        pnlContent.Controls.Add(btnLaunch);
        pnlContent.Controls.Add(btnExit);
        pnlContent.Controls.Add(btnSkip);
        pnlContent.Controls.Add(prgPatch);
        pnlContent.Location = new Point(240, 110);
        pnlContent.Name = "pnlContent";
        pnlContent.Size = new Size(800, 500);
        pnlContent.MouseDown += Main_MouseDown;
        pnlContent.MouseMove += Main_MouseMove;
        pnlContent.MouseUp += Main_MouseUp;

        //
        // lblTitle
        //
        lblTitle.AutoSize = false;
        lblTitle.BackColor = Color.Transparent;
        lblTitle.ForeColor = Color.LemonChiffon;
        lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Regular);
        lblTitle.Location = new Point(22, 20);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(700, 38);
        lblTitle.TabIndex = 0;
        lblTitle.TabStop = false;
        lblTitle.Text = "Endless Online Patcher";
        lblTitle.MouseDown += Main_MouseDown;
        lblTitle.MouseMove += Main_MouseMove;
        lblTitle.MouseUp += Main_MouseUp;

        //
        // lblMessage
        //
        lblMessage.BackColor = Color.Transparent;
        lblMessage.ForeColor = Color.LemonChiffon;
        lblMessage.Font = new Font("Segoe UI", 16F);
        lblMessage.Location = new Point(22, 75);
        lblMessage.Name = "lblMessage";
        lblMessage.Size = new Size(756, 210);
        lblMessage.TabIndex = 0;
        lblMessage.TabStop = false;
        lblMessage.Text = "Loading...";
        lblMessage.MouseDown += Main_MouseDown;
        lblMessage.MouseMove += Main_MouseMove;
        lblMessage.MouseUp += Main_MouseUp;

        //
        // pbxLogout (top-right close button — PictureBox, not in tab order;
        //            Escape key is the gamepad close action)
        //
        pbxLogout.BackColor = Color.Transparent;
        pbxLogout.Image = Properties.Resources.eo_logout;
        pbxLogout.Location = new Point(762, 16);
        pbxLogout.Name = "pbxLogout";
        pbxLogout.Size = new Size(33, 21);
        pbxLogout.SizeMode = PictureBoxSizeMode.Zoom;
        pbxLogout.TabIndex = 0;
        pbxLogout.TabStop = false;
        pbxLogout.Click += pbxLogout_Click;
        pbxLogout.MouseDown += pbxLogout_MouseDown;
        pbxLogout.MouseEnter += pbxLogout_MouseEnter;
        pbxLogout.MouseLeave += pbxLogout_MouseLeave;
        pbxLogout.MouseUp += pbxLogout_MouseUp;

        //
        // prgPatch — visual download/extract progress bar
        //
        prgPatch.Location = new Point(22, 298);
        prgPatch.Name = "prgPatch";
        prgPatch.Size = new Size(756, 26);
        prgPatch.Style = ProgressBarStyle.Continuous;
        prgPatch.TabIndex = 0;
        prgPatch.TabStop = false;
        prgPatch.Visible = false;

        //
        // btnExit — TabIndex 1, first in focus order
        //
        btnExit.NormalImage = Properties.Resources.eo_exit;
        btnExit.HoverImage = Properties.Resources.eo_exit_hover;
        btnExit.BackgroundImage = Properties.Resources.eo_exit;
        btnExit.Location = new Point(33, 398);
        btnExit.Name = "btnExit";
        btnExit.Size = new Size(273, 90);
        btnExit.TabIndex = 1;
        btnExit.TabStop = true;
        btnExit.Visible = false;
        btnExit.Click += btnExit_Click;
        btnExit.MouseDown += btnExit_MouseDown;
        btnExit.MouseUp += btnExit_MouseUp;

        //
        // btnSkip — TabIndex 2
        //
        btnSkip.NormalImage = Properties.Resources.skip;
        btnSkip.HoverImage = Properties.Resources.skip_hover;
        btnSkip.BackgroundImage = Properties.Resources.skip;
        btnSkip.Location = new Point(494, 300);
        btnSkip.Name = "btnSkip";
        btnSkip.Size = new Size(273, 90);
        btnSkip.TabIndex = 2;
        btnSkip.TabStop = true;
        btnSkip.Visible = false;
        btnSkip.Click += btnLaunch_Click;
        btnSkip.MouseDown += btnSkip_MouseDown;
        btnSkip.MouseUp += btnSkip_MouseUp;

        //
        // btnPatch — TabIndex 3 (same position as btnLaunch, mutually exclusive)
        //
        btnPatch.NormalImage = Properties.Resources.eo_patch;
        btnPatch.HoverImage = Properties.Resources.eo_patch_hover;
        btnPatch.BackgroundImage = Properties.Resources.eo_patch;
        btnPatch.Location = new Point(494, 398);
        btnPatch.Name = "btnPatch";
        btnPatch.Size = new Size(273, 90);
        btnPatch.TabIndex = 3;
        btnPatch.TabStop = true;
        btnPatch.Visible = false;
        btnPatch.Click += btnPatch_Click;
        btnPatch.MouseDown += btnPatch_MouseDown;
        btnPatch.MouseUp += btnPatch_MouseUp;

        //
        // btnLaunch — TabIndex 3 (same position as btnPatch, mutually exclusive)
        //
        btnLaunch.NormalImage = Properties.Resources.eo_launch;
        btnLaunch.HoverImage = Properties.Resources.eo_launch_hover;
        btnLaunch.BackgroundImage = Properties.Resources.eo_launch;
        btnLaunch.Location = new Point(494, 398);
        btnLaunch.Name = "btnLaunch";
        btnLaunch.Size = new Size(273, 90);
        btnLaunch.TabIndex = 3;
        btnLaunch.TabStop = true;
        btnLaunch.Visible = false;
        btnLaunch.Click += btnLaunch_Click;
        btnLaunch.MouseDown += btnLaunch_MouseDown;
        btnLaunch.MouseUp += btnLaunch_MouseUp;

        //
        // Main form — 1280×720 (16:9), black surround, borderless
        //
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.Black;
        ClientSize = new Size(1280, 720);
        Controls.Add(pnlContent);
        FormBorderStyle = FormBorderStyle.None;
        Icon = (Icon)resources.GetObject("$this.Icon");
        KeyPreview = true;
        MaximumSize = new Size(1280, 720);
        MinimumSize = new Size(1280, 720);
        Name = "Main";
        SizeGripStyle = SizeGripStyle.Hide;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Endless Online Patcher";
        KeyDown += Main_KeyDown;
        Shown += Main_Shown;
        MouseDown += Main_MouseDown;
        MouseMove += Main_MouseMove;
        MouseUp += Main_MouseUp;

        pnlContent.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)pbxLogout).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Label lblTitle;
    private Label lblMessage;
    private PictureBox pbxLogout;
    private EoButton btnPatch;
    private EoButton btnLaunch;
    private EoButton btnExit;
    private EoButton btnSkip;
    private ToolTip lblMessageHover;
    private Panel pnlContent;
    private ProgressBar prgPatch;
}
