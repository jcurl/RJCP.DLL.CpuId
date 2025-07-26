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
        private const int MaxCpuLeaves = 256;
        private const int MaxCpus = 64;

        public ICpuId Create()
        {
            return Create(0);
        }

        public ICpuId Create(int core)
        {
            if (core < 0 || core >= Environment.ProcessorCount)
                throw new ArgumentOutOfRangeException(nameof(core));

            LoadLibrary();
            return UnsafeCreate(core);
        }

        public IEnumerable<ICpuId> CreateAll()
        {
            LoadLibrary();
            return UnsafeCreateAll();
        }

        private static unsafe ICpuId UnsafeCreate(int core)
        {
            List<CpuIdRegister> registerList = new();

            // Get a dump of a specific core. The library handles thread pinning for us.
            CpuIdLib.CpuIdInfo[] data = new CpuIdLib.CpuIdInfo[MaxCpuLeaves];
            int r;
            fixed (CpuIdLib.CpuIdInfo* cpuidptr = &data[0]) {
                r = CpuIdLib.iddumponcore(cpuidptr, Marshal.SizeOf(data[0]) * data.Length, core);
            }
            if (r <= 0) return null;

            for (int i = 0; i < r; i++) {
                CpuIdRegister result = new(data[i].veax, data[i].vecx,
                    new int[] { data[i].peax, data[i].pebx, data[i].pecx, data[i].pedx });
                registerList.Add(result);
            }

            ICpuRegisters registers = new CpuIdLibRegisters(core);
            return CpuIdX86.CreateCpuIdX86(registers, registerList);
        }

        private static unsafe IEnumerable<ICpuId> UnsafeCreateAll()
        {
            // Get a dump of all cores. The library handles setting up each thread and querying all the CPU ID
            // information.
            CpuIdLib.CpuIdInfo[] data = new CpuIdLib.CpuIdInfo[MaxCpuLeaves * MaxCpus];
            int r;
            fixed (CpuIdLib.CpuIdInfo* cpuidptr = &data[0]) {
                r = CpuIdLib.iddumpall(cpuidptr, Marshal.SizeOf(data[0]) * data.Length);
            }
            if (r <= 0) return null;

            // Each CPU has the first element with EAX=0xFFFFFFFF and the CPU number as ECX. This isn't captured by the
            // CPUID instruction, but a part of the library to allow separating the CPU information
            List<ICpuId> cpus = new();
            int cpustart = 0;
            for (int i = 0; i < r; i++) {
                if (data[i].veax == -1) {
                    // Describes the start of a CPU node.
                    if (i - cpustart > 0) {
                        // Process the data that we had.
                        ICpuId cpu = CreateCpuIdX86(cpus.Count, data, cpustart + 1, i - cpustart - 1);
                        cpus.Add(cpu);
                    }
                    cpustart = i;
                }
            }
            if (r - cpustart > 0) {
                // Process the data that we had.
                ICpuId cpu = CreateCpuIdX86(cpus.Count, data, cpustart + 1, r - cpustart - 1);
                cpus.Add(cpu);
            }

            return cpus;
        }

        private static ICpuId CreateCpuIdX86(int core, CpuIdLib.CpuIdInfo[] data, int offset, int length)
        {
            ThrowHelper.ThrowIfArrayOutOfBounds(data, offset, length);

            List<CpuIdRegister> registerList = new();
            for (int i = 0; i < length; i++) {
                int r = offset + i;
                CpuIdRegister result = new(data[r].veax, data[r].vecx,
                    new int[] { data[r].peax, data[r].pebx, data[r].pecx, data[r].pedx });
                registerList.Add(result);
            }

            ICpuRegisters registers = new CpuIdLibRegisters(core);
            return CpuIdX86.CreateCpuIdX86(registers, registerList);
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
