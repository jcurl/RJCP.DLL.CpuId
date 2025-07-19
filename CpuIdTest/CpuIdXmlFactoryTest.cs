namespace RJCP.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CodeQuality.NUnitExtensions;
    using CpuId;
    using NUnit.Framework;

    [TestFixture]
    public class CpuIdXmlFactoryTest
    {
        private readonly static string MultiCpu = Path.Combine(Deploy.TestDirectory, "TestResources", "GenuineIntel", "i7-9700.xml");

        [Test]
        public void IntelCreateFromXmlFileProperty()
        {
            CpuIdXmlFactory factory = new() {
                FileName = MultiCpu
            };
            ICpuId cpu = factory.Create();

            Assert.That(cpu, Is.Not.Null);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-9700 CPU @ 3.00GHz"));
        }

        [Test]
        public void IntelCreateFromXmlConstructorFile()
        {
            CpuIdXmlFactory factory = new(MultiCpu);
            ICpuId cpu = factory.Create();

            Assert.That(cpu, Is.Not.Null);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-9700 CPU @ 3.00GHz"));
        }

        [Test]
        public void IntelCreateFromXmlSpecificCore()
        {
            CpuIdXmlFactory factory = new(MultiCpu);

            // This test file has exactly 8 cores.
            for (int i = 0; i < 8; i++) {
                CpuId.Intel.ICpuIdX86 cpu = factory.Create(i) as CpuId.Intel.ICpuIdX86;

                Assert.That(cpu, Is.Not.Null);
                Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-9700 CPU @ 3.00GHz"));

                // Ensure that we've got each different core. The register eax=1, out ebx contains the APIC which is
                // dependent on the core.
                int apic = cpu.Registers.GetCpuId(1, 0).Result[1] >> 25;
                Assert.That(apic, Is.EqualTo(i));
            }
        }

        [Test]
        public void IntelCreateAllFromXmlFile()
        {
            IEnumerable<ICpuId> cpus = CpuIdXmlFactory.CreateAll(MultiCpu);

            Assert.That(cpus, Is.Not.Null);
            Assert.That(cpus.Count(), Is.EqualTo(8));
            foreach (ICpuId id in cpus) {
                Assert.That(id.Description, Is.EqualTo("Intel(R) Core(TM) i7-9700 CPU @ 3.00GHz"));
            }
        }

        [Test]
        public void IntelCreateAllFromXmlFileProperty()
        {
            CpuIdXmlFactory factory = new() {
                FileName = MultiCpu
            };
            IEnumerable<ICpuId> cpus = factory.CreateAll();

            Assert.That(cpus, Is.Not.Null);
            Assert.That(cpus.Count(), Is.EqualTo(8));
            foreach (ICpuId id in cpus) {
                Assert.That(id.Description, Is.EqualTo("Intel(R) Core(TM) i7-9700 CPU @ 3.00GHz"));
            }
        }

        [Test]
        public void IntelCreateAllFromXmlConstructorFile()
        {
            CpuIdXmlFactory factory = new(MultiCpu);
            IEnumerable<ICpuId> cpus = factory.CreateAll();

            Assert.That(cpus, Is.Not.Null);
            Assert.That(cpus.Count(), Is.EqualTo(8));
            foreach (ICpuId id in cpus) {
                Assert.That(id.Description, Is.EqualTo("Intel(R) Core(TM) i7-9700 CPU @ 3.00GHz"));
            }
        }

        [Test]
        public void CreateAll()
        {
            // The main purpose of this test is to ensure that we can load as man XML dumps as possible, and that
            // instantiation doesn't crash.
            CreateAllDir("contrib", "instlatx64", "AuthenticAMD");
            CreateAllDir("contrib", "instlatx64", "GenuineIntel");
            CreateAllDir("contrib", "other");
            CreateAllDir("contrib", "users");
            CreateAllDir("AuthenticAmd");
            CreateAllDir("GenuineIntel");
            CreateAllDir("GenericIntel");
        }

        private static void CreateAllDir(params string[] path)
        {
            string directory = Path.Combine(path);
            string fullPath = Path.IsPathRooted(directory) ?
                directory :
                Path.Combine(Deploy.TestDirectory, "TestResources", directory);
            string[] files = Directory.GetFiles(fullPath, "*.xml", SearchOption.AllDirectories);
            foreach (string file in files) {
                CreateAll(file);
            }
        }

        private static void CreateAll(string fileName)
        {
            Console.WriteLine("Instantiating: {0}", fileName);
            IEnumerable<ICpuId> cpus = CpuIdXmlFactory.CreateAll(fileName);
            foreach (ICpuId cpu in cpus) {
                Assert.That(cpu, Is.Not.Null);

                CpuId.Intel.ICpuIdX86 x86cpu = cpu as CpuId.Intel.ICpuIdX86;

                switch (cpu.CpuVendor) {
                case CpuVendor.AuthenticAmd:
                    Assert.That(x86cpu, Is.Not.Null);
                    if (!string.IsNullOrEmpty(x86cpu.BrandString) && !x86cpu.BrandString.Equals(x86cpu.Description))
                        // Used for debugging if the conversion to the brand string is correct. We only check those that
                        // actually have a brand string.
                        Console.WriteLine("  CPU Brand: {0}; Description: {1}", x86cpu.BrandString, x86cpu.Description);
                    break;
                case CpuVendor.GenuineIntel:
                    Assert.That(x86cpu, Is.Not.Null);
                    break;
                }
            }
        }
    }
}
