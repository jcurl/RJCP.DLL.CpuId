namespace RJCP.Diagnostics
{
    using System;
    using System.Collections.Generic;

    public static class Program
    {
        public static int Main()
        {
            ICpuIdFactory cpuFactory = new CpuIdFactory();
            IEnumerable<ICpuId> cpus = cpuFactory.CreateAll();

            string fileName = string.Format("{0}.xml", Environment.MachineName);
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
