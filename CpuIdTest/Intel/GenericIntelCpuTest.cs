namespace RJCP.Diagnostics.Intel
{
    using System.IO;
    using CodeQuality.NUnitExtensions;
    using NUnit.Framework;

    [TestFixture]
    public class GenericIntelCpuTest
    {
        private readonly static string TestResources = Path.Combine(Deploy.TestDirectory, "TestResources", "GenericIntel");

        private static GenericIntelCpu GetCpu(string fileName)
        {
            string fullPath = Path.Combine(TestResources, fileName);

            CpuIdXmlFactory factory = new CpuIdXmlFactory();
            ICpuId cpu = factory.Create(fullPath);
            GenericIntelCpu x86cpu = cpu as GenericIntelCpu;
            Assert.That(cpu, Is.Not.Null);
            Assert.That(cpu.CpuVendor, Is.EqualTo(CpuVendor.Unknown));
            Assert.That(cpu.VendorId, Is.EqualTo("            "));
            return x86cpu;
        }

        [Test]
        public void LoadGenericProcessor()
        {
            GenericIntelCpu cpu = GetCpu("i7-9700_novendor.xml");
            Assert.That(cpu.Description, Is.Empty);
        }
    }
}
