namespace RJCP.Diagnostics
{
    using System;
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
            Console.WriteLine("CPU Vendor: {0}", cpu.CpuVendor);
            Console.WriteLine("CPU Vendor Id: {0}", cpu.VendorId);
            Console.WriteLine("CPU Description: {0}", cpu.Description);

            if (cpu is Intel.ICpuIdX86 x86cpu) {
                Console.WriteLine("Intel: Signature: {0:X}", x86cpu.ProcessorSignature);
                Console.WriteLine("Intel: Family: {0:X}", x86cpu.Family);
                Console.WriteLine("Intel: Model: {0:X}", x86cpu.Model);
                Console.WriteLine("Intel: Type: {0}", x86cpu.ProcessorType);
                Console.WriteLine("Intel: Stepping: {0:X}", x86cpu.Stepping);
                Console.WriteLine("Intel: APIC Id: {0:X}", x86cpu.ApicId);
                Console.WriteLine("Intel: Max Threads / Package: {0}", x86cpu.ApicMaxThreads);

                foreach (var reg in x86cpu.Registers.Registers) {
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
