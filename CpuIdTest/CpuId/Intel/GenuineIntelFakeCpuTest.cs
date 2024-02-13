﻿namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.IO;
    using CodeQuality.NUnitExtensions;
    using NUnit.Framework;

    class GenuineIntelFakeCpuTest
    {
        private readonly static string TestResources = Path.Combine(Deploy.TestDirectory, "TestResources", "FakeIntel");

        private static ICpuIdX86 GetCpu(string fileName)
        {
            string fullPath = Path.Combine(TestResources, fileName);

            CpuIdXmlFactory factory = new();
            ICpuId cpu = factory.Create(fullPath);
            ICpuIdX86 x86cpu = cpu as ICpuIdX86;
            Assert.That(x86cpu, Is.Not.Null);
            Assert.That(x86cpu.Topology.CoreTopology.IsReadOnly, Is.True);
            return x86cpu;
        }

        [Test]
        public void NoFunctions()
        {
            ICpuIdX86 cpu = GetCpu("nofunctions.xml");
            Assert.That(cpu.VendorId, Is.EqualTo("GenuineIntel"));
            Assert.That(cpu.ProcessorSignature, Is.EqualTo(0));
            Assert.That(cpu.Model, Is.EqualTo(0));
            Assert.That(cpu.Family, Is.EqualTo(0));
            Assert.That(cpu.ProcessorType, Is.EqualTo(0));
            Assert.That(cpu.Stepping, Is.EqualTo(0));
            Assert.That(cpu.Features["FPU"].Value, Is.False);
        }

        [Test]
        public void NoFunctionsNoFunction1Query()
        {
            ICpuIdX86 cpu = GetCpu("nofunctions1.xml");
            Assert.That(cpu.VendorId, Is.EqualTo("GenuineIntel"));

            // Even though the file contains CPUID[01h], it shouldn't be queried because 0h says it doesn't exist.
            Assert.That(cpu.ProcessorSignature, Is.EqualTo(0));
            Assert.That(cpu.Model, Is.EqualTo(0));
            Assert.That(cpu.Family, Is.EqualTo(0));
            Assert.That(cpu.ProcessorType, Is.EqualTo(0));
            Assert.That(cpu.Stepping, Is.EqualTo(0));
            Assert.That(cpu.Features["FPU"].Value, Is.False);
        }

        [Test]
        public void I486_DX4()
        {
            ICpuIdX86 cpu = GetCpu("i486dx4.xml");
            Assert.That(cpu.VendorId, Is.EqualTo("GenuineIntel"));

            // Even though the file contains CPUID[01h], it shouldn't be queried because 0h says it doesn't exist.
            Assert.That(cpu.ProcessorSignature, Is.EqualTo(0x482));
            Assert.That(cpu.Model, Is.EqualTo(8));
            Assert.That(cpu.Family, Is.EqualTo(4));
            Assert.That(cpu.ProcessorType, Is.EqualTo(0));
            Assert.That(cpu.Stepping, Is.EqualTo(2));
            Assert.That(cpu.Features["FPU"].Value, Is.True);

            GenuineIntelCpu intelCpu = cpu as GenuineIntelCpu;
            Assert.That(intelCpu, Is.Not.Null, "Expected Intel CPU");
            Assert.That(intelCpu.Description, Is.EqualTo("IntelDX4(TM)"));
        }
    }
}
