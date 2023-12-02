namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Native;

    internal class X86CpuIdFactory : X86CpuIdFactoryBase
    {
        private const int MaxCpuLeaves = 256;
        private const int MaxCpus = 64;

        public override ICpuId Create()
        {
            return Create(GetLocalCpuNode());
        }

        public override IEnumerable<ICpuId> CreateAll()
        {
            IEnumerable<BasicCpu> cpus = GetLocalCpuNodes();
            List<ICpuId> ids = new List<ICpuId>();
            foreach (BasicCpu cpu in cpus) {
                ids.Add(Create(cpu));
            }
            return ids;
        }

        private static unsafe BasicCpu GetLocalCpuNode()
        {
            if (CpuIdLib.hascpuid() == 0)
                throw new PlatformNotSupportedException("CPUID instruction not supported");

            CpuIdLib.CpuIdInfo[] data = new CpuIdLib.CpuIdInfo[MaxCpuLeaves];
            int r;
            fixed (CpuIdLib.CpuIdInfo* cpuidptr = &data[0]) {
                r = CpuIdLib.iddump(cpuidptr, Marshal.SizeOf(data[0]) * data.Length);
            }
            return new BasicCpu(data, 0, r);
        }

        private static unsafe List<BasicCpu> GetLocalCpuNodes()
        {
            if (CpuIdLib.hascpuid() == 0)
                throw new PlatformNotSupportedException("CPUID instruction not supported");

            CpuIdLib.CpuIdInfo[] data = new CpuIdLib.CpuIdInfo[MaxCpuLeaves * MaxCpus];
            int r;
            fixed (CpuIdLib.CpuIdInfo* cpuidptr = &data[0]) {
                r = CpuIdLib.iddumpall(cpuidptr, Marshal.SizeOf(data[0]) * data.Length);
            }

            // Each CPU has the first element with EAX=0xFFFFFFFF and the CPU number as ECX. This isn't captured by the
            // CPUID instruction, but a part of the library to allow separating the CPU information
            List<BasicCpu> cpus = new List<BasicCpu>();
            int cpustart = 0;
            for (int i = 0; i < r; i++) {
                if (data[i].veax == -1) {
                    // Describes the start of a CPU node.
                    if (i - cpustart > 0) {
                        // Process the data that we had.
                        BasicCpu cpu = new BasicCpu(data, cpustart + 1, i - cpustart - 1);
                        cpus.Add(cpu);
                    }
                    cpustart = i;
                }
            }
            if (r - cpustart > 0) {
                // Process the data that we had.
                BasicCpu cpu = new BasicCpu(data, cpustart + 1, r - cpustart - 1);
                cpus.Add(cpu);
            }

            return cpus;
        }
    }
}
