namespace RJCP.Diagnostics.Native
{
    using System.Runtime.InteropServices;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    internal static partial class CpuIdLib
    {
        [DllImport("cpuid.dll", ExactSpelling = true)]
        public static extern int hascpuid();

        [DllImport("cpuid.dll", ExactSpelling = true)]
        public static extern int cpuid(int eax, int ecx, out int peax, out int pebx, out int pecx, out int pedx);

        [DllImport("cpuid.dll", ExactSpelling = true)]
        public static unsafe extern int iddump(CpuIdInfo* s, int len);

        [DllImport("cpuid.dll", ExactSpelling = true)]
        public static unsafe extern int iddumpall(CpuIdInfo* s, int len);
    }
}
