using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace YtDlpGUI
{
    internal sealed class GradientPanel : Panel
    {
        public Color StartColor { get; set; } = Color.FromArgb(5, 24, 50);
        public Color EndColor { get; set; } = Color.FromArgb(9, 76, 157);
        public LinearGradientMode GradientMode { get; set; } = LinearGradientMode.Horizontal;

        public GradientPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle, StartColor, EndColor, GradientMode))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }
    }

    internal sealed class CardPanel : Panel
    {
        public Color BorderColor { get; set; } = Color.FromArgb(29, 67, 105);
        public int CornerRadius { get; set; } = 14;

        public CardPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            UpdateRegion();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle bounds = new Rectangle(0, 0, Math.Max(1, Width - 1), Math.Max(1, Height - 1));
            using (GraphicsPath path = CreateRoundedPath(bounds, CornerRadius))
            using (Pen pen = new Pen(BorderColor))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void UpdateRegion()
        {
            if (Width <= 0 || Height <= 0)
                return;

            Rectangle bounds = new Rectangle(0, 0, Width, Height);
            using (GraphicsPath path = CreateRoundedPath(bounds, CornerRadius))
            {
                Region old = Region;
                Region = new Region(path);
                if (old != null)
                    old.Dispose();
            }
        }

        private static GraphicsPath CreateRoundedPath(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = Math.Max(2, radius * 2);
            Rectangle arc = new Rectangle(rectangle.Location, new Size(diameter, diameter));

            path.AddArc(arc, 180, 90);
            arc.X = rectangle.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = rectangle.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = rectangle.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    internal sealed class ModernProgressBar : Control
    {
        private int _value;

        public int Minimum { get; set; } = 0;
        public int Maximum { get; set; } = 100;
        public Color TrackColor { get; set; } = Color.FromArgb(14, 42, 72);
        public Color FillColor { get; set; } = Color.FromArgb(0, 153, 255);
        public Color GlowColor { get; set; } = Color.FromArgb(80, 196, 255);

        public int Value
        {
            get { return _value; }
            set
            {
                int next = Math.Max(Minimum, Math.Min(Maximum, value));
                if (_value == next)
                    return;
                _value = next;
                Invalidate();
            }
        }

        public ModernProgressBar()
        {
            DoubleBuffered = true;
            Height = 12;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle track = new Rectangle(0, 0, Math.Max(1, Width - 1), Math.Max(1, Height - 1));
            int radius = Math.Max(2, Height / 2);
            using (GraphicsPath trackPath = RoundedRect(track, radius))
            using (SolidBrush trackBrush = new SolidBrush(TrackColor))
            {
                e.Graphics.FillPath(trackBrush, trackPath);
            }

            if (Maximum <= Minimum || Value <= Minimum)
                return;

            double ratio = (Value - Minimum) / (double)(Maximum - Minimum);
            int fillWidth = Math.Max(Height, (int)Math.Round(track.Width * ratio));
            fillWidth = Math.Min(track.Width, fillWidth);
            Rectangle fill = new Rectangle(0, 0, fillWidth, track.Height);
            using (GraphicsPath fillPath = RoundedRect(fill, radius))
            using (LinearGradientBrush brush = new LinearGradientBrush(fill, FillColor, GlowColor, LinearGradientMode.Horizontal))
            {
                e.Graphics.FillPath(brush, fillPath);
            }
        }

        private static GraphicsPath RoundedRect(Rectangle rectangle, int radius)
        {
            int diameter = Math.Max(2, radius * 2);
            GraphicsPath path = new GraphicsPath();
            Rectangle arc = new Rectangle(rectangle.Left, rectangle.Top, diameter, diameter);
            path.AddArc(arc, 90, 180);
            arc.X = rectangle.Right - diameter;
            path.AddArc(arc, 270, 180);
            path.CloseFigure();
            return path;
        }
    }
}
