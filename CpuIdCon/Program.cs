namespace RJCP.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CpuId;

    public static class Program
    {
        public static int Main()
        {
            ICpuId firstCpu;
            IEnumerable<ICpuId> cpus;
            try {
                CpuIdFactory cpuFactory = new();
                firstCpu = cpuFactory.Create();
                cpus = cpuFactory.CreateAll();
            } catch (PlatformNotSupportedException) {
                Console.WriteLine("This platform is not supported");
                return 1;
            }

            bool written;
            string fileName = string.Empty;
            try {
                if (firstCpu is CpuId.Intel.ICpuIdX86 x86cpu) {
                    fileName = string.IsNullOrWhiteSpace(firstCpu.Description)
                        ? string.Format("{0}{1:X07} ({2}).xml", firstCpu.VendorId, x86cpu.ProcessorSignature, Environment.MachineName)
                        : string.Format("{0}{1:X07} ({2}, {3}).xml", firstCpu.VendorId, x86cpu.ProcessorSignature, firstCpu.Description, Environment.MachineName);
                    written = Write(cpus, fileName);
                    if (!written) {
                        fileName = string.Format("{0}{1:X07} ({2}).xml", firstCpu.VendorId, x86cpu.ProcessorSignature, Environment.MachineName);
                        written = Write(cpus, fileName);
                    }
                } else {
                    fileName = string.IsNullOrWhiteSpace(firstCpu.Description)
                        ? string.Format("{0} ({1}).xml", firstCpu.VendorId, Environment.MachineName)
                        : string.Format("{0} ({1}, {2}).xml", firstCpu.VendorId, firstCpu.Description, Environment.MachineName);
                    written = Write(cpus, fileName);
                    if (!written) {
                        fileName = string.Format("{0} ({1}).xml", firstCpu.VendorId, Environment.MachineName);
                        written = Write(cpus, fileName);
                    }
                }
                if (!written) {
                    written = Write(cpus, "CPU.xml");
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error writing to {fileName}.");
                Console.WriteLine($"{ex}");
                return 1;
            }

            if (!written) {
                using (Stream stream = Console.OpenStandardOutput()) {
                    CpuIdXmlFactory.Save(stream, cpus);
                }
                Console.WriteLine("");
                Console.WriteLine("DONE.");
            }
            return 0;
        }

        private static bool Write(IEnumerable<ICpuId> cpus, string fileName)
        {
            try {
                CpuIdXmlFactory.Save(fileName, cpus);
                Console.WriteLine("Wrote output to: {0}", fileName);
                return true;
            } catch (PathTooLongException) {
                return false;
            } catch (FileNotFoundException) {
                return false;
            } catch (DirectoryNotFoundException) {
                return false;
            }
        }
    }
}
