namespace RJCP.Diagnostics
{
    using System;
    using Intel;

    public static class Program
    {
        public static int Main()
        {
            ICpuIdFactory cpuFactory = new CpuIdFactory();
            ICpuId cpu = cpuFactory.Create();

            if (cpu is GenericIntelCpuBase x86cpu) {
                string fileName = string.Format("{0}.xml", Environment.MachineName);
                try {
                    x86cpu.Save(fileName);
                    Console.WriteLine("Wrote output to: {0}", fileName);
                    return 0;
                } catch (Exception ex) {
                    Console.WriteLine("Error writing to: {0}", fileName);
                    Console.WriteLine(" -> {0}", ex.Message);
                    return 1;
                }
            }
            return 2;
        }
    }
}
