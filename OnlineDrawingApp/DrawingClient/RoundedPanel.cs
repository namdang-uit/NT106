using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundedPanel : Panel
{

    public int BorderRadius { get; set; } = 45;

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (this.Width > 0 && this.Height > 0)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddArc(0, 0, BorderRadius, BorderRadius, 180, 90); // Góc trên trái
            path.AddArc(Width - BorderRadius, 0, BorderRadius, BorderRadius, 270, 90); 
            path.AddArc(Width - BorderRadius, Height - BorderRadius, BorderRadius, BorderRadius, 0, 90); 
            path.AddArc(0, Height - BorderRadius, BorderRadius, BorderRadius, 90, 90); 
            path.CloseFigure();

            this.Region = new Region(path);
        }
    }
}