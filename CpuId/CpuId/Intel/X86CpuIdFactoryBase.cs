namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.Collections.Generic;

    internal abstract class X86CpuIdFactoryBase : ICpuIdFactory
    {
        public abstract ICpuId Create();

        public abstract IEnumerable<ICpuId> CreateAll();

        protected ICpuIdX86 Create(BasicCpu cpu)
        {
            ICpuIdX86 x86cpu;

            switch (cpu.VendorId) {
            case "GenuineIntel":
                x86cpu = new GenuineIntelCpu(cpu);
                break;
            case "AuthenticAMD":
                x86cpu = new AuthenticAmdCpu(cpu);
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
