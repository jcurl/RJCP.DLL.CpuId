namespace RJCP.Diagnostics
{
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
    }
}
