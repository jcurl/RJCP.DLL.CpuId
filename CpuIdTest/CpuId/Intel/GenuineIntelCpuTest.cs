namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.IO;
    using CodeQuality.NUnitExtensions;
    using NUnit.Framework;

    /// <summary>
    /// Test Fixture for interpreting Intel CPUID results for various CPUs
    /// </summary>
    /// <remarks>
    /// The test cases use the CPuIdXmlFactory to read the registers from an XML file. All test cases here limit
    /// themselves to valid XML files for process x86, obtained from an Intel CPU.
    /// </remarks>
    [TestFixture]
    public class GenuineIntelCpuTest
    {
        private readonly static string TestResources = Path.Combine(Deploy.TestDirectory, "TestResources", "GenuineIntel");

        private static readonly string[] CpuId01Ecx = new[] {
            "SSE3", "PCLMULQDQ", "DTES64", "MONITOR", "DS-CPL", "VMX", "SMX", "EIST",
            "TM2", "SSSE3", "CNXT-ID", "SDBG", "FMA", "CMPXCHG16B", "xTPR", "PDCM",
            "", "PCID", "DCA", "SSE4_1", "SSE4_2", "x2APIC", "MOVBE", "POPCNT",
            "TSC-DEADLINE", "AESNI", "XSAVE", "OSXSAVE", "AVX", "F16C", "RDRAND", "HYPERVISOR"
        };

        private static readonly string[] CpuId01Edx = new[] {
            "FPU", "VME", "DE", "PSE", "TSC", "MSR", "PAE", "MCE",
            "CX8", "APIC", "", "SEP", "MTRR", "PGE", "MCA", "CMOV",
            "PAT", "PSE-36", "PSN", "CLFSH", "", "DS", "ACPI", "MMX",
            "FXSR", "SSE", "SSE2", "SS", "HTT", "TM", "IA64", "PBE"
        };

        private static readonly string[] CpuId07Ebx = new[] {
            "FSGSBASE", "IA32_TSC_ADJUST", "SGX", "BMI1", "HLE", "AVX2", "FDP_EXCPTN_ONLY", "SMEP",
            "BMI2", "ERMS", "INVPCID", "RTM", "RDT-M", "FPU-CS Dep", "MPX", "RDT-A",
            "AVX512F", "AVX512DQ", "RDSEED", "ADX", "SMAP", "AVX512_IFMA", "", "CLFLUSHOPT",
            "CLWB", "INTEL_PT", "AVX512PF", "AVX512ER", "AVX512CD", "SHA", "AVX512BW", "AVX512VL"
        };

        private static readonly string[] CpuId07Ecx = new[] {
            "PREFETCHWT1", "AVX512_VBMI", "UMIP", "PKU", "OSPKE", "WAITPKG", "AVX512_VBMI2", "CET_SS",
            "GFNI", "VAES", "VPCLMULQDQ", "AVX512_VNNI", "AVX512_BITALG", "TME_EN", "AVX512_VPOPCNTDQ", "",
            "LA57", null, null, null, null, null, "RDPID", "KL",
            "BUS_LOCK_DETECT", "CLDEMOTE", "", "MOVDIRI", "MOVDIR64B", "ENQCMD", "SGX_LC", "PKS"
        };

        private static readonly string[] CpuId07Edx = new[] {
            "", "SGX-KEYS", "AVX512_4VNNIW", "AVX512_4FMAPS", "FSRM", "UINTR", "", "",
            "AVX512_VP2INTERSECT", "SRBDS_CTRL", "MD_CLEAR", "RTM_ALWAYS_ABORT", "", "RTM_FORCE_ABORT", "SERIALIZE", "Hybrid",
            "TSXLDTRK", "", "PCONFIG", "LBR", "CET_IBT", "", "AMX_BF16", "AVX512_FP16",
            "AMX_TILE", "AMX_INT8", "IBRS_IBPB", "STIBP", "L1D_FLUSH", "IA32_ARCH_CAPABILITIES", "IA32_CORE_CAPABILITIES", "SSBD"
        };

        private static readonly string[] CpuId07_01Eax = new[] {
            "", "", "", "", "AVX_VNNI", "AVX512_BF16", "", "",
            "", "", "FZMOVSB", "FSSTOSB", "FSCMPSB", "", "", "",
            "", "", "", "", "", "", "HRESET", "",
            "", "", "LAM", "", "", "", "", ""
        };

        private static readonly string[] CpuId07_01Ebx = new[] {
            "IA32_PPIN", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId07_01Ecx = new[] {
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId07_01Edx = new[] {
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "CET_SSS", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId07_02Eax = new[] {
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId07_02Ebx = new[] {
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId07_02Ecx = new[] {
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId07_02Edx = new[] {
            "PSFD", "IPRED_CTRL", "RRSBA_CTRL", "DDPD_U", "BHI_CTRL", "MCDT_NO", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId13Eax = new[] {
            "XSAVEOPT", "XSAVEC", "XGETBV", "XSAVES", "XFD", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId81Ecx = new[] {
            "AHF64", "", "", "", "", "LZCNT", "", "",
            "PREFETCHW", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", "",
            "", "", "", "", "", "", "", ""
        };

        private static readonly string[] CpuId81Edx = new[] {
            "", "", "", "", "", "", "", "",
            "", "", "", "SYSCALL", "", "", "", "",
            "", "", "", "", "XD", "", "", "",
            "", "", "1GB_PAGE", "RDTSCP", "", "LM", "", ""
        };

        private FeatureCheck FeatureCheck { get; set; }

        public GenuineIntelCpuTest()
        {
            FeatureCheck = new FeatureCheck();
            FeatureCheck.AddFeatureSet("standard", "CPUID[01h].ECX", CpuId01Ecx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[01h].EDX", CpuId01Edx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h].EBX", CpuId07Ebx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h].ECX", CpuId07Ecx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h].EDX", CpuId07Edx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h,01h].EAX", CpuId07_01Eax);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h.01h].EBX", CpuId07_01Ebx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h.01h].ECX", CpuId07_01Ecx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h.01h].EDX", CpuId07_01Edx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h,02h].EAX", CpuId07_02Eax);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h.02h].EBX", CpuId07_02Ebx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h.02h].ECX", CpuId07_02Ecx);
            FeatureCheck.AddFeatureSet("standard", "CPUID[07h.02h].EDX", CpuId07_02Edx);
            FeatureCheck.AddFeatureSet("procstate", "CPUID[0Dh,01h].EAX", CpuId13Eax);
            FeatureCheck.AddFeatureSet("extended", "CPUID[80000001h].ECX", CpuId81Ecx);
            FeatureCheck.AddFeatureSet("extended", "CPUID[80000001h].EDX", CpuId81Edx);
        }

        private GenuineIntelCpu GetCpu(string fileName)
        {
            string fullPath = Path.Combine(TestResources, fileName);
            FeatureCheck.LoadCpu(fullPath);
            GenuineIntelCpu x86cpu = FeatureCheck.Cpu as GenuineIntelCpu;
            Assert.That(x86cpu, Is.Not.Null);
            Assert.That(x86cpu.CpuVendor, Is.EqualTo(CpuVendor.GenuineIntel));
            Assert.That(x86cpu.VendorId, Is.EqualTo("GenuineIntel"));
            Assert.That(x86cpu.Topology.CoreTopology.IsReadOnly, Is.True);
            Assert.That(x86cpu.Topology.CacheTopology.IsReadOnly, Is.True);
            return x86cpu;
        }

        private void CheckSignature(int signature)
        {
            Assert.That(FeatureCheck.Cpu.ProcessorSignature,
                Is.EqualTo(signature), "Signature incorrect");
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
            // We should load a file that has the most features
            GetCpu("i9-12900k.xml");
            FeatureCheck.AssertOnMissingDescription();
        }

        [Test]
        public void Pentium4()
        {
            GenuineIntelCpu cpu = GetCpu("Pentium4.xml");
            CheckSignature(0xF27);
            FeatureCheck.Check("standard", 0x00004400, 0xBFEBFBFF);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Pentium(R) 4 CPU 2.53GHz"));
            Assert.That(cpu.Features["HTT"].Value, Is.True);
            Assert.That(cpu.Features["APIC"].Value, Is.True);

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(2));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 0, 0);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1);
        }

        [Test]
        public void PentiumM()
        {
            GenuineIntelCpu cpu = GetCpu("Dell_M70.xml");
            CheckSignature(0x6D8);
            FeatureCheck.Check("standard", 0x00000180, 0xAFE9FBFF);
            FeatureCheck.Check("extended", 0x00000000, 0x00100000);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Pentium(R) M processor 2.00GHz"));

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(2));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 0, 0);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1);
        }

        [Test]
        public void Pentium4Mobile()
        {
            GenuineIntelCpu cpu = GetCpu("Dell_c840.xml");
            CheckSignature(0xF24);
            FeatureCheck.Check("standard", 0x00000000, 0x3FEBF9FF);
            FeatureCheck.Check("extended", 0x00000000, 0x00000000);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Pentium(R) 4 Mobile CPU 1.80GHz"));
        }

        [Test]
        public void CoreQuadQ9450()
        {
            GenuineIntelCpu cpu = GetCpu("Core2Quad.xml");
            CheckSignature(0x10677);
            FeatureCheck.Check("standard", 0x0008E3FD, 0xBFEBFBFF);
            FeatureCheck.Check("extended", 0x00000001, 0x20100000);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM)2 Quad  CPU   Q9450  @ 2.66GHz"));

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Smt, 0, 0);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 2, 3);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 2);
        }

        [Test]
        public void CoreDuoT2700()
        {
            GenuineIntelCpu cpu = GetCpu("Dell_M65.xml");
            CheckSignature(0x6EC);
            FeatureCheck.Check("standard", 0x0000C1A9, 0xBFE9FBFF);
            FeatureCheck.Check("extended", 0x00000000, 0x00100000);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) Duo CPU      T2700  @ 2.33GHz"));

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Smt, 0, 0);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 0, 1);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 1);
        }

        [Test]
        public void Corei7_920()
        {
            GenuineIntelCpu cpu = GetCpu("i7-920.xml");
            CheckSignature(0x106A5);
            FeatureCheck.Check("standard", 0x0098E3BD, 0xBFEBFBFF);
            FeatureCheck.Check("extended", 0x00000001, 0x28100800);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7 CPU         920  @ 2.67GHz"));

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Smt, 0, 1);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 1, 7);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 4);
        }

        [Test]
        public void Xeon_W3540()
        {
            GenuineIntelCpu cpu = GetCpu("xeon-W3540.xml");
            CheckSignature(0x106A5);
            FeatureCheck.Check("standard", 0x009CE3BD, 0xBFEBFBFF);
            FeatureCheck.Check("extended", 0x00000001, 0x28100800);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Xeon(R) CPU           W3540  @ 2.93GHz"));

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Smt, 0, 1);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 1, 7);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 4);
        }

        [Test]
        public void Corei3_2120T()
        {
            GenuineIntelCpu cpu = GetCpu("i3-2120T.xml");
            CheckSignature(0x206A7);
            FeatureCheck.Check("standard", 0x9C982203, 0x1FEBFBFF);
            FeatureCheck.Check("extended", 0x00000001, 0x28100800);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i3-2120T CPU @ 2.60GHz"));

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Smt, 0, 0);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 1, 1);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 1);
        }

        [Test]
        public void Corei7_2630QM()
        {
            GenuineIntelCpu cpu = GetCpu("i7-2630QM.xml");
            CheckSignature(0x206A7);
            FeatureCheck.Check("standard", 0x1FBAE3BF, 0xBFEBFBFF);
            FeatureCheck.Check("extended", 0x00000001, 0x28100800);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-2630QM CPU @ 2.00GHz"));

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Smt, 0, 1);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 3, 7);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 4);
        }

        [Test]
        public void Corei5_3317U()
        {
            GenuineIntelCpu cpu = GetCpu("i5-3317U_SurfacePro.xml");
            CheckSignature(0x306A9);
            FeatureCheck.Check("standard", 0x7FBAE3BF, 0xBFEBFBFF, 0x00000281, 0x00000000, 0x00000000);
            FeatureCheck.Check("extended", 0x00000001, 0x28100800);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i5-3317U CPU @ 1.70GHz"));

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Smt, 0, 1);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 0, 7);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 4);
        }

        [Test]
        public void Corei7_3820QM()
        {
            GenuineIntelCpu cpu = GetCpu("i7-3820QM.xml");
            CheckSignature(0x306A9);
            FeatureCheck.Check("standard", 0x7FBAE3FF, 0xBFEBFBFF, 0x00000281, 0x00000000, 0x00000000);
            FeatureCheck.Check("extended", 0x00000001, 0x28100800);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-3820QM CPU @ 2.70GHz"));

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Smt, 0, 1);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 0, 7);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 4);
        }

        [Test]
        public void Corei7_4930K()
        {
            GenuineIntelCpu cpu = GetCpu("i7-4930K.xml");
            CheckSignature(0x306E4);
            FeatureCheck.Check("standard", 0x7FBEE3BF, 0xBFEBFBFF, 0x00000281, 0x00000000, 0x00000000);
            FeatureCheck.Check("extended", 0x00000001, 0x2C100800);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-4930K CPU @ 3.40GHz"));

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Smt, 0, 1);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 4, 15);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 5);
        }

        [Test]
        public void Corei7_6600U()
        {
            GenuineIntelCpu cpu = GetCpu("i7-6600U_SurfaceBook.xml");
            CheckSignature(0x406E3);
            FeatureCheck.Check("standard", 0xFEDAF387, 0xBFEBFBFF, 0x009C6FBB, 0x00000000, 0xBC000400);
            FeatureCheck.Check("procstate", 0x0000000F);
            FeatureCheck.Check("extended", 0x00000121, 0x2C100800);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-6600U CPU @ 2.60GHz"));

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Smt, 1, 1);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 1, 7);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 4);
        }

        [Test]
        public void Corei7_6600U_2p1c()
        {
            // This has a width of zero bits for the SMT portion, even if the CPU is listed in leaf 4.
            GetCpu("vmware/i7-6600U_2p1c.xml");
            Corei7_6600U_2p1c_All(FeatureCheck, 0, 0, 0);
        }

        [Test]
        public void Corei7_6600U_2p1c_All()
        {
            GetCpu("vmware/i7-6600U_2p1c.xml");

            int apic = 0;
            foreach (ICpuIdX86 cpu in FeatureCheck.Cpus) {
                FeatureCheck featureCheck = FeatureCheck.GetFeatureCpu(cpu);
                Corei7_6600U_2p1c_All(featureCheck, apic, 0, apic);
                apic += 2;
            }
        }

        private void Corei7_6600U_2p1c_All(FeatureCheck x86cpu, int apic, int core, int pkg)
        {
            CheckSignature(0x406E3);
            // Note that by running in VMWare, the feature flags are quite different.
            x86cpu.Check("standard", 0XF6FA3203, 0x0F8BFBFF, 0x009C27AB, 0x00000000, 0xBC000000);
            x86cpu.Check("procstate", 0x0000000B);
            x86cpu.Check("extended", 0x00000121, 0x2C100000);
            x86cpu.AssertOnDifference();
            Assert.That(x86cpu.Cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-6600U CPU @ 2.60GHz"));

            Assert.That(x86cpu.Cpu.Topology.ApicId, Is.EqualTo(apic));
            Assert.That(x86cpu.Cpu.Topology.CoreTopology, Has.Count.EqualTo(2));
            x86cpu.AssertCoreTopo(CpuTopoType.Core, core, 0);  // Could also be 1, but HTT=0 implies 1 core, so this is zero.
            x86cpu.AssertCoreTopo(CpuTopoType.Package, pkg, -1); // Could also be -1 << 1, but HTT=0, implies 1 core.
        }

        [Test]
        public void Corei7_6700K()
        {
            GetCpu("i7-6700K.xml");
            Corei7_6700K(FeatureCheck, 0, 0, 0);
        }

        [Test]
        public void Corei7_6700K_All()
        {
            GetCpu("i7-6700K.xml");

            int apic = 0;
            int smt = 0;
            int core = 0;
            foreach (ICpuIdX86 cpu in FeatureCheck.Cpus) {
                FeatureCheck featureCheck = FeatureCheck.GetFeatureCpu(cpu);
                Corei7_6700K(featureCheck, apic, smt, core);
                apic++;
                smt++;
                if (smt == 2) {
                    smt = 0;
                    core++;
                }
            }
        }

        private void Corei7_6700K(FeatureCheck x86cpu, int apic, int smt, int core)
        {
            CheckSignature(0x506E3);
            x86cpu.Check("standard", 0x7FFAFBBF, 0xBFEBFBFF, 0x029C6FBF, 0x00000000, 0x9C002400);
            x86cpu.Check("procstate", 0x0000000F);
            x86cpu.Check("extended", 0x00000121, 0x2C100800);
            x86cpu.AssertOnDifference();
            Assert.That(x86cpu.Cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-6700K CPU @ 4.00GHz"));

            Assert.That(x86cpu.Cpu.Topology.ApicId, Is.EqualTo(apic));
            Assert.That(x86cpu.Cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            x86cpu.AssertCoreTopo(CpuTopoType.Smt, smt, 1);
            x86cpu.AssertCoreTopo(CpuTopoType.Core, core, 7);
            x86cpu.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 4);
        }

        [Test]
        public void Corei7_6700K_vmware()
        {
            // This has a width of zero bits for the SMT portion, even if the CPU is listed in leaf 4.
            GetCpu("vmware/i7-6700K_vmware.xml");
            Corei7_6700K_vmware(FeatureCheck, 0, 0, 0, 0);
        }

        [Test]
        public void Corei7_6700K_All_vmware()
        {
            GetCpu("vmware/i7-6700K_vmware.xml");

            int apic = 0;
            int core = 0;
            int pkg = 0;
            foreach (ICpuIdX86 cpu in FeatureCheck.Cpus) {
                FeatureCheck featureCheck = FeatureCheck.GetFeatureCpu(cpu);
                Corei7_6700K_vmware(featureCheck, apic, 0, core, pkg);
                apic++;
                core++;
                if (core == 2) {
                    core = 0;
                    pkg++;
                }
            }
        }

        private void Corei7_6700K_vmware(FeatureCheck x86cpu, int apic, int smt, int core, int pkg)
        {
            CheckSignature(0x506E3);
            // Note that by running in VMWare, the feature flags are quite different.
            x86cpu.Check("standard", 0xF7FA3203, 0x1F8BBBFF, 0x009C27AB, 0x00000000, 0xBC000400);
            x86cpu.Check("procstate", 0x0000000B);
            x86cpu.Check("extended", 0x00000121, 0x2C100000);
            x86cpu.AssertOnDifference();
            Assert.That(x86cpu.Cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-6700K CPU @ 4.00GHz"));

            Assert.That(x86cpu.Cpu.Topology.ApicId, Is.EqualTo(apic));
            Assert.That(x86cpu.Cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            x86cpu.AssertCoreTopo(CpuTopoType.Smt, smt, 0);
            x86cpu.AssertCoreTopo(CpuTopoType.Core, core, 1);
            x86cpu.AssertCoreTopo(CpuTopoType.Package, pkg, -1 << 1);
        }

        [Test]
        public void Corei7_9700()
        {
            GetCpu("i7-9700.xml");
            Corei7_9700(FeatureCheck, 0, 0, 0);
        }

        [Test]
        public void Corei7_9700_All()
        {
            GetCpu("i7-9700.xml");

            int apic = 0;
            int core = 0;
            foreach (ICpuIdX86 cpu in FeatureCheck.Cpus) {
                FeatureCheck featureCheck = FeatureCheck.GetFeatureCpu(cpu);
                Corei7_9700(featureCheck, apic, 0, core);
                apic += 2;
                core++;
            }
        }

        private void Corei7_9700(FeatureCheck x86cpu, int apic, int smt, int core)
        {
            CheckSignature(0x906ED);
            x86cpu.Check("standard", 0x7FFAFBFF, 0xBFEBFBFF, 0x029C6FBF, 0x40000000, 0xBC000400);
            x86cpu.Check("procstate", 0x0000000F);
            x86cpu.Check("extended", 0x00000121, 0x2C100800);
            x86cpu.AssertOnDifference();
            Assert.That(x86cpu.Cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-9700 CPU @ 3.00GHz"));

            Assert.That(x86cpu.Cpu.Topology.ApicId, Is.EqualTo(apic));
            Assert.That(x86cpu.Cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            x86cpu.AssertCoreTopo(CpuTopoType.Smt, smt, 1);
            x86cpu.AssertCoreTopo(CpuTopoType.Core, core, 7);
            x86cpu.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 4);
        }

        [Test]
        public void Corei9_10900K()
        {
            GenuineIntelCpu cpu = GetCpu("i9-10900K.xml");
            CheckSignature(0xA0655);
            FeatureCheck.Check("standard", 0x7FFAFBFF, 0xBFEBFBFF, 0x029C67AF, 0x40000008, 0xBC000400);
            FeatureCheck.Check("procstate", 0x0000000F);
            FeatureCheck.Check("extended", 0x00000121, 0x2C100000);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i9-10900K CPU @ 3.70GHz"));

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Smt, 0, 1);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 0, 15);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 5);
        }

        [Test]
        public void Corei9_12900K_AlderLake()
        {
            GenuineIntelCpu cpu = GetCpu("i9-12900K.xml");
            CheckSignature(0x90672);
            FeatureCheck.Check("standard", 0x7FFAFBFF, 0xBFEBFBFF, 0x239CA7EB, 0x98C027AC, 0xFC1CC410, 0x00400810);
            FeatureCheck.Check("procstate", 0x0000000F);
            FeatureCheck.Check("extended", 0x00000121, 0x2C100000);
            FeatureCheck.AssertOnDifference();
            Assert.That(cpu.Description, Is.EqualTo("12th Gen Intel(R) Core(TM) i9-12900K"));

            Assert.That(cpu.Topology.CoreTopology, Has.Count.EqualTo(3));
            FeatureCheck.AssertCoreTopo(CpuTopoType.Smt, 0, 1);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Core, 0, 63);
            FeatureCheck.AssertCoreTopo(CpuTopoType.Package, 0, -1 << 7);

            CacheTopoList expectedCache = new CacheTopoList() {
                new CacheTopoPrefetch(CacheType.Prefetch, 64),
                new CacheTopoCpu(1, CacheType.Data, 12, 64, 64, 1),
                new CacheTopoCpu(1, CacheType.Instruction, 8, 64, 64, 1),
                new CacheTopoCpu(2, CacheType.Unified, 10, 64, 0x800, 1),
                new CacheTopoCpu(3, CacheType.Unified, 12, 64, 0xa000, 1),
                new CacheTopoTlb(1, CacheType.InstructionTlb4k, 8, 32 * 8),
                new CacheTopoTlb(1, CacheType.InstructionTlb2M4M, 8, 4 * 8),
                new CacheTopoTlb(1, CacheType.StoreOnlyTlb | CacheType.Page4k | CacheType.Page2M | CacheType.Page4M | CacheType.Page1G, 16, 16),
                new CacheTopoTlb(1, CacheType.LoadOnlyTlb | CacheType.Page4k, 4, 4 * 16),
                new CacheTopoTlb(1, CacheType.LoadOnlyTlb | CacheType.Page2M | CacheType.Page4M, 4, 4 * 8),
                new CacheTopoTlb(1, CacheType.LoadOnlyTlb | CacheType.Page1G, 8, 8),
                new CacheTopoTlb(2, CacheType.UnifiedTlb4k | CacheType.Page2M | CacheType.Page4M, 8, 8 * 128),
                new CacheTopoTlb(2, CacheType.UnifiedTlb4k | CacheType.Page1G, 8, 8 * 128)
            };
            Assert.That(cpu.Topology.CacheTopology, Is.EquivalentTo(expectedCache).Using(new CacheTopoComparer()));
        }
    }
}
