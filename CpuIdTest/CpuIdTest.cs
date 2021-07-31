namespace RJCP.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using CpuId;
    using NUnit.Framework;

    [TestFixture]
    public class CpuIdTest
    {
        [Test]
        public void CurrentCpuId()
        {
            ICpuIdFactory factory = new CpuIdFactory();
            ICpuId cpu = factory.Create();
            Assert.That(cpu, Is.Not.Null);
            DumpCpu(cpu);

            if (cpu is CpuId.Intel.ICpuIdX86 x86cpu) {
                Assert.That(x86cpu.Topology.CoreTopology.IsReadOnly, Is.True);
            }
        }

        [Test]
        public void AllCpuId()
        {
            ICpuIdFactory factory = new CpuIdFactory();
            IEnumerable<ICpuId> cpus = factory.CreateAll();

            Assert.That(cpus, Is.Not.Null);

            int cpuNumber = 0;
            foreach (ICpuId cpu in cpus) {
                Console.WriteLine("==> CPU #{0}", cpuNumber);
                DumpCpu(cpu);
                cpuNumber++;

                if (cpu is CpuId.Intel.ICpuIdX86 x86cpu) {
                    Assert.That(x86cpu.Topology.CoreTopology.IsReadOnly, Is.True);
                }
            }
        }

        private static void DumpCpu(ICpuId cpu)
        {
            Console.WriteLine("CPU Vendor: {0}", cpu.CpuVendor);
            Console.WriteLine("CPU Vendor Id: {0}", cpu.VendorId);
            Console.WriteLine("CPU Description: {0}", cpu.Description);

            if (cpu is CpuId.Intel.ICpuIdX86 x86cpu) {
                Console.WriteLine("x86: Brand: {0}", x86cpu.BrandString);
                Console.WriteLine("x86: Signature: {0:X}h", x86cpu.ProcessorSignature);
                Console.WriteLine("x86: Family: {0:X}h", x86cpu.Family);
                Console.WriteLine("x86: Model: {0:X}h", x86cpu.Model);
                Console.WriteLine("x86: Type: {0}", x86cpu.ProcessorType);
                Console.WriteLine("x86: Stepping: {0:X}h", x86cpu.Stepping);

                foreach (var reg in x86cpu.Registers) {
                    Console.WriteLine("{0:X8} {1:X8}: {2:X8} {3:X8} {4:X8} {5:X8}",
                        reg.Function, reg.SubFunction, reg.Result[0], reg.Result[1], reg.Result[2], reg.Result[3]);
                }
            }

            foreach (string feature in cpu.Features) {
                Console.WriteLine("Feature: [{0}] {1} ({2})",
                    cpu.Features[feature] ? "X" : "-", feature, cpu.Features.Description(feature));
            }
        }
    }
}
