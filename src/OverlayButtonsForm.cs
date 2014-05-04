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
        public OverlayButtonsForm()
        {
            InitializeComponent();
            Win32APIUtils.SetWindowSize(Handle, 55, 20);
        }
    }
}
