namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    internal class CpuIdNetFactory : ICpuIdFactory
    {
        private const int MaxCpus = 64;

        public ICpuId Create()
        {
            ICpuRegisters registers = new CpuIdNetRegisters();
            if (!registers.IsOnline) return null;

            return CpuIdX86.CreateCpuIdX86(registers);
        }

        public ICpuId Create(int core)
        {
            if (core < 0 || core >= Environment.ProcessorCount)
                throw new ArgumentOutOfRangeException(nameof(core));

            ICpuRegisters registers = new CpuIdNetRegisters(core);
            if (!registers.IsOnline) return null;

            return CpuIdX86.CreateCpuIdX86(registers);
        }

        public IEnumerable<ICpuId> CreateAll()
        {
            List<ICpuId> ids = new();

            int cpus = Math.Min(MaxCpus, Environment.ProcessorCount);
            for (int core = 0; core < cpus; core++) {
                ICpuRegisters registers = new CpuIdNetRegisters(core);
                ids.Add(CpuIdX86.CreateCpuIdX86(registers));
            }
            return ids;
        }
    }
}
