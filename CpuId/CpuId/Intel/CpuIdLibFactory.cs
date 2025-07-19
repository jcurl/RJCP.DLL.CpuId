namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using Native.Win32;

    [SupportedOSPlatform("windows")]
    internal class CpuIdLibFactory : ICpuIdFactory
    {
        private const int MaxCpus = 64;

        public ICpuId Create()
        {
            LoadLibrary();
            return CpuIdX86.CreateCpuIdX86(new CpuIdLibRegisters());
        }

        public IEnumerable<ICpuId> CreateAll()
        {
            LoadLibrary();
            IEnumerable<ICpuRegisters> cpus = GetLocalCpuNodes();
            List<ICpuId> ids = new();
            foreach (ICpuRegisters cpu in cpus) {
                ids.Add(CpuIdX86.CreateCpuIdX86(cpu));
            }
            return ids;
        }

        private static unsafe List<ICpuRegisters> GetLocalCpuNodes()
        {
            if (CpuIdLib.hascpuid() == 0)
                throw new PlatformNotSupportedException("CPUID instruction not supported");

            CpuIdLib.CpuIdInfo[] data = new CpuIdLib.CpuIdInfo[CpuIdLibRegisters.MaxCpuLeaves * MaxCpus];
            int r;
            fixed (CpuIdLib.CpuIdInfo* cpuidptr = &data[0]) {
                r = CpuIdLib.iddumpall(cpuidptr, Marshal.SizeOf(data[0]) * data.Length);
            }

            // Each CPU has the first element with EAX=0xFFFFFFFF and the CPU number as ECX. This isn't captured by the
            // CPUID instruction, but a part of the library to allow separating the CPU information
            List<ICpuRegisters> cpus = new();
            int cpustart = 0;
            for (int i = 0; i < r; i++) {
                if (data[i].veax == -1) {
                    // Describes the start of a CPU node.
                    if (i - cpustart > 0) {
                        // Process the data that we had.
                        ICpuRegisters cpu = new CpuIdLibRegisters(data, cpustart + 1, i - cpustart - 1);
                        cpus.Add(cpu);
                    }
                    cpustart = i;
                }
            }
            if (r - cpustart > 0) {
                // Process the data that we had.
                ICpuRegisters cpu = new CpuIdLibRegisters(data, cpustart + 1, r - cpustart - 1);
                cpus.Add(cpu);
            }

            return cpus;
        }

        private static SafeLibraryHandle m_CpuIdHandle;

        private static void LoadLibrary()
        {
            m_CpuIdHandle ??= Win32.LoadLibrary<CpuIdLibFactory>("cpuid.dll");
            if (m_CpuIdHandle.IsInvalid)
                throw new PlatformNotSupportedException("Cannot load platform specific libraries");
        }
    }
}
