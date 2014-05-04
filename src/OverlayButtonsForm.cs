using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ACTTimeline
{
    public partial class OverlayButtonsForm : Form
    {
        TimelineController controller;

        public OverlayButtonsForm(TimelineController controller_)
        {
            controller = controller_;

            InitializeComponent();

            this.ShowInTaskbar = false;

            // Force set small window size below OS minimum.
            Win32APIUtils.SetWindowSize(Handle, 55, 20);

            Win32APIUtils.SetWS_EX_NOACTIVATE(Handle, true);

            controller.PausedUpdate += controller_PausedUpdate;
            controller_PausedUpdate(null, EventArgs.Empty);
        }

        void controller_PausedUpdate(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { controller_PausedUpdate(sender, e); }));
                return;
            }

            buttonPlayPause.Text = controller.Paused ? "▷" : "■";
        }

        private void buttonRewind_Click(object sender, EventArgs e)
        {
            controller.CurrentTime = 0;
        }

        private void buttonPlayPause_Click(object sender, EventArgs e)
        {
            controller.Paused = !controller.Paused;
        }
    }
}
