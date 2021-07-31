﻿namespace RJCP.Diagnostics.CpuIdWin.Controls
{
    using System;
    using System.Windows.Forms;
    using static Native.User32;
    using static Native.UxTheme;

    public class ThemeTreeView : TreeView
    {
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETEXTENDEDSTYLE = TV_FIRST + 44;
        private const int TVS_EX_DOUBLEBUFFER = 4;
        private const int TVS_EX_AUTOHSCROLL = 0x20;
        private const int TVS_EX_FADEINOUTEXPANDOS = 0x40;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (!DesignMode && Environment.OSVersion.Version.Major >= 6) {
                const int styleMask = TVS_EX_DOUBLEBUFFER + TVS_EX_AUTOHSCROLL + TVS_EX_FADEINOUTEXPANDOS;
                const int style = TVS_EX_DOUBLEBUFFER + TVS_EX_AUTOHSCROLL;

                int hResult = SetWindowTheme(Handle, "explorer", null);
                if (hResult == 0) {
                    _ = SendMessage(Handle, TVM_SETEXTENDEDSTYLE, styleMask, style);
                }
            }
        }
    }
}
