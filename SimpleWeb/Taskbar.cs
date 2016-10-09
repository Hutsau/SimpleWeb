using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SimpleWeb
{
    public static class Taskbar
    {
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string ClassName, string WindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void Show()
        {
            IntPtr HWnd;

            if ((HWnd = FindWindow("Shell_TrayWnd", null)) != IntPtr.Zero)
                ShowWindow(HWnd, SW_SHOW);

            if ((HWnd = FindWindow("Button", null)) != IntPtr.Zero)
                ShowWindow(HWnd, SW_SHOW);
        }

        public static void Hide()
        {
            IntPtr HWnd;

            if ((HWnd = FindWindow("Shell_TrayWnd", null)) != IntPtr.Zero)
                ShowWindow(HWnd, SW_HIDE);

            if ((HWnd = FindWindow("Button", null)) != IntPtr.Zero)
                ShowWindow(HWnd, SW_HIDE);
        }
    }
}
