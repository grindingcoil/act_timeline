using Advanced_Combat_Tracker;
using System;
using System.ComponentModel;
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

        public TimelineController Controller { get; private set; }

        public PluginSettings Settings { get; private set; }

        private ACTTabPageControl tabPageControl;
        public TimelineView TimelineView { get; private set; }
        private CheckBox checkBoxShowView;

        // delegate for PluginSettings
        public string TimelineTxtFilePath
        {
            get { return Controller.TimelineTxtFilePath; }
            set { Controller.TimelineTxtFilePath = value; }
        }

        public string FontString
        {
            get { return TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(TimelineView.TimelineFont); }
            set { TimelineView.TimelineFont = TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(value) as Font; }
        }

        public ACTPlugin()
        {
            // See |InitPlugin()|
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            try
            {
                ScreenSpace = pluginScreenSpace;
                StatusText = pluginStatusText;

                StatusText.Text = "Loading Sprache.dll";
#if DEBUG
                // Sprache.dll is already injected by libZ in Release builds.
                Assembly.LoadFrom("Sprache.dll");
#endif
                StatusText.Text = "Sprache.dll Load Success!";

#if DEBUG
                // See Issue #1
                // Control.CheckForIllegalCrossThreadCalls = true;
#endif

                Controller = new TimelineController();

                TimelineView = new TimelineView(Controller);
                TimelineView.Show();
                TimelineView.DoubleClick += TimelineView_DoubleClick;

                Settings = new PluginSettings(this);
                Settings.AddStringSetting("TimelineTxtFilePath");
                Settings.AddStringSetting("FontString");

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
            checkBoxShowView.Location = new Point(mainFormSize.Width - 435, 0);
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
            if (checkBoxShowView != null)
                ActGlobals.oFormActMain.Controls.Remove(checkBoxShowView);

            if (Settings != null)
                Settings.Save();

            if (TimelineView != null)
                TimelineView.Close();

            if (Controller != null)
                Controller.Stop();

            if (StatusText != null)
                StatusText.Text = "Plugin Exited m(_ _)m";
        }
    }
}
