namespace RJCP.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using CpuId;

    public static class Program
    {
        public static int Main()
        {
            ICpuId firstCpu;
            IEnumerable<ICpuId> cpus;
            try {
                CpuIdFactory cpuFactory = new CpuIdFactory();
                firstCpu = cpuFactory.Create();
                cpus = cpuFactory.CreateAll();
            } catch (PlatformNotSupportedException) {
                Console.WriteLine("This platform is not supported");
                return 1;
            }

            string fileName;
            if (firstCpu is CpuId.Intel.ICpuIdX86 x86cpu) {
                if (string.IsNullOrWhiteSpace(firstCpu.Description)) {
                    fileName = string.Format("{0}{1:X07} ({2}).xml", firstCpu.VendorId, x86cpu.ProcessorSignature, Environment.MachineName);
                } else {
                    fileName = string.Format("{0}{1:X07} ({2}, {3}).xml", firstCpu.VendorId, x86cpu.ProcessorSignature, firstCpu.Description, Environment.MachineName);
                }
            } else {
                if (string.IsNullOrWhiteSpace(firstCpu.Description)) {
                    fileName = string.Format("{0} ({1}).xml", firstCpu.VendorId, Environment.MachineName);
                } else {
                    fileName = string.Format("{0} ({1}, {2}).xml", firstCpu.VendorId, firstCpu.Description, Environment.MachineName);
                }
            }

            try {
                CpuIdXmlFactory.Save(fileName, cpus);
                Console.WriteLine("Wrote output to: {0}", fileName);
                return 0;
            } catch (Exception ex) {
                Console.WriteLine("Error writing to: {0}", fileName);
                Console.WriteLine(" -> {0}", ex.Message);
                return 1;
            }
        }
    }
}
