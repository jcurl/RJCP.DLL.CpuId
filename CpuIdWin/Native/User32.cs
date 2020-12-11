namespace RJCP.Diagnostics.CpuIdWin.Native
{
    using System;
    using System.Runtime.InteropServices;

    internal static class User32
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
    }
}
