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
        public void IntelCreateFromXmlFile()
        {
            CpuIdXmlFactory factory = new CpuIdXmlFactory();
            ICpuId cpu = factory.Create(MultiCpu);

            Assert.That(cpu, Is.Not.Null);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-9700 CPU @ 3.00GHz"));
        }

        [Test]
        public void IntelCreateFromXmlFileProperty()
        {
            CpuIdXmlFactory factory = new CpuIdXmlFactory {
                FileName = MultiCpu
            };
            ICpuId cpu = factory.Create();

            Assert.That(cpu, Is.Not.Null);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-9700 CPU @ 3.00GHz"));
        }

        [Test]
        public void IntelCreateFromXmlConstructorFile()
        {
            CpuIdXmlFactory factory = new CpuIdXmlFactory(MultiCpu);
            ICpuId cpu = factory.Create();

            Assert.That(cpu, Is.Not.Null);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-9700 CPU @ 3.00GHz"));
        }

        [Test]
        public void IntelCreateAllFromXmlFile()
        {
            CpuIdXmlFactory factory = new CpuIdXmlFactory();
            IEnumerable<ICpuId> cpus = factory.CreateAll(MultiCpu);

            Assert.That(cpus, Is.Not.Null);
            Assert.That(cpus.Count(), Is.EqualTo(8));
            foreach (ICpuId id in cpus) {
                Assert.That(id.Description, Is.EqualTo("Intel(R) Core(TM) i7-9700 CPU @ 3.00GHz"));
            }
        }

        [Test]
        public void IntelCreateAllFromXmlFileProperty()
        {
            CpuIdXmlFactory factory = new CpuIdXmlFactory {
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
            CpuIdXmlFactory factory = new CpuIdXmlFactory(MultiCpu);
            IEnumerable<ICpuId> cpus = factory.CreateAll();

            Assert.That(cpus, Is.Not.Null);
            Assert.That(cpus.Count(), Is.EqualTo(8));
            foreach (ICpuId id in cpus) {
                Assert.That(id.Description, Is.EqualTo("Intel(R) Core(TM) i7-9700 CPU @ 3.00GHz"));
            }
        }

        [Test]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Asserts in subfunctions")]
        public void CreateAll()
        {
            // The main purpose of this test is to ensure that we can load as man XML dumps as possible, and that
            // instantiation doesn't crash.
            CreateAll("contrib", "instlatx64", "AuthenticAMD");
            CreateAll("contrib", "instlatx64", "GenuineIntel");
            CreateAll("contrib", "other");
            CreateAll("contrib", "users");
            CreateAll("AuthenticAmd");
            CreateAll("GenuineIntel");
            CreateAll("GenericIntel");
        }

        private void CreateAll(params string[] path)
        {
            string directory = Path.Combine(path);
            string fullPath;
            if (Path.IsPathRooted(directory)) {
                fullPath = directory;
            } else {
                fullPath = Path.Combine(Deploy.TestDirectory, "TestResources", directory);
            }

            CpuIdXmlFactory factory = new CpuIdXmlFactory();
            string[] files = Directory.GetFiles(fullPath, "*.xml", SearchOption.AllDirectories);
            foreach (string file in files) {
                CreateAll(factory, file);
            }
        }

        private void CreateAll(CpuIdXmlFactory factory, string fileName)
        {
            Console.WriteLine("Instantiating: {0}", fileName);
            IEnumerable<ICpuId> cpus = factory.CreateAll(fileName);
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
