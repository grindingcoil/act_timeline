using Advanced_Combat_Tracker;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ACTTimeline
{
    public class ACTPlugin : IActPluginV1
    {
        public TabPage ScreenSpace { get; private set; }
        public Label StatusText { get; private set; }

        public PluginSettings Settings { get; private set; }

        private ACTTabPageControl tabPageControl;

        public TimelineView TimelineView { get; private set; }

        private Timeline timeline;
        private Timeline Timeline
        {
            get { return timeline; }
            set
            {
                timeline = value;
                TimelineView.Timeline = timeline;
            }
        }

        private string timelineTxtFilePath;
        public string TimelineTxtFilePath
        {
            get { return timelineTxtFilePath; }
            set
            {
                if (value == null || value == "")
                    return;

                try
                {
                    if (!File.Exists(value))
                        throw new ResourceNotFoundException(value);

                    timelineTxtFilePath = value;
                    Timeline = TimelineLoader.LoadFromFile(timelineTxtFilePath);
                }
                catch(Exception e)
                {
                    MessageBox.Show(String.Format("Failed to load timeline. Error: {0}", e.Message), "ACT Timeline Plugin");
                }
            }
        }

        private CheckBox checkBoxShowView;

        public ACTPlugin()
        {
            // See |InitPlugin()|
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            try
            {
                Assembly.LoadFrom("Sprache.dll");

                ScreenSpace = pluginScreenSpace;
                StatusText = pluginStatusText;

                TimelineView = new TimelineView();
                TimelineView.Show();
                TimelineView.DoubleClick += TimelineView_DoubleClick;

                Settings = new PluginSettings(this);
                Settings.AddStringSetting("TimelineTxtFilePath");

                SetupTab();
                InjectButton();

                Settings.Load();

                StatusText.Text = "Plugin Started (^^)!";
            }
            catch(Exception e)
            {
                if (StatusText != null)
                    StatusText.Text = "Plugin Init Failed: "+e.Message;
            }
        }

        void TimelineView_DoubleClick(object sender, EventArgs e)
        {
            TimelineView.Hide();
            checkBoxShowView.Checked = false;
        }

        void InjectButton()
        {
            checkBoxShowView = new CheckBox();
            checkBoxShowView.Appearance = System.Windows.Forms.Appearance.Button;
            checkBoxShowView.Name = "checkBoxShowView";
            checkBoxShowView.Size = new System.Drawing.Size(90, 24);
            checkBoxShowView.Text = "Show Timeline";
            checkBoxShowView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            checkBoxShowView.UseVisualStyleBackColor = true;
            checkBoxShowView.Checked = true;
            checkBoxShowView.CheckedChanged += checkBoxShowView_CheckedChanged;
            Settings.AddControlSetting("TimelineShown", checkBoxShowView);

            var formMain = ActGlobals.oFormActMain;
            formMain.Resize += formMain_Resize;
            formMain.Controls.Add(checkBoxShowView);
            formMain.Controls.SetChildIndex(checkBoxShowView, 0);

            formMain_Resize(this, null);
        }

        void checkBoxShowView_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowView.Checked)
                TimelineView.Show();
            else
                TimelineView.Hide();
        }

        void formMain_Resize(object sender, EventArgs e)
        {
            // update button location
            var mainFormSize = ActGlobals.oFormActMain.Size;
            checkBoxShowView.Location = new Point(mainFormSize.Width - 440, 0);
        }

        void SetupTab()
        {
            ScreenSpace.Text = "ACT Timeline";

            tabPageControl = new ACTTabPageControl(this);
            ScreenSpace.Controls.Add(tabPageControl);
            ScreenSpace.Resize += ScreenSpace_Resize;
            ScreenSpace_Resize(this, null);

            tabPageControl.Show();
        }

        void ScreenSpace_Resize(object sender, EventArgs e)
        {
            tabPageControl.Location = new System.Drawing.Point(0, 0);
            tabPageControl.Size = ScreenSpace.Size;
        }
    
        void IActPluginV1.DeInitPlugin()
        {
            Settings.Save();

            TimelineView.Close();

            StatusText.Text = "Plugin Exited m(_ _)m";
        }
    }
}
