using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ACTTimeline
{
    public class TimelineView : Form
    {
        private DataGridView dataGridView;
        private OverlayButtonsForm buttons;
        private CachedSoundPlayer soundplayer;

        private int numberOfRowsToDisplay;
        public int NumberOfRowsToDisplay
        {
            get { return numberOfRowsToDisplay; }
            set
            {
                numberOfRowsToDisplay = value;

                int gridHeight = dataGridView.RowTemplate.Height * numberOfRowsToDisplay;
                this.Height = gridHeight;
                dataGridView.Height = gridHeight;
            }
        }

        private bool moveByDrag;
        public bool MoveByDrag {
            get { return moveByDrag; }
            set
            {
                moveByDrag = value;
                Win32APIUtils.SetWS_EX_TRANSPARENT(Handle, !moveByDrag);
            }
        }

        private bool showOverlayButtons;
        public bool ShowOverlayButtons
        {
            get { return showOverlayButtons; }
            set
            {
                showOverlayButtons = value;
                OnVisibleChanged(EventArgs.Empty);
            }
        }

        private TimelineController controller;
        
        public TimelineView(TimelineController controller_)
        {
            controller = controller_;
            controller.TimelineUpdate += controller_TimelineUpdate;
            controller.CurrentTimeUpdate += controller_CurrentTimeUpdate;

            SetupUI();

            this.MouseDown += TimelineView_MouseDown;
            this.VisibleChanged += TimelineView_VisibleChanged;
            this.Move += TimelineView_Move;

            typeof(DataGridView).
                GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).
                SetValue(dataGridView, true, null);
            dataGridView.SelectionChanged += (object sender, EventArgs args) => dataGridView.ClearSelection();
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView.Columns.Add(new TimeLeftColumn { Controller = controller_ });

            this.Opacity = 0.8;
            NumberOfRowsToDisplay = 3;
            MoveByDrag = true;
            ShowOverlayButtons = true;

            soundplayer = new CachedSoundPlayer();
        }

        const int UIWidth = 200;

        private void SetupUI()
        {
            dataGridView = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)(dataGridView)).BeginInit();
            this.SuspendLayout();

            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.ColumnHeadersVisible = false;
            dataGridView.Enabled = false;
            dataGridView.Location = new Point(0, 0);
            dataGridView.Margin = new Padding(0);
            dataGridView.MultiSelect = false;
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.ScrollBars = ScrollBars.None;
            dataGridView.Size = new Size(UIWidth, 80);

            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(UIWidth, 80);
            this.Controls.Add(dataGridView);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "TimelineView";
            this.Text = "Timeline";
            this.TopMost = true;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(dataGridView)).EndInit();
            this.ResumeLayout(false);

            buttons = new OverlayButtonsForm(controller);
        }

        void TimelineView_VisibleChanged(object sender, EventArgs e)
        {
            buttons.Visible = Visible && showOverlayButtons;
        }

        void TimelineView_Move(object sender, EventArgs e)
        {
            buttons.Location = new Point(this.Location.X + UIWidth - buttons.Width, this.Location.Y - buttons.Height);
        }

        void TimelineView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && MoveByDrag)
            {
                Win32APIUtils.DragMove(Handle);
            }
        }

        void controller_TimelineUpdate(object sender, EventArgs e)
        {
            if (controller.Timeline == null)
                return;

            foreach (AlertSound sound in controller.Timeline.AlertSoundAssets.All)
            {
                soundplayer.WarmUpCache(sound.Filename);
            }
        }

        void controller_CurrentTimeUpdate(object sender, EventArgs e)
        {
            Timeline timeline = controller.Timeline;
            if (timeline == null)
                return;

            if (InvokeRequired)
            {
                Invoke(new Action(() => { controller_CurrentTimeUpdate(sender, e); }));
                return;
            }
            else
            {
                // play pending alerts
                var pendingAlerts = timeline.PendingAlertsAt(controller.CurrentTime);
                foreach (ActivityAlert pendingAlert in pendingAlerts)
                {
                    soundplayer.PlaySound(pendingAlert.Sound.Filename);
                    pendingAlert.Processed = true;
                }

                // sync dataGridView
                dataGridView.DataSource = null;
                dataGridView.DataSource = timeline.VisibleItemsAt(controller.CurrentTime - TimeLeftCell.THRESHOLD).ToList();
            }
        }
    }

    class TimeLeftColumn : DataGridViewColumn
    {
        public TimelineController Controller;

        public TimeLeftColumn()
        {
            this.Width = 100;
            this.ReadOnly = true;
            this.CellTemplate = new TimeLeftCell();
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
            this.DataPropertyName = "Self";
        }
    }

    class TimeLeftCell : DataGridViewTextBoxCell
    {
        public const float BAR_START = 10.0F;
        public const float THRESHOLD = 1.0F;
        
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

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, "", "", errorText, cellStyle, advancedBorderStyle, paintParts);

            TimelineActivity activity = (TimelineActivity)value;
            TimelineController controller = ((TimeLeftColumn)OwningColumn).Controller;

            {
                double timeTillStart = activity.TimeFromStart - controller.CurrentTime;
                float timeTillStartF = (float)timeTillStart;
                float durationF = (float)activity.Duration;

                if (durationF < 0.1F)
                    durationF = 0.1F;

                PaintBar(graphics, cellBounds, timeTillStartF, durationF);
            }

            {
                double timeTillEnd = activity.EndTime - controller.CurrentTime;
                string text = timeTillEnd > 0 ? timeTillEnd.ToString("0") : "ACTION!";
                PaintText(graphics, cellBounds, text);
            }
        }

        private static RectangleF DrawAreaFromCellBounds(Rectangle cellBounds)
        {
            return new RectangleF(cellBounds.X + MARGIN, cellBounds.Y + MARGIN, cellBounds.Width - MARGIN * 2, cellBounds.Height - MARGIN * 2);
        }

        private static void PaintText(Graphics graphics, Rectangle cellBounds, string valueString)
        {
            RectangleF drawArea = DrawAreaFromCellBounds(cellBounds);

            RectangleF textRect = drawArea;

            GraphicsPath pathText = new GraphicsPath();
            pathText.AddString(valueString, ValueFont.FontFamily, (int)ValueFont.Style, ValueFont.Size, textRect, ValueStringFormat);

            GraphicsState state = graphics.Save();
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawPath(ValuePen, pathText);
            graphics.FillPath(ValueFill, pathText);
            graphics.Restore(state);
        }

        private static void PaintBar(Graphics graphics, Rectangle cellBounds, float timeTillStart, float duration)
        {
            RectangleF drawArea = DrawAreaFromCellBounds(cellBounds);
            RectangleF barFill = drawArea;

            Color colorA;
            Color colorB;
            Rectangle gradientRect;
            if (timeTillStart > BAR_START)
            {
                // draw nothing.
                return;
            }
            else if (timeTillStart > 0)
            {
                float bar = (BAR_START - timeTillStart) / BAR_START;
                if (bar > 1.0F)
                    bar = 1.0F;

                barFill.X += barFill.Width;
                barFill.Width *= bar;
                barFill.X -= barFill.Width;

                colorA = BarColorAtTimeLeft(timeTillStart);
                colorB = ControlPaint.Light(colorA, 1.0F);
                gradientRect = Rectangle.Ceiling(barFill);
            }
            else
            {
                float bar = -timeTillStart / duration;
                if (bar > 1.0F)
                    bar = 1.0F;

                barFill.Width *= bar;

                colorA = Color.Aqua;
                colorB = ControlPaint.Light(colorA, 1.0F);
                gradientRect = Rectangle.Ceiling(barFill);
            }

            if (barFill.Width < 1.0F)
                return;

            Brush barBrush = new LinearGradientBrush(gradientRect, colorA, colorB, LinearGradientMode.Horizontal) { WrapMode = WrapMode.TileFlipX };
            graphics.FillRectangle(barBrush, barFill);
        }
    }
}
