namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.IO;
    using CodeQuality.NUnitExtensions;
    using NUnit.Framework;

    [TestFixture]
    public class AuthenticAmdCpuTest
    {
        private readonly static string TestResources = Path.Combine(Deploy.TestDirectory, "TestResources", "AuthenticAmd");

        private static readonly string[] CpuId01Ecx = new[] {
            "SSE3", "PCLMULQDQ", "", "MONITOR", "", "", "", "",
            "", "SSSE3", "", "", "FMA", "CMPXCHG16B", "", "",
            "", "PCID", "", "SSE4.1", "SSE4.2", "x2APIC", "MOVBE", "POPCNT",
            "", "AESNI", "XSAVE", "OSXSAVE", "AVX", "F16C", "RDRAND", "HYPERVISOR"
        };

        private static readonly string[] CpuId01Edx = new[] {
            "FPU", "VME", "DE", "PSE", "TSC", "MSR", "PAE", "MCE",
            "CX8", "APIC", "", "SEP", "MTRR", "PGE", "MCA", "CMOV",
            "PAT", "PSE-36", "", "CLFSH", "", "", "", "MMX",
            "FXSR", "SSE", "SSE2", "", "HTT", "", "", ""
        };

        private static readonly string[] CpuId07Ebx = new[] {
            "FSGSBASE", "", "", "BMI1", "", "AVX2", "", "SMEP",
            "BMI2", "", "INVPCID", "", "PQM", "", "", "PQE",
            "", "", "RDSEED", "ADX", "SMAP", "", "", "CLFLUSHOPT",
            "CLWB", "", "", "", "", "SHA", "", ""
        };

        private static readonly string[] CpuId07Ecx = new[] {
            "", "", "UMIP", "PKU", "OSPKE", "", "", "CET_SS",
            "", "VAES", "VPCLMULQDQ", "", "", "", "", "",
            "LA57", "", "", "", "", "", "RDPID", "",
            "BUS_LOCK_DETECT", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId07Edx = new[] {
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId13Eax = new[] {
            "XSAVEOPT", "XSAVEC", "XGETBV", "XSAVES", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId81Ecx = new[] {
            "AHF64", "CMP", "SVM", "ExtApicSpace", "AM", "ABM", "SSE4A", "MisAlignSSE",
            "PREFETCHW", "OSVW", "IBS", "XOP", "SKINIT", "WDT", "", "LWP",
            "FMA4", "TCE", "", "NODEID", "", "TBM", "TOPX", "PerfCtrExtCore",
            "PerfCtrExtNB", "StreamPerfMon", "DBE", "PerfTSC", "PerfL2I", "MONITORX", "ADMSK", ""
        };

        private static readonly string[] CpuId81Edx = new[] {
            "", "", "", "", "", "", "", "",
            "", "", "", "SYSCALL", "", "", "", "",
            "", "", "", "MP", "XD", "", "MMXEXT", "",
            "", "FFXSR", "1GB_PAGE", "RDTSCP", "", "LM", "3DNowExt", "3DNow"
        };

        private static readonly string[] CpuId88Ebx = new[] {
            "CLZERO", "IRPERF", "ASRFPEP", "INVLPGB", "RDPRU", "", "MBE", "",
            "MCOMMIT", "WBNOINVD", "", "", "IBPB", "INT_WBINVD", "IBRS", "STIBP",
            "IBRS_ALL", "STIBP_ALL", "IBRS_PREF", "IBRS_SMP", "EFER.LMSLE", "INVLPGB_NESTED", "", "PPIN",
            "SSBD", "SSBD_VirtSpecCtrl", "SSBD_NotRequired", "CPPC", "PSFD", "BTC_NO", "IPBP_RET", ""
        };

        private static readonly string[] CpuId81AEax = new[] {
            "FP128", "MOVU", "FP256", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId81FEbx = new[] {
            "SME", "SEV", "PageFlushMsr", "SEV-ES", "SEV-SNP", "VMPL", "RMPQUERY", "VmplSSS",
            "SecureTsc", "TscAuxVirtualization", "HwEnvCacheCoh", "SEV-64", "RestrictedInjection", "AlternateInjection", "DebugSwap", "PreventHostIbs",
            "VTE", "VmgexitParameter", "VirtualTomMsr", "IbsVirtGuestCtl", "", "", "", "",
            "VmsaRegProt", "SmtProtection", "SvsmCommPageMSR", "NestedVirtSnpMsr", "", "", "", ""
        };

        private FeatureCheck FeatureCheck { get; set; }

        public AuthenticAmdCpuTest()
        {
            FeatureCheck = new FeatureCheck();
            FeatureCheck.AddFeatureSet("standard", "CPUID[01h].ECX", CpuId01Ecx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[01h].EDX", CpuId01Edx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h].EBX", CpuId07Ebx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h].ECX", CpuId07Ecx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h].EDX", CpuId07Edx);
            FeatureCheck.AddFeatureSet("procstate", "CPUID[0Dh,01h].EAX", CpuId13Eax);
            FeatureCheck.AddFeatureSet("extended", "CPUID[80000001h].ECX", CpuId81Ecx);
            FeatureCheck.AddFeatureSet("extended", "CPUID[80000001h].EDX", CpuId81Edx);
            FeatureCheck.AddFeatureSet("extended", "CPUID[80000008h].EBX", CpuId88Ebx);
            FeatureCheck.AddFeatureSet("extended", "CPUID[8000001Ah].EAX", CpuId81AEax);
            FeatureCheck.AddFeatureSet("extended", "CPUID[8000001Fh].EBX", CpuId81FEbx);
        }
        private AuthenticAmdCpu GetCpu(string fileName)
        {
            string fullPath = Path.Combine(TestResources, fileName);
            FeatureCheck.LoadCpu(fullPath);
            AuthenticAmdCpu x86cpu = FeatureCheck.Cpu as AuthenticAmdCpu;
            Assert.That(x86cpu, Is.Not.Null);
            Assert.That(x86cpu.CpuVendor, Is.EqualTo(CpuVendor.AuthenticAmd));
            Assert.That(x86cpu.VendorId, Is.EqualTo("AuthenticAMD"));
            Assert.That(x86cpu.Topology.CoreTopology.IsReadOnly, Is.True);
            Assert.That(x86cpu.Topology.CacheTopology.IsReadOnly, Is.True);
            return x86cpu;
        }

        private void CheckSignature(int signature)
        {
            Assert.That(FeatureCheck.Cpu.ProcessorSignature
                , Is.EqualTo(signature), "Signature incorrect");
            Assert.That(FeatureCheck.Cpu.Stepping,
                Is.EqualTo(signature & 0xF), "Stepping incorrect");
            Assert.That(FeatureCheck.Cpu.Model,
                Is.EqualTo(((signature >> 4) & 0xF) + ((signature >> 12) & 0xF0)), "Model incorrect");
            Assert.That(FeatureCheck.Cpu.Family,
                Is.EqualTo(((signature >> 8) & 0xF) + ((signature >> 20) & 0xFF)), "Family incorrect");
            Assert.That(FeatureCheck.Cpu.ProcessorType,
                Is.EqualTo((signature >> 12) & 0x3), "Processor Type incorrect");
        }

        [Test]
        public void CheckDescription()
        {
            // We just want to load any file, to get the AuthenticAmdCpu to check for descriptions
            GetCpu("AMD A8.xml");
            FeatureCheck.AssertOnMissingDescription();
        }

        [Test]
        public void AmdA8()
        {
            AuthenticAmdCpu cpu = GetCpu("AMD A8.xml");
            CheckSignature(0x610F01);
            FeatureCheck.Check("standard", 0x3698320B, 0x178BFBFF, 0x00000008, 0x00000000, 0x00000000);
            FeatureCheck.Check("extended", 0x01EBBFFF, 0x2FD3FBFF, 0x00000000);
            Assert.That(cpu.Description, Is.EqualTo("AMD A8-5500 APU with Radeon(tm) HD Graphics"));

            CacheTopoList expectedCache = new CacheTopoList() {
                new CacheTopoCpu(1, CacheType.Instruction, 2, 64, 64),
                new CacheTopoCpu(1, CacheType.Data, 4, 64, 16),
                new CacheTopoCpu(2, CacheType.Unified, 16, 64, 2048),
                new CacheTopoTlb(1, CacheType.InstructionTlb4k, 0, 48),
                new CacheTopoTlb(1, CacheType.InstructionTlb2M4M, 0, 24),
                new CacheTopoTlb(1, CacheType.InstructionTlb1G, 0, 24),
                new CacheTopoTlb(1, CacheType.DataTlb4k, 0, 64),
                new CacheTopoTlb(1, CacheType.DataTlb2M4M, 0, 64),
                new CacheTopoTlb(1, CacheType.DataTlb1G, 0, 64),
                new CacheTopoTlb(2, CacheType.InstructionTlb4k, 4, 512),
                new CacheTopoTlb(2, CacheType.InstructionTlb2M4M, 8, 1024),
                new CacheTopoTlb(2, CacheType.InstructionTlb1G, 8, 24),
                new CacheTopoTlb(2, CacheType.DataTlb4k, 8, 1024),
                new CacheTopoTlb(2, CacheType.DataTlb2M4M, 8, 1024),
                new CacheTopoTlb(2, CacheType.DataTlb1G, 8, 1024)
            };
            Assert.That(cpu.Topology.CacheTopology, Is.EquivalentTo(expectedCache).Using(new CacheTopoComparer()));
        }

        [Test]
        public void AmdGeode()
        {
            AuthenticAmdCpu cpu = GetCpu("AMD Geode.xml");
            CheckSignature(0x5A2);
            FeatureCheck.Check("standard", 0x00000000, 0x0088A93D);
            FeatureCheck.Check("extended", 0x00000000, 0xC0C0A13D);
            Assert.That(cpu.Description, Is.EqualTo("Geode(TM) Integrated Processor by AMD PCS"));
        }

        [Test]
        public void AmdOpteron2347HE()
        {
            AuthenticAmdCpu cpu = GetCpu("AMD Opteron-2347-HE.xml");
            CheckSignature(0x100F21);
            FeatureCheck.Check("standard", 0x00802009, 0x17BFBFFD);
            FeatureCheck.Check("extended", 0x000007FF, 0xEFD3FBFF, 0x00003030);
            Assert.That(cpu.Description, Is.EqualTo("Quad-Core AMD Opteron(tm) Processor 2347 HE"));
            Assert.That(cpu.BrandString, Is.EqualTo("Quad-Core AMD Opteron(tm) Processor 2347 HE"));

            CacheTopoList expectedCache = new CacheTopoList() {
                new CacheTopoCpu(1, CacheType.Instruction, 2, 64, 64),
                new CacheTopoCpu(1, CacheType.Data, 2, 64, 64),
                new CacheTopoCpu(2, CacheType.Unified, 16, 64, 512),
                new CacheTopoCpu(3, CacheType.Unified, 32, 64, 2 * 1024),
                new CacheTopoTlb(1, CacheType.InstructionTlb4k, 0, 32),
                new CacheTopoTlb(1, CacheType.InstructionTlb2M4M, 0, 16),
                new CacheTopoTlb(1, CacheType.DataTlb4k, 0, 48),
                new CacheTopoTlb(1, CacheType.DataTlb2M4M, 0, 48),
                new CacheTopoTlb(1, CacheType.DataTlb1G, 0, 48),
                new CacheTopoTlb(2, CacheType.InstructionTlb4k, 4, 512),
                new CacheTopoTlb(2, CacheType.DataTlb4k, 4, 512),
                new CacheTopoTlb(2, CacheType.DataTlb2M4M, 2, 128),
            };
            Assert.That(cpu.Topology.CacheTopology, Is.EquivalentTo(expectedCache).Using(new CacheTopoComparer()));
        }

        [Test]
        public void AmdPhenomIIX2550()
        {
            AuthenticAmdCpu cpu = GetCpu("AMD PhenomIIX2-550.xml");
            CheckSignature(0x100F42);
            FeatureCheck.Check("standard", 0x00802009, 0x178BFBFF);
            FeatureCheck.Check("extended", 0x000037FF, 0xEFD3FBFF, 0x00002001);
            Assert.That(cpu.Description, Is.EqualTo("AMD Phenom(tm) II X2 550 Processor"));
            Assert.That(cpu.BrandString, Is.EqualTo("AMD Phenom(tm) II X2 550 Processor"));

            CacheTopoList expectedCache = new CacheTopoList() {
                new CacheTopoCpu(1, CacheType.Instruction, 2, 64, 64),
                new CacheTopoCpu(1, CacheType.Data, 2, 64, 64),
                new CacheTopoCpu(2, CacheType.Unified, 16, 64, 512),
                new CacheTopoCpu(3, CacheType.Unified, 48, 64, 6 * 1024),
                new CacheTopoTlb(1, CacheType.InstructionTlb4k, 0, 32),
                new CacheTopoTlb(1, CacheType.InstructionTlb2M4M, 0, 16),
                new CacheTopoTlb(1, CacheType.DataTlb4k, 0, 48),
                new CacheTopoTlb(1, CacheType.DataTlb2M4M, 0, 48),
                new CacheTopoTlb(1, CacheType.DataTlb1G, 0, 48),
                new CacheTopoTlb(2, CacheType.InstructionTlb4k, 4, 512),
                new CacheTopoTlb(2, CacheType.DataTlb4k, 4, 512),
                new CacheTopoTlb(2, CacheType.DataTlb2M4M, 2, 128),
                new CacheTopoTlb(2, CacheType.DataTlb1G, 8, 16)
            };
            Assert.That(cpu.Topology.CacheTopology, Is.EquivalentTo(expectedCache).Using(new CacheTopoComparer()));
        }
    }
}
