namespace EoPatcher.UI;

partial class Main
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
        lblTitle = new Label();
        lblMessage = new Label();
        pbxLogout = new PictureBox();
        pbxPatch = new PictureBox();
        pbxLaunch = new PictureBox();
        pbxExit = new PictureBox();
        pbxSkip = new PictureBox();
        lblMessageHover = new ToolTip(components);
        ((System.ComponentModel.ISupportInitialize)pbxLogout).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pbxPatch).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pbxLaunch).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pbxExit).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pbxSkip).BeginInit();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.Transparent;
        lblTitle.ForeColor = Color.LemonChiffon;
        lblTitle.Location = new Point(13, 13);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(127, 15);
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
        lblMessage.Location = new Point(13, 38);
        lblMessage.Name = "lblMessage";
        lblMessage.Size = new Size(267, 45);
        lblMessage.TabIndex = 2;
        lblMessage.Text = "Loading...";
        lblMessage.MouseDown += Main_MouseDown;
        lblMessage.MouseMove += Main_MouseMove;
        lblMessage.MouseUp += Main_MouseUp;
        // 
        // pbxLogout
        // 
        pbxLogout.Image = Properties.Resources.eo_logout;
        pbxLogout.Location = new Point(247, 8);
        pbxLogout.Name = "pbxLogout";
        pbxLogout.Size = new Size(33, 21);
        pbxLogout.TabIndex = 3;
        pbxLogout.TabStop = false;
        pbxLogout.Click += pbxLogout_Click;
        pbxLogout.MouseDown += pbxLogout_MouseDown;
        pbxLogout.MouseEnter += pbxLogout_MouseEnter;
        pbxLogout.MouseLeave += pbxLogout_MouseLeave;
        pbxLogout.MouseUp += pbxLogout_MouseUp;
        // 
        // pbxPatch
        // 
        pbxPatch.BackColor = Color.Transparent;
        pbxPatch.Image = Properties.Resources.eo_patch;
        pbxPatch.Location = new Point(187, 86);
        pbxPatch.Name = "pbxPatch";
        pbxPatch.Size = new Size(91, 30);
        pbxPatch.TabIndex = 4;
        pbxPatch.TabStop = false;
        pbxPatch.Visible = false;
        pbxPatch.MouseClick += pbxPatch_MouseClick;
        pbxPatch.MouseDown += pbxPatch_MouseDown;
        pbxPatch.MouseEnter += pbxPatch_MouseEnter;
        pbxPatch.MouseLeave += pbxPatch_MouseLeave;
        pbxPatch.MouseUp += pbxPatch_MouseUp;
        // 
        // pbxLaunch
        // 
        pbxLaunch.BackColor = Color.Transparent;
        pbxLaunch.Image = Properties.Resources.eo_launch;
        pbxLaunch.Location = new Point(187, 86);
        pbxLaunch.Name = "pbxLaunch";
        pbxLaunch.Size = new Size(91, 30);
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
        pbxExit.Location = new Point(90, 86);
        pbxExit.Name = "pbxExit";
        pbxExit.Size = new Size(91, 30);
        pbxExit.TabIndex = 6;
        pbxExit.TabStop = false;
        pbxExit.Visible = false;
        pbxExit.Click += pbxExit_Click;
        pbxExit.MouseDown += pbxExit_MouseDown;
        pbxExit.MouseEnter += pbxExit_MouseEnter;
        pbxExit.MouseLeave += pbxExit_MouseLeave;
        pbxExit.MouseUp += pbxExit_MouseUp;
        // 
        // pbxSkip
        // 
        pbxSkip.BackColor = Color.Transparent;
        pbxSkip.Image = Properties.Resources.skip;
        pbxSkip.Location = new Point(187, 53);
        pbxSkip.Name = "pbxSkip";
        pbxSkip.Size = new Size(91, 30);
        pbxSkip.TabIndex = 7;
        pbxSkip.TabStop = false;
        pbxSkip.Visible = false;
        pbxSkip.Click += pbxLaunch_Click;
        pbxSkip.MouseDown += pbxSkip_MouseDown;
        pbxSkip.MouseEnter += pbxSkip_MouseEnter;
        pbxSkip.MouseLeave += pbxSkip_MouseLeave;
        pbxSkip.MouseUp += pbxSkip_MouseUp;
        // 
        // Main
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackgroundImage = Properties.Resources.eo_popup;
        BackgroundImageLayout = ImageLayout.Center;
        ClientSize = new Size(290, 125);
        Controls.Add(pbxSkip);
        Controls.Add(pbxExit);
        Controls.Add(pbxLaunch);
        Controls.Add(pbxPatch);
        Controls.Add(pbxLogout);
        Controls.Add(lblMessage);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.None;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximumSize = new Size(290, 125);
        MinimumSize = new Size(290, 125);
        Name = "Main";
        SizeGripStyle = SizeGripStyle.Hide;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Endless Online Patcher";
        Shown += Main_Shown;
        MouseDown += Main_MouseDown;
        MouseMove += Main_MouseMove;
        MouseUp += Main_MouseUp;
        ((System.ComponentModel.ISupportInitialize)pbxLogout).EndInit();
        ((System.ComponentModel.ISupportInitialize)pbxPatch).EndInit();
        ((System.ComponentModel.ISupportInitialize)pbxLaunch).EndInit();
        ((System.ComponentModel.ISupportInitialize)pbxExit).EndInit();
        ((System.ComponentModel.ISupportInitialize)pbxSkip).EndInit();
        ResumeLayout(false);
        PerformLayout();
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
}
