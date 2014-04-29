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

            plugin.TimelineView.Move += TimelineView_Move;
            plugin.TimelineView.CurrentTimeUpdate += TimelineView_CurrentTimeUpdate;
        }

        public static string FormatMMSS(double time)
        {
            var mm = Math.Floor(time / 60.0);
            var ss = time - mm * 60.0;
            return String.Format("{0:00}:{1:00}", mm, ss);
        }

        void TimelineView_CurrentTimeUpdate(object sender, EventArgs e)
        {
            double currtime = plugin.TimelineView.CurrentTime;
            double endtime = plugin.TimelineView.Timeline.EndTime;
            labelCurrPos.Text = FormatMMSS(currtime);
            labelEndPos.Text = FormatMMSS(endtime);
            trackBar.Value = (int)currtime;
            trackBar.Maximum = (int)endtime;
        }

        void TimelineView_Move(object sender, EventArgs e)
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
            plugin.TimelineTxtFilePath = String.Format("{0}/{1}", Globals.TimelineTxtsRoot, timelineTxtFilePath);
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

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            plugin.TimelineView.CurrentTime = (int)trackBar.Value;
        }

        private void buttonRewind_Click(object sender, EventArgs e)
        {
            plugin.TimelineView.CurrentTime = 0;
        }
    }
}
