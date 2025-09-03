namespace Later.App;

internal class PaintableProgressBar : ProgressBar
{
    private SolidBrush? brush;

    public PaintableProgressBar()
    {
        this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (brush == null || brush.Color != this.ForeColor)
            brush = new SolidBrush(this.ForeColor);

        Rectangle rec = new(0, 0, this.Width, this.Height);
        if (ProgressBarRenderer.IsSupported)
            ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rec);
        rec.Width = (int)(rec.Width * (((double)Value - (double)Minimum) / ((double)Maximum - (double)Minimum))) - 4;
        rec.Height = rec.Height - 4;
        e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            brush?.Dispose();
        }
        base.Dispose(disposing);
    }
}
