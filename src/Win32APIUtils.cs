using System;
using System.Runtime.InteropServices;

namespace ACTTimeline
{
    class Win32APIUtils
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int GWL_EXSTYLE = (-20);

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void DragMove(IntPtr handle)
        {
            ReleaseCapture();
            SendMessage(handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        public static void SetWindow_EX_TRANSPARENT(IntPtr handle, bool value)
        {
            int origStyle = GetWindowLong(handle, GWL_EXSTYLE);

            int style;
            if (value)
                style = origStyle | WS_EX_TRANSPARENT;
            else
                style = origStyle & ~WS_EX_TRANSPARENT;

            SetWindowLong(handle, GWL_EXSTYLE, style);
        }
    }
}
