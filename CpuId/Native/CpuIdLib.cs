namespace RJCP.Diagnostics.Native
{
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    [SupportedOSPlatform("windows")]
    internal static partial class CpuIdLib
    {
        [DllImport("cpuid.dll")]
        public static extern int hascpuid();

        [DllImport("cpuid.dll")]
        public static extern int cpuid(int eax, int ecx, out int peax, out int pebx, out int pecx, out int pedx);

        [DllImport("cpuid.dll")]
        public static unsafe extern int iddump(CpuIdInfo* s, int len);

        [DllImport("cpuid.dll")]
        public static unsafe extern int iddumponcore(CpuIdInfo* s, int len, int core);

        [DllImport("cpuid.dll")]
        public static unsafe extern int iddumpall(CpuIdInfo* s, int len);
    }
}
