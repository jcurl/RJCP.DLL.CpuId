namespace RJCP.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Versioning;
    using CpuId;
    using CpuId.Intel;
    using NUnit.Framework;

    [TestFixture]
    public class CpuIdTest
    {
        [Test]
        [Platform("Win")]
        [SupportedOSPlatform("windows")]
        public void FirstCpuId()
        {
            CpuIdFactory factory = new();
            ICpuIdX86 cpu = factory.Create() as ICpuIdX86;
            Assert.That(cpu, Is.Not.Null);
            DumpCpu(cpu);

            Assert.That(cpu.Topology.CoreTopology.IsReadOnly, Is.True);
        }

        [Test]
        [Platform("Win")]
        [SupportedOSPlatform("windows")]
        public void CpuIdIndividualCores()
        {
            CpuIdFactory factory = new();
            HashSet<int> apics = new();
            for (int i = 0; i < Environment.ProcessorCount; i++) {
                ICpuIdX86 cpu = factory.Create(i) as ICpuIdX86;
                Assert.That(cpu, Is.Not.Null);
                DumpCpu(cpu);

                // Each core should have a unique APIC ID on Intel platforms.
                int apic = cpu.Registers.GetCpuId(1, 0).Result[1] >> 24;
                Assert.That(apics, Does.Not.Contain(apic));
                apics.Add(apic);

                Assert.That(cpu.Topology.CoreTopology.IsReadOnly, Is.True);
            }
        }

        [Test]
        [Platform("Win")]
        [SupportedOSPlatform("windows")]
        public void InvalidCpuCore()
        {
            CpuIdFactory factory = new();

            Assert.That(() => {
                _ = factory.Create(-1);
            }, Throws.TypeOf<ArgumentOutOfRangeException>());

            Assert.That(() => {
                _ = factory.Create(64);
            }, Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [Platform("Win")]
        [SupportedOSPlatform("windows")]
        public void AllCpuId()
        {
            CpuIdFactory factory = new();
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

            Assert.That(cpuNumber, Is.EqualTo(Environment.ProcessorCount));
        }

        [Test]
        [Platform("Win")]
        [SupportedOSPlatform("windows")]
        public void GetRegister()
        {
            CpuIdFactory factory = new();

            int core = Environment.ProcessorCount - 1;
            ICpuIdX86 cpu = factory.Create(core) as ICpuIdX86;
            Assert.That(cpu, Is.Not.Null);

            // The test exercises getting the CPUID directly, hopefully from a value that is not cached by CpuRegisters.
            // This will cause the underlying implementation of "cpuid" to get called for that specific function and
            // subfunction. We should have pinning here. To test if this really works, will need to single step.
            CpuIdRegister reg = cpu.Registers.GetCpuId(0x60000000, 0);
            Assert.That(reg.Function, Is.EqualTo(0x60000000));
            Assert.That(reg.SubFunction, Is.EqualTo(0));
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
                    cpu.Features[feature].Value ? "X" : "-", feature, cpu.Features[feature].Description);
            }
        }
    }
}
