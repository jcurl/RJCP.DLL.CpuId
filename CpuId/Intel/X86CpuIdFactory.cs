namespace RJCP.Diagnostics.Intel
{
    internal class X86CpuIdFactory : ICpuIdFactory
    {
        public ICpuId Create()
        {
            BasicCpu cpu = new BasicCpu();
            switch (cpu.VendorId) {
            case "GenuineIntel":
                return new GenuineIntelCpu(cpu);
            default:
                return new GenericIntelCpu(cpu);
            }
        }
    }
}
