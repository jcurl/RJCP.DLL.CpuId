namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("Linux")]
    internal class CpuIdNetFactory : ICpuIdFactory
    {
        private const int MaxCpus = 64;

        public ICpuId Create()
        {
            return Create(0);
        }

        public ICpuId Create(int core)
        {
            if (core < 0 || core >= Environment.ProcessorCount)
                throw new ArgumentOutOfRangeException(nameof(core));

            IEnumerable<CpuIdRegister> registers;
            using (CpuIdNetRegisters.Pin(core)) {
                // This constructor doesn't pin to a particular thread. This allows the enumeration of the registers to
                // be done without trying to pin/unpin per CPUID register which would be very slow.
                ICpuRegisters local = new CpuIdNetRegisters();
                registers = CpuIdX86.QueryCpu(local);
            }

            ICpuRegisters cpu = new CpuIdNetRegisters(core);
            return CpuIdX86.CreateCpuIdX86(cpu, registers);
        }

        public IEnumerable<ICpuId> CreateAll()
        {
            List<ICpuId> ids = new();

            int cpus = Math.Min(MaxCpus, Environment.ProcessorCount);
            for (int core = 0; core < cpus; core++) {
                ids.Add(Create(core));
            }
            return ids;
        }
    }
}
