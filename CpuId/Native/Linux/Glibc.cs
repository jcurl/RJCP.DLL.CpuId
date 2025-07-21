namespace RJCP.Diagnostics.Native.Linux
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    [SupportedOSPlatform("Linux")]
    internal static class Glibc
    {
        private const int CPU_SETSIZE = 1024;
        private const int __CPU_BITS = 64;

        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Linux name")]
        public unsafe struct cpu_set_t
        {
            public fixed ulong bits[CPU_SETSIZE / __CPU_BITS];
        }

        [DllImport("libc", SetLastError = true, ExactSpelling = true)]
        private static extern int sched_setaffinity(int pid, IntPtr cpusetsize, ref cpu_set_t mask);

        [DllImport("libc", SetLastError = true, ExactSpelling = true)]
        private static extern int sched_getaffinity(int pid, IntPtr cpusetsize, ref cpu_set_t mask);

        public static unsafe void CPU_ZERO(ref cpu_set_t set)
        {
            for (int i = 0; i < CPU_SETSIZE / __CPU_BITS; i++) {
                set.bits[i] = 0;
            }
        }

        public static unsafe void CPU_SET(int cpu, ref cpu_set_t set)
        {
            int index = cpu / __CPU_BITS;
            int offset = cpu % __CPU_BITS;
            set.bits[index] |= 1UL << offset;
        }

        public static bool SetThreadAffinity(int core)
        {
            cpu_set_t set = new();
            CPU_ZERO(ref set);
            CPU_SET(core, ref set);

            int result = sched_setaffinity(0, (IntPtr)Marshal.SizeOf<cpu_set_t>(), ref set);
            return result == 0;
        }

        public static bool SetThreadAffinity(cpu_set_t set)
        {
            int result = sched_setaffinity(0, (IntPtr)Marshal.SizeOf<cpu_set_t>(), ref set);
            return result == 0;
        }

        public static bool GetThreadAffinity(out cpu_set_t set)
        {
            set = new cpu_set_t();
            CPU_ZERO(ref set);
            int result = sched_getaffinity(0, (IntPtr)Marshal.SizeOf<cpu_set_t>(), ref set);
            return result == 0;
        }
    }
}
