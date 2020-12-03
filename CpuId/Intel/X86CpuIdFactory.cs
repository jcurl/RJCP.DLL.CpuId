namespace RJCP.Diagnostics.Intel
{
    using System.Xml;

    internal class X86CpuIdFactory : ICpuIdFactory
    {
        public ICpuId Create()
        {
            return Create(new BasicCpu());
        }

        public ICpuId Create(XmlNode node)
        {
            return Create(new BasicCpu(node));
        }

        private ICpuId Create(BasicCpu cpu)
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
