using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ACTTimeline
{
    public partial class TimelineView : Form
    {
        private Timer timer;
        
        private Timeline timeline;
        private RelativeClock relativeClock;

        const int NUMBER_OF_ROWS_TO_DISPLAY = 3;

        public TimelineView()
        {
            InitializeComponent();

            timeline = TimelineLoader.LoadFromFile(@"..\..\..\..\resources\timeline\test.txt");

            this.MouseDown += form_MouseDown;
            this.DoubleClick += (object sender, EventArgs e) => { this.Close(); };

            typeof(DataGridView).
                GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).
                SetValue(dataGridView, true, null);
            dataGridView.SelectionChanged += (object sender, EventArgs args) => dataGridView.ClearSelection();
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView.Columns.Add(new TimeLeftColumn { DataPropertyName = "TimeLeft" });
            
            timer = new Timer();
            timer.Tick += timer_Tick;
            timer.Interval = 50;
            timer.Start();

            relativeClock = new RelativeClock();

            this.Opacity = 0.8;
            this.Height = dataGridView.RowTemplate.Height * NUMBER_OF_ROWS_TO_DISPLAY;
            dataGridView.Height = this.Height;

            Synchronize();
        }

        void form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Win32APIUtils.DragMove(Handle);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Synchronize();
        }

        private void Synchronize()
        {
            timeline.CurrentTime = relativeClock.CurrentTime();

            // timeline.PendingAlerts()

            dataGridView.DataSource = null;
            dataGridView.DataSource = timeline.VisibleItems(TimeLeftCell.THRESHOLD).ToList();
        }
    }

    class TimeLeftColumn : DataGridViewColumn
    {
        public TimeLeftColumn()
        {
            this.Width = 100;
            this.ReadOnly = true;
            this.CellTemplate = new TimeLeftCell();
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
        }
    }

    class TimeLeftCell : DataGridViewTextBoxCell
    {
        public const float BAR_START = 10.0F;
        public const float MAX_BAR_RATIO = 1.1F;
        public const float THRESHOLD = - (MAX_BAR_RATIO - 1.0F) * BAR_START;
        
        public const int MARGIN = 4; // px

        static private readonly Font ValueFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
        static private readonly StringFormat ValueStringFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
        static private readonly Pen ValuePen = new Pen(Brushes.White, 1.0F) { LineJoin = LineJoin.Round };
        static private readonly Brush ValueFill = Brushes.Black;

        public static Color BarColorAtTimeLeft(float timeLeft)
        {
            if (timeLeft > 5)
                return Color.GreenYellow;
            else if (timeLeft > 3)
                return Color.Orange;
            else if (timeLeft > 0)
                return Color.OrangeRed;
            else
                return Color.Red;
        }

        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, "", "", errorText, cellStyle, advancedBorderStyle, paintParts);

            float timeLeft = Convert.ToSingle(value);

            // draw bar
            float bar = (BAR_START - timeLeft) / BAR_START;
            if (bar > MAX_BAR_RATIO)
                bar = MAX_BAR_RATIO;

            RectangleF barArea = new RectangleF(cellBounds.X + MARGIN, cellBounds.Y + MARGIN, cellBounds.Width - MARGIN * 2, cellBounds.Height - MARGIN * 2);
            RectangleF barFill = barArea;
            barFill.Width *= bar / MAX_BAR_RATIO;

            if (barFill.Width > 1)
            {
                Color color = BarColorAtTimeLeft(timeLeft);
                Color lighterColor = ControlPaint.Light(color, 1.0F);

                Rectangle gradientRect = Rectangle.Ceiling(barFill);
                Brush barBrush = new LinearGradientBrush(gradientRect, lighterColor, color, LinearGradientMode.Horizontal) { WrapMode = WrapMode.TileFlipX };
                
                graphics.FillRectangle(barBrush, barFill);
            }
            float barX = barArea.X + barArea.Width / MAX_BAR_RATIO;
            graphics.DrawLine(Pens.DarkGray, barX, cellBounds.Y, barX, cellBounds.Y + cellBounds.Height);
 
            // draw text
            string valueString = timeLeft.ToString("0");

            RectangleF textRect = barArea;
            textRect.Width *= 1.0F / MAX_BAR_RATIO;

            GraphicsPath pathText = new GraphicsPath();
            pathText.AddString(valueString, ValueFont.FontFamily, (int)ValueFont.Style, ValueFont.Size, textRect, ValueStringFormat);

            GraphicsState state = graphics.Save();
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawPath(ValuePen, pathText);
            graphics.FillPath(ValueFill, pathText);
            graphics.Restore(state);
        }
    }
}
