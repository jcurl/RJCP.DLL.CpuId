namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.Collections.Generic;

    internal abstract class X86CpuIdFactoryBase : ICpuIdFactory
    {
        public abstract ICpuId Create();

        public abstract IEnumerable<ICpuId> CreateAll();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Factory Method")]
        protected ICpuIdX86 Create(BasicCpu cpu)
        {
            ICpuIdX86 x86cpu;

            switch (cpu.VendorId) {
            case "GenuineIntel":
                try {
                    x86cpu = new GenuineIntelCpu(cpu);
                } catch {
                    if (!cpu.CpuRegisters.IsOnline) throw;
                    x86cpu = new GenericIntelCpu(cpu);
                }
                break;
            case "AuthenticAMD":
                try {
                    x86cpu = new AuthenticAmdCpu(cpu);
                } catch {
                    if (!cpu.CpuRegisters.IsOnline) throw;
                    x86cpu = new GenericIntelCpu(cpu);
                }
                break;
            default:
                x86cpu = new GenericIntelCpu(cpu);
                break;
            }

            x86cpu.Topology.CoreTopology.IsReadOnly = true;
            x86cpu.Topology.CacheTopology.IsReadOnly = true;
            return x86cpu;
        }
    }
}
