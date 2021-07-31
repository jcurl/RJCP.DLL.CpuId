namespace RJCP.Diagnostics.CpuIdWin.Controls
{
    using System;
    using System.Windows.Forms;
    using static Native.User32;
    using static Native.UxTheme;

    public class ThemeListView : ListView
    {
        private const int LV_FIRST = 0x1000;
        private const int LVM_SETEXTENDEDLISTVIEWSTYLE = LV_FIRST + 54;
        private const int LVS_EX_DOUBLEBUFFER = 0x10000;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (!DesignMode && Environment.OSVersion.Version.Major >= 6) {
                const int styleMask = LVS_EX_DOUBLEBUFFER;
                const int style = LVS_EX_DOUBLEBUFFER;

                int hResult = SetWindowTheme(Handle, "explorer", null);
                if (hResult == 0) {
                    _ = SendMessage(Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, styleMask, style);
                }
            }
        }
    }
}
