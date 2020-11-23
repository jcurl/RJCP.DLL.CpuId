namespace RJCP.Diagnostics.Native
{
    using System.Runtime.InteropServices;

    internal partial class CpuIdLib
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct CpuIdInfo
        {
            public int veax;
            public int vecx;
            public int peax;
            public int pebx;
            public int pecx;
            public int pedx;
        }
    }
}
