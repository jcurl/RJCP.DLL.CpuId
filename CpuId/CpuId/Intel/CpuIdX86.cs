namespace RJCP.Diagnostics.CpuId.Intel
{
    internal static class CpuIdX86
    {
        private const int VendorIdFunction = 0;

        public static ICpuIdX86 CreateCpuIdX86(ICpuRegisters cpuRegisters)
        {
            ICpuIdX86 x86cpu;

            ICpuRegisters cpu = new CpuRegisters(cpuRegisters);

            switch (GetVendorId(cpu)) {
            case "GenuineIntel":
                try {
                    x86cpu = new GenuineIntelCpu(cpu);
                } catch {
                    if (!cpu.IsOnline) throw;
                    x86cpu = new GenericIntelCpu(cpu);
                }
                break;
            case "AuthenticAMD":
                try {
                    x86cpu = new AuthenticAmdCpu(cpu);
                } catch {
                    if (!cpu.IsOnline) throw;
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

        public static string GetVendorId(ICpuRegisters registers)
        {
            CpuIdRegister vendorFunction = registers.GetCpuId(VendorIdFunction, 0);
            if (vendorFunction is null) return string.Empty;

            char[] vendorId = new char[12];
            int ebx = vendorFunction.Result[1];
            int ecx = vendorFunction.Result[2];
            int edx = vendorFunction.Result[3];

            vendorId[0] = (char)(ebx & 0xFF);
            vendorId[1] = (char)((ebx >> 8) & 0xFF);
            vendorId[2] = (char)((ebx >> 16) & 0xFF);
            vendorId[3] = (char)((ebx >> 24) & 0xFF);
            vendorId[4] = (char)(edx & 0xFF);
            vendorId[5] = (char)((edx >> 8) & 0xFF);
            vendorId[6] = (char)((edx >> 16) & 0xFF);
            vendorId[7] = (char)((edx >> 24) & 0xFF);
            vendorId[8] = (char)(ecx & 0xFF);
            vendorId[9] = (char)((ecx >> 8) & 0xFF);
            vendorId[10] = (char)((ecx >> 16) & 0xFF);
            vendorId[11] = (char)((ecx >> 24) & 0xFF);
            return new string(vendorId);
        }
    }
}
