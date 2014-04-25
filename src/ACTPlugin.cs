using Advanced_Combat_Tracker;
using System;
using System.Windows.Forms;

namespace ACTTimeline
{
    class ACTPlugin : IActPluginV1
    {
        public ACTPlugin()
        {
        }

        public TabPage ScreenSpace { get; private set; }
        public Label StatusText { get; private set; }

        public PluginSettings Settings { get; private set; }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            try
            {
                ScreenSpace = pluginScreenSpace;
                StatusText = pluginStatusText;

                Settings = new PluginSettings(this);

                StatusText.Text = "Plugin Started (^^)!";
            }
            catch(Exception e)
            {
                if (StatusText != null)
                    StatusText.Text = "Plugin Init Failed: "+e.Message;
            }
        }
    
        void IActPluginV1.DeInitPlugin()
        {
            Settings.Save();

            StatusText.Text = "Plugin Exited m(_ _)m";
        }
    }
}
