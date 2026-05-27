using System.ComponentModel;

namespace EndlessOnlinePatcher;

/// <summary>
/// A flat, image-backed Button that:
///   - Swaps between NormalImage and HoverImage on mouse enter/leave
///   - Draws a gold focus border so gamepad/keyboard focus is visible
///   - Suppresses hover swaps while Locked (used during the patching state)
/// </summary>
internal sealed class EoButton : Button
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Image? NormalImage { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Image? HoverImage { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Locked { get; set; }

    public EoButton()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        FlatAppearance.MouseOverBackColor = Color.Transparent;
        FlatAppearance.MouseDownBackColor = Color.Transparent;
        BackColor = Color.Transparent;
        BackgroundImageLayout = ImageLayout.Zoom;
        Text = string.Empty;
        UseVisualStyleBackColor = false;
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        if (!Locked && HoverImage != null)
            BackgroundImage = HoverImage;
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        if (!Locked)
            BackgroundImage = NormalImage;
    }

    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
        Invalidate();
    }

    protected override void OnLostFocus(EventArgs e)
    {
        base.OnLostFocus(e);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (Focused)
        {
            using var pen = new Pen(Color.Gold, 3);
            e.Graphics.DrawRectangle(pen, new Rectangle(2, 2, Width - 5, Height - 5));
        }
    }
}
