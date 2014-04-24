using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advanced_Combat_Tracker;
using System.Windows.Forms;
using System.IO;
using System.Xml;

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

    class PluginSettings : SettingsSerializer
    {
        private string settingsFile;

        public PluginSettings(ACTPlugin actPlugin)
            : base(actPlugin)
        {
            settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\ACTTimeline.config.xml");
            if (File.Exists(settingsFile))
            {
                FileStream fs = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
               
                XmlTextReader reader = new XmlTextReader(fs);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "SettingsSerializer")
                    {
                        ImportFromXml(reader);
                        break;
                    }
                }
                reader.Close();
            }
        }

        public void Save()
        {
            FileStream stream = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 1;
            writer.IndentChar = '\t';
            writer.WriteStartDocument(true);
                writer.WriteStartElement("Config");
                    writer.WriteStartElement("SettingsSerializer");
                        ExportToXml(writer);
                    writer.WriteEndElement();
                writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
    }
}
