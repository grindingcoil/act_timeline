using Advanced_Combat_Tracker;
using System;
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

                Settings = new PluginSettings(this);
                Settings.AddStringSetting("TimelineTxtFilePath");

                SetupTab();
                Settings.Load();

                StatusText.Text = "Plugin Started (^^)!";
            }
            catch(Exception e)
            {
                if (StatusText != null)
                    StatusText.Text = "Plugin Init Failed: "+e.Message;
            }
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
