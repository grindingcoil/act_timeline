using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ACTTimeline
{
    public partial class ACTTabPageControl : UserControl
    {
        private ACTPlugin plugin;
        private bool updateFromOverlayMove;

        public ACTTabPageControl(ACTPlugin plugin_)
        {
            InitializeComponent();

            plugin = plugin_;
            updateFromOverlayMove = false;

            var settings = plugin.Settings;
            settings.AddControlSetting("ResourcesDir", textBoxResourceDir);
            settings.AddControlSetting("OverlayX", udOverlayX);
            settings.AddControlSetting("OverlayY", udOverlayY);
            settings.AddControlSetting("NumberOfRowsToDisplay", udNumRows);
            settings.AddControlSetting("MoveOverlayByDrag", checkBoxMoveOverlayByDrag);
            settings.AddControlSetting("ShowOverlayButtons", checkBoxShowOverlayButtons);

            plugin.TimelineView.Move += TimelineView_Move;
            plugin.TimelineView.TimelineFontChanged += TimelineView_TimelineFontChanged;
            plugin.TimelineView.ColumnWidthChanged += TimelineView_ColumnWidthChanged;
            plugin.TimelineView.OpacityChanged += TimelineView_OpacityChanged;
            plugin.Controller.CurrentTimeUpdate += Controller_CurrentTimeUpdate;
            plugin.Controller.TimelineUpdate += Controller_TimelineUpdate;
            plugin.Controller.PausedUpdate += Controller_PausedUpdate;
            TimelineView_TimelineFontChanged(this, null);
            TimelineView_ColumnWidthChanged(this, null);
            TimelineView_OpacityChanged(this, null);
            Controller_TimelineUpdate(this, null);
            Controller_PausedUpdate(this, null);
        }

        private void Controller_PausedUpdate(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Controller_PausedUpdate(sender, e)));
                return;
            }

            buttonPause.Enabled = !plugin.Controller.Paused;
            buttonPlay.Enabled = plugin.Controller.Paused;
        }

        public static string FormatMMSS(double time)
        {
            var mm = Math.Floor(time / 60.0);
            var ss = time - mm * 60.0;
            return String.Format("{0:00}:{1:00}", mm, ss);
        }

        private void Controller_TimelineUpdate(object sender, EventArgs e)
        {
            Timeline timeline = plugin.Controller.Timeline;
            if (timeline == null)
                return;

            double endtime = timeline.EndTime;
            labelEndPos.Text = FormatMMSS(endtime);
            trackBar.Maximum = (int)Math.Ceiling(endtime);

            labelLoadedTimeline.Text = timeline.Name;
        }

        private void Controller_CurrentTimeUpdate(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { Controller_CurrentTimeUpdate(sender, e); }));
                return;
            }

            double currtime = plugin.Controller.CurrentTime;
            labelCurrPos.Text = FormatMMSS(currtime);

            int currTimeInt = (int)Math.Floor(currtime);
            if (currTimeInt < trackBar.Maximum)
                trackBar.Value = currTimeInt;
        }

        private void TimelineView_Move(object sender, EventArgs e)
        {
            updateFromOverlayMove = true;
            udOverlayX.Value = plugin.TimelineView.Left;
            udOverlayY.Value = plugin.TimelineView.Top;
            updateFromOverlayMove = false;
        }

        private void buttonResourceDirSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();

            textBoxResourceDir.Text = folderBrowserDialog.SelectedPath;
        }

        private void textBoxResourceDir_TextChanged(object sender, EventArgs e)
        {
            Globals.ResourceRoot = textBoxResourceDir.Text;
            Synchronize();
        }

        private string GenerateDirStatusString()
        {
            if (!Directory.Exists(Globals.ResourceRoot))
            {
                return "Resource dir not found :/";
            }

            string statusText = "Resource dir exists! ";

            if (!Directory.Exists(Globals.SoundFilesRoot))
            {
                statusText += "Sound files dir not found!";
                return statusText;
            }
            statusText += String.Format("Found {0} sound files. ", Globals.NumberOfSoundFilesInResourcesDir());
            
            if (!Directory.Exists(Globals.TimelineTxtsRoot))
            {
                statusText += "Timeline txt files dir not found!";
                return statusText;
            }
            statusText += String.Format("Found {0} timeline txt files.", Globals.TimelineTxtsInResourcesDir.Length);

            return statusText;
        }

        private void Synchronize()
        {
            labelResourceDirStatus.Text = GenerateDirStatusString();
            
            // update timeline list
            listTimelines.Items.Clear();
            foreach (string fullpath in Globals.TimelineTxtsInResourcesDir)
            {
                listTimelines.Items.Add(Path.GetFileName(fullpath));
            }
        }

        private void buttonResourceDirOpen_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Globals.ResourceRoot))
                Process.Start(Globals.ResourceRoot);
        }

        private void buttonRefreshList_Click(object sender, EventArgs e)
        {
            Synchronize();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            string timelineTxtFilePath = (string)listTimelines.SelectedItem;
            plugin.Controller.TimelineTxtFilePath = String.Format("{0}/{1}", Globals.TimelineTxtsRoot, timelineTxtFilePath);
        }

        private void udOverlayX_ValueChanged(object sender, EventArgs e)
        {
            if (!updateFromOverlayMove)
                plugin.TimelineView.Left = (int)udOverlayX.Value;
        }

        private void udOverlayY_ValueChanged(object sender, EventArgs e)
        {
            if (!updateFromOverlayMove)
                plugin.TimelineView.Top = (int)udOverlayY.Value;
        }

        private void udNumRows_ValueChanged(object sender, EventArgs e)
        {
            plugin.TimelineView.NumberOfRowsToDisplay = (int)udNumRows.Value;
        }

        private void checkBoxMoveOverlayByDrag_CheckedChanged(object sender, EventArgs e)
        {
            plugin.TimelineView.MoveByDrag = checkBoxMoveOverlayByDrag.Checked;
        }

        private void trackbar_Scroll(object sender, EventArgs e)
        {
            plugin.Controller.CurrentTime = (int)trackBar.Value;
        }

        private void buttonRewind_Click(object sender, EventArgs e)
        {
            plugin.Controller.CurrentTime = 0;
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            plugin.Controller.Paused = true;
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            plugin.Controller.Paused = false;
        }

        private void checkBoxShowOverlayButtons_CheckedChanged(object sender, EventArgs e)
        {
            plugin.TimelineView.ShowOverlayButtons = checkBoxShowOverlayButtons.Checked;
        }

        private void TimelineView_TimelineFontChanged(object sender, EventArgs e)
        {
            labelCurrentFont.Text = plugin.FontString;
        }

        private void buttonFontSelect_Click(object sender, EventArgs e)
        {
            FontDialog fontdialog = new FontDialog();
            fontdialog.Font = plugin.TimelineView.TimelineFont;

            if (fontdialog.ShowDialog() != DialogResult.Cancel)
            {
                plugin.TimelineView.TimelineFont = fontdialog.Font;
            }
        }

        private void TimelineView_ColumnWidthChanged(object sender, EventArgs e)
        {
            udTextWidth.Value = plugin.TimelineView.TextWidth;
            udBarWidth.Value = plugin.TimelineView.BarWidth;
        }

        private void udTextWidth_ValueChanged(object sender, EventArgs e)
        {
            plugin.TimelineView.TextWidth = (int)udTextWidth.Value;
        }

        private void udBarWidth_ValueChanged(object sender, EventArgs e)
        {
            plugin.TimelineView.BarWidth = (int)udBarWidth.Value;
        }

        private void TimelineView_OpacityChanged(object sender, EventArgs e)
        {
            int percentage = (int)(plugin.TimelineView.MyOpacity * 100);

            labelCurrOpacity.Text = String.Format("{0}%", percentage);

            percentage = Math.Min(trackBarOpacity.Maximum, percentage);
            percentage = Math.Max(trackBarOpacity.Minimum, percentage);
            trackBarOpacity.Value = percentage;
        }

        private void trackBarOpacity_Scroll(object sender, EventArgs e)
        {
            plugin.TimelineView.MyOpacity = ((double)trackBarOpacity.Value) / 100;
        }
    }
}
