namespace EoPatcher.UI;

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
        pbxPatch = new PictureBox();
        pbxLaunch = new PictureBox();
        pbxExit = new PictureBox();
        pbxSkip = new PictureBox();
        prgPatch = new ProgressBar();
        lblMessageHover = new ToolTip(components);
        pnlContent.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pbxLogout).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pbxPatch).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pbxLaunch).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pbxExit).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pbxSkip).BeginInit();
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
        pnlContent.Controls.Add(pbxPatch);
        pnlContent.Controls.Add(pbxLaunch);
        pnlContent.Controls.Add(pbxExit);
        pnlContent.Controls.Add(pbxSkip);
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
        lblTitle.TabIndex = 1;
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
        lblMessage.TabIndex = 2;
        lblMessage.Text = "Loading...";
        lblMessage.MouseDown += Main_MouseDown;
        lblMessage.MouseMove += Main_MouseMove;
        lblMessage.MouseUp += Main_MouseUp;

        //
        // pbxLogout (top-right close button, native size repositioned)
        //
        pbxLogout.BackColor = Color.Transparent;
        pbxLogout.Image = Properties.Resources.eo_logout;
        pbxLogout.Location = new Point(762, 16);
        pbxLogout.Name = "pbxLogout";
        pbxLogout.Size = new Size(33, 21);
        pbxLogout.SizeMode = PictureBoxSizeMode.Zoom;
        pbxLogout.TabIndex = 3;
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
        prgPatch.TabIndex = 8;
        prgPatch.Visible = false;

        //
        // pbxPatch — 3× scaled with Zoom mode
        //
        pbxPatch.BackColor = Color.Transparent;
        pbxPatch.Image = Properties.Resources.eo_patch;
        pbxPatch.Location = new Point(494, 398);
        pbxPatch.Name = "pbxPatch";
        pbxPatch.Size = new Size(273, 90);
        pbxPatch.SizeMode = PictureBoxSizeMode.Zoom;
        pbxPatch.TabIndex = 4;
        pbxPatch.TabStop = false;
        pbxPatch.Visible = false;
        pbxPatch.MouseClick += pbxPatch_MouseClick;
        pbxPatch.MouseDown += pbxPatch_MouseDown;
        pbxPatch.MouseEnter += pbxPatch_MouseEnter;
        pbxPatch.MouseLeave += pbxPatch_MouseLeave;
        pbxPatch.MouseUp += pbxPatch_MouseUp;

        //
        // pbxLaunch — same position as pbxPatch, mutually exclusive visibility
        //
        pbxLaunch.BackColor = Color.Transparent;
        pbxLaunch.Image = Properties.Resources.eo_launch;
        pbxLaunch.Location = new Point(494, 398);
        pbxLaunch.Name = "pbxLaunch";
        pbxLaunch.Size = new Size(273, 90);
        pbxLaunch.SizeMode = PictureBoxSizeMode.Zoom;
        pbxLaunch.TabIndex = 5;
        pbxLaunch.TabStop = false;
        pbxLaunch.Visible = false;
        pbxLaunch.Click += pbxLaunch_Click;
        pbxLaunch.MouseDown += pbxLaunch_MouseDown;
        pbxLaunch.MouseEnter += pbxLaunch_MouseEnter;
        pbxLaunch.MouseLeave += pbxLaunch_MouseLeave;
        pbxLaunch.MouseUp += pbxLaunch_MouseUp;

        //
        // pbxExit
        //
        pbxExit.BackColor = Color.Transparent;
        pbxExit.Image = Properties.Resources.eo_exit;
        pbxExit.Location = new Point(33, 398);
        pbxExit.Name = "pbxExit";
        pbxExit.Size = new Size(273, 90);
        pbxExit.SizeMode = PictureBoxSizeMode.Zoom;
        pbxExit.TabIndex = 6;
        pbxExit.TabStop = false;
        pbxExit.Visible = false;
        pbxExit.Click += pbxExit_Click;
        pbxExit.MouseDown += pbxExit_MouseDown;
        pbxExit.MouseEnter += pbxExit_MouseEnter;
        pbxExit.MouseLeave += pbxExit_MouseLeave;
        pbxExit.MouseUp += pbxExit_MouseUp;

        //
        // pbxSkip — sits above Patch, same X column
        //
        pbxSkip.BackColor = Color.Transparent;
        pbxSkip.Image = Properties.Resources.skip;
        pbxSkip.Location = new Point(494, 300);
        pbxSkip.Name = "pbxSkip";
        pbxSkip.Size = new Size(273, 90);
        pbxSkip.SizeMode = PictureBoxSizeMode.Zoom;
        pbxSkip.TabIndex = 7;
        pbxSkip.TabStop = false;
        pbxSkip.Visible = false;
        pbxSkip.Click += pbxLaunch_Click;
        pbxSkip.MouseDown += pbxSkip_MouseDown;
        pbxSkip.MouseEnter += pbxSkip_MouseEnter;
        pbxSkip.MouseLeave += pbxSkip_MouseLeave;
        pbxSkip.MouseUp += pbxSkip_MouseUp;

        //
        // Main form — 1280×720 (16:9), black surround, borderless
        //
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.Black;
        ClientSize = new Size(1280, 720);
        Controls.Add(pnlContent);
        FormBorderStyle = FormBorderStyle.None;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximumSize = new Size(1280, 720);
        MinimumSize = new Size(1280, 720);
        Name = "Main";
        SizeGripStyle = SizeGripStyle.Hide;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Endless Online Patcher";
        Shown += Main_Shown;
        MouseDown += Main_MouseDown;
        MouseMove += Main_MouseMove;
        MouseUp += Main_MouseUp;

        pnlContent.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)pbxLogout).EndInit();
        ((System.ComponentModel.ISupportInitialize)pbxPatch).EndInit();
        ((System.ComponentModel.ISupportInitialize)pbxLaunch).EndInit();
        ((System.ComponentModel.ISupportInitialize)pbxExit).EndInit();
        ((System.ComponentModel.ISupportInitialize)pbxSkip).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Label lblTitle;
    private Label lblMessage;
    private PictureBox pbxLogout;
    private PictureBox pbxPatch;
    private PictureBox pbxLaunch;
    private PictureBox pbxExit;
    private PictureBox pbxSkip;
    private ToolTip lblMessageHover;
    private Panel pnlContent;
    private ProgressBar prgPatch;
}
