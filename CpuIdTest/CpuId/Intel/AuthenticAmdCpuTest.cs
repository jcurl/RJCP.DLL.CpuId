namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.IO;
    using CodeQuality.NUnitExtensions;
    using NUnit.Framework;

    [TestFixture]
    public class AuthenticAmdCpuTest
    {
        private readonly static string TestResources = Path.Combine(Deploy.TestDirectory, "TestResources", "AuthenticAmd");

        private static readonly string[] CpuId_Reserved = new[] {
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId_01_Ecx = new[] {
            "SSE3", "PCLMULQDQ", "", "MONITOR", "", "", "", "",
            "", "SSSE3", "", "", "FMA", "CMPXCHG16B", "", "",
            "", "PCID", "", "SSE4.1", "SSE4.2", "x2APIC", "MOVBE", "POPCNT",
            "", "AESNI", "XSAVE", "OSXSAVE", "AVX", "F16C", "RDRAND", "HYPERVISOR"
        };

        private static readonly string[] CpuId_01_Edx = new[] {
            "FPU", "VME", "DE", "PSE", "TSC", "MSR", "PAE", "MCE",
            "CX8", "APIC", "", "SEP", "MTRR", "PGE", "MCA", "CMOV",
            "PAT", "PSE-36", "", "CLFSH", "", "", "", "MMX",
            "FXSR", "SSE", "SSE2", "", "HTT", "", "", ""
        };

        private static readonly string[] CpuId_07_Ebx = new[] {
            "FSGSBASE", "", "", "BMI1", "", "AVX2", "", "SMEP",
            "BMI2", "", "INVPCID", "", "PQM", "", "", "PQE",
            "", "", "RDSEED", "ADX", "SMAP", "", "", "CLFLUSHOPT",
            "CLWB", "", "", "", "", "SHA", "", ""
        };

        private static readonly string[] CpuId_07_Ecx = new[] {
            "", "", "UMIP", "PKU", "OSPKE", "", "", "CET_SS",
            "", "VAES", "VPCLMULQDQ", "", "", "", "", "",
            "LA57", "", "", "", "", "", "RDPID", "",
            "BUS_LOCK_DETECT", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId_0D_s01_Eax = new[] {
            "XSAVEOPT", "XSAVEC", "XGETBV", "XSAVES", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId_0D_s01_Ecx = new[] {
            "", "", "", "", "", "", "", "",
            "", "", "", "CET_U", "CET_S", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        // TODO DOTNET-887: Use names as in doc
        private static readonly string[] CpuId_80_01_Ecx = new[] {
            "AHF64", "CMP", "SVM", "ExtApicSpace", "AM", "ABM", "SSE4A", "MisAlignSSE",
            "PREFETCHW", "OSVW", "IBS", "XOP", "SKINIT", "WDT", "", "LWP",
            "FMA4", "TCE", "", "NODEID", "", "TBM", "TOPX", "PerfCtrExtCore",
            "PerfCtrExtNB", "StreamPerfMon", "DBE", "PerfTSC", "PerfL2I", "MONITORX", "ADMSK", ""
        };

        private static readonly string[] CpuId_80_01_Edx = new[] {
            "FPU", "VME", "DE", "PSE", "TSC", "MSR", "PAE", "MCE",
            "CX8", "APIC", "", "SYSCALL", "MTRR", "PGE", "MCA", "CMOV",
            "PAT", "PSE-36", "", "MP", "XD", "", "MMXEXT", "MMX",
            "FXSR", "FFXSR", "1GB_PAGE", "RDTSCP", "", "LM", "3DNowExt", "3DNow"
        };

        private static readonly string[] CpuId_80_07_Ebx = new[] {
            "McaOverflowRecov", "SUCCOR", "HWA", "ScalableMca", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId_80_07_Edx = new[] {
            "TS", "FID", "VID", "TTP", "TM", "", "100MHz", "HwPstate",
            "TscInvariant", "CPB", "EffFreqRO", "ProcFeedbackInterface", "ProcPowerReporting", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        // TODO DOTNET-887: Use names as in doc
        private static readonly string[] CpuId_80_08_Ebx = new[] {
            "CLZERO", "IRPERF", "ASRFPEP", "INVLPGB", "RDPRU", "", "MBE", "",
            "MCOMMIT", "WBNOINVD", "", "", "IBPB", "INT_WBINVD", "IBRS", "STIBP",
            "IBRS_ALL", "STIBP_ALL", "IBRS_PREF", "IBRS_SMP", "EFER.LMSLE", "INVLPGB_NESTED", "", "PPIN",
            "SSBD", "SSBD_VirtSpecCtrl", "SSBD_NotRequired", "CPPC", "PSFD", "BTC_NO", "IPBP_RET", ""
        };

        private static readonly string[] CpuId_80_0A_Edx = new[] {
            "NP", "LbrVirt", "SVML", "NRIPS", "TscRateMsr", "VmcbClean", "FlushByAsid", "DecodeAssists",
            "", "", "PauseFilter", "", "PauseFilterThreshold", "AVIC", "", "VMSAVEvirt",
            "VGIF", "GMET", "x2AVIC", "SSSCheck", "SpecCtrl", "ROGPT", "", "HOST_MCE_OVERRIDE",
            "TlbiCtl", "VNMI", "IbsVirt", "ExtLvtAvicAccessChg", "NestedVirtVmcbAddrChk", "BusLockThreshold", "", ""
        };

        private static readonly string[] CpuId_80_1A_Eax = new[] {
            "FP128", "MOVU", "FP256", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId_80_1B_Eax = new[] {
            "IBSFFV", "FetchSam", "OpSam", "RdWrOpCnt", "OpCnt", "BrnTrgt", "OpCntExt", "RipInvalidChk",
            "OpBrnFuse", "", "", "IbsL3MissFiltering", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId_80_1C_Eax = new[] {
            "LwpAvail", "LwpVAL", "LwpIRE", "LwpBRE", "LwpDME", "LwpCNH", "LwpRNH", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "LwpCont", "LwpPTSC", "LwpInt"
        };

        private static readonly string[] CpuId_80_1C_Ecx = new[] {
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "LwpBranchPrediction", "LwpIpFiltering", "LwpCacheLevels", "LwpCacheLatency"
        };

        private static readonly string[] CpuId_80_1C_Edx = new[] {
            "LwpAvail", "LwpVAL", "LwpIRE", "LwpBRE", "LwpDME", "LwpCNH", "LwpRNH", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "LwpCont", "LwpPTSC", "LwpInt"
        };

        private static readonly string[] CpuId_80_1F_Eax = new[] {
            "SME", "SEV", "PageFlushMsr", "SEV-ES", "SEV-SNP", "VMPL", "RMPQUERY", "VmplSSS",
            "SecureTsc", "TscAuxVirtualization", "HwEnvCacheCoh", "SEV-64", "RestrictedInjection", "AlternateInjection", "DebugSwap", "PreventHostIbs",
            "VTE", "VmgexitParameter", "VirtualTomMsr", "IbsVirtGuestCtl", "", "", "", "",
            "VmsaRegProt", "SmtProtection", "SvsmCommPageMSR", "NestedVirtSnpMsr", "", "", "", ""
        };

        private static readonly string[] CpuId_80_20_Ebx = new[] {
            "", "L3MBE", "L3SMBE", "BMEC", "L3RR", "ABMC", "SDCIAE", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId_80_20_s03_Ecx = new[] {
            "L3CacheLclBwFillMon", "L3CacheRmtBwFillMon", "L3CacheLclBwNtWrMon", "L3CacheRmtBwNtWrMon", "L3CacheLclSlowBwFIllMon", "L3CacheRmtSlowBwFIllMon", "L3CacheVicMon", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId_80_21_Eax = new[] {
            "NoNestedDataBp", "", "LFenceAlwaysSerializing", "SmmPgCfgLock", "", "", "NullSelectClearsBase", "UpperAddressIgnore",
            "AutomaticIBRS", "NoSmmCtlMSR", "", "", "", "PrefetchCtlMsr", "", "",
            "", "CpuidUserDis", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId_80_22_Eax = new[] {
            "PerfMonV2", "LbrStack", "LbrAndPmcFreeze", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };
        private static readonly string[] CpuId_80_23_Eax = new[] {
            "MemHmk", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private FeatureCheck FeatureCheck { get; set; }

        public AuthenticAmdCpuTest()
        {
            FeatureCheck = new FeatureCheck();

            // The order of the group is important, as this defines the order when using `FeatureCheck.Check`
            FeatureCheck.AddFeatureSet("standard", "CPUID[01h].ECX", CpuId_01_Ecx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[01h].EDX", CpuId_01_Edx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h].EBX", CpuId_07_Ebx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h].ECX", CpuId_07_Ecx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h].EDX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("procstate", "CPUID[0Dh,01h].EAX", CpuId_0D_s01_Eax);
            FeatureCheck.AddFeatureSet("procstate", "CPUID[0Dh,01h].ECX", CpuId_0D_s01_Ecx);
            FeatureCheck.AddFeatureSet("procstate", "CPUID[0Dh,01h].EDX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("extended", "CPUID[80000001h].ECX", CpuId_80_01_Ecx);
            FeatureCheck.AddFeatureSet("extended", "CPUID[80000001h].EDX", CpuId_80_01_Edx);
            FeatureCheck.AddFeatureSet("power", "CPUID[80000007h].EBX", CpuId_80_07_Ebx);
            FeatureCheck.AddFeatureSet("power", "CPUID[80000007h].EDX", CpuId_80_07_Edx);
            FeatureCheck.AddFeatureSet("extfeature", "CPUID[80000008h].EBX", CpuId_80_08_Ebx);
            FeatureCheck.AddFeatureSet("svm", "CPUID[8000000Ah].EDX", CpuId_80_0A_Edx);
            FeatureCheck.AddFeatureSet("perfopt", "CPUID[8000001Ah].EAX", CpuId_80_1A_Eax);
            FeatureCheck.AddFeatureSet("perfopt", "CPUID[8000001Ah].EBX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("perfopt", "CPUID[8000001Ah].ECX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("perfopt", "CPUID[8000001Ah].EDX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("perfsamp", "CPUID[8000001Bh].EAX", CpuId_80_1B_Eax);
            FeatureCheck.AddFeatureSet("perfsamp", "CPUID[8000001Bh].EBX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("perfsamp", "CPUID[8000001Bh].ECX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("perfsamp", "CPUID[8000001Bh].EDX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("lwp", "CpuId[8000001Ch].EAX", CpuId_80_1C_Eax);
            FeatureCheck.AddFeatureSet("lwp", "CpuId[8000001Ch].ECX", CpuId_80_1C_Ecx);
            FeatureCheck.AddFeatureSet("lwp", "CpuId[8000001Ch].EDX", CpuId_80_1C_Edx);
            FeatureCheck.AddFeatureSet("encmem", "CPUID[8000001Fh].EAX", CpuId_80_1F_Eax);
            FeatureCheck.AddFeatureSet("pqos", "CPUID[80000020h].EAX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("pqos", "CPUID[80000020h].EBX", CpuId_80_20_Ebx);
            FeatureCheck.AddFeatureSet("pqos", "CPUID[80000020h].ECX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("pqos", "CPUID[80000020h].EDX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("pqos", "CPUID[80000020h].ECX", CpuId_80_20_s03_Ecx);
            FeatureCheck.AddFeatureSet("pqos", "CPUID[80000020h].EDX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("extended2", "CPUID[80000021h].EAX", CpuId_80_21_Eax);
            FeatureCheck.AddFeatureSet("extended2", "CPUID[80000021h].ECX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("extended2", "CPUID[80000021h].EDX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("extperf", "CPUID[80000022H].EAX", CpuId_80_22_Eax);
            FeatureCheck.AddFeatureSet("extperf", "CPUID[80000022h].ECX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("extperf", "CPUID[80000022h].EDX", CpuId_Reserved);
            FeatureCheck.AddFeatureSet("mkenc", "CPUID[80000022H].EAX", CpuId_80_23_Eax);
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
            Assert.That(FeatureCheck.Cpu.ProcessorSignature, Is.EqualTo(signature), "Signature incorrect");
            Assert.That(FeatureCheck.Cpu.Stepping, Is.EqualTo(signature & 0xF), "Stepping incorrect");
            Assert.That(FeatureCheck.Cpu.Model, Is.EqualTo(((signature >> 4) & 0xF) + ((signature >> 12) & 0xF0)), "Model incorrect");
            Assert.That(FeatureCheck.Cpu.Family, Is.EqualTo(((signature >> 8) & 0xF) + ((signature >> 20) & 0xFF)), "Family incorrect");
            Assert.That(FeatureCheck.Cpu.ProcessorType, Is.EqualTo((signature >> 12) & 0x3), "Processor Type incorrect");
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
            FeatureCheck.Check("extended", 0x01EBBFFF, 0x2FD3FBFF);
            FeatureCheck.Check("extfeature", 0x00000000);
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
            FeatureCheck.Check("extended", 0x000007FF, 0xEFD3FBFF);
            FeatureCheck.Check("extfeature", 0x00003030);
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
            FeatureCheck.Check("extended", 0x000037FF, 0xEFD3FBFF);
            FeatureCheck.Check("extfeature", 0x00002001);
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
