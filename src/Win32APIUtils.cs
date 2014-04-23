using System;
using System.Runtime.InteropServices;

namespace hw
{
    class Win32APIUtils
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public static void DragMove(IntPtr handle)
        {
            ReleaseCapture();
            SendMessage(handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
    }
}
