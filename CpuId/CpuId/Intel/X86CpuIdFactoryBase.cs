namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.Collections.Generic;

    internal abstract class X86CpuIdFactoryBase : ICpuIdFactory
    {
        public abstract ICpuId Create();

        public abstract IEnumerable<ICpuId> CreateAll();

        protected ICpuId Create(BasicCpu cpu)
        {
            switch (cpu.VendorId) {
            case "GenuineIntel":
                return new GenuineIntelCpu(cpu);
            case "AuthenticAMD":
                return new AuthenticAmdCpu(cpu);
            default:
                return new GenericIntelCpu(cpu);
            }
        }
    }
}
