namespace RJCP.Diagnostics.Intel
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
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

        public static GenuineIntelCpu GetCpu(string fileName)
        {
            string fullPath = Path.Combine(TestResources, fileName);

            CpuIdXmlFactory factory = new CpuIdXmlFactory();
            ICpuId cpu = factory.Create(fullPath);
            GenuineIntelCpu x86cpu = cpu as GenuineIntelCpu;
            Assert.That(cpu, Is.Not.Null);
            Assert.That(cpu.CpuVendor, Is.EqualTo(CpuVendor.GenuineIntel));
            Assert.That(cpu.VendorId, Is.EqualTo("GenuineIntel"));
            return x86cpu;
        }

        private static void CheckSignature(ICpuIdX86 cpu, int signature)
        {
            Assert.That(cpu.ProcessorSignature, Is.EqualTo(signature), "Signature incorrect");
            Assert.That(cpu.Stepping, Is.EqualTo(signature & 0xF), "Stepping incorrect");
            Assert.That(cpu.Model, Is.EqualTo(((signature >> 4) & 0xF) + ((signature >> 12) & 0xF0)), "Model incorrect");
            Assert.That(cpu.Family, Is.EqualTo(((signature >> 8) & 0xF) + ((signature >> 20) & 0xFF)), "Family incorrect");
            Assert.That(cpu.ProcessorType, Is.EqualTo((signature >> 12) & 0x3), "Processor Type incorrect");
        }

        private static readonly string[] CpuId01Ecx = new[] {
            "SSE3", "PCLMULQDQ", "DTES64", "MONITOR", "DS-CPL", "VMX", "SMX", "EIST",
            "TM2", "SSSE3", "CNXT-ID", "SDBG", "FMA", "CMPXCHG16B", "xTPR", "PDCM",
            "CPUID[01h].EXC[16]", "PCID", "DCA", "SSE4.1", "SSE4.2", "x2APIC", "MOVBE", "POPCNT",
            "TSC-DEADLINE", "AESNI", "XSAVE", "OSXSAVE", "AVX", "F16C", "RDRAND", "HYPERVISOR"
        };

        private static readonly string[] CpuId01Edx = new[] {
            "FPU", "VME", "DE", "PSE", "TSC", "MSR", "PAE", "MCE",
            "CX8", "APIC", "CPUID[01h].EDX[10]", "SEP", "MTRR", "PGE", "MCA", "CMOV",
            "PAT", "PSE-36", "PSN", "CLFSH", "CPUID[01h].EDX[20]", "DS", "ACPI", "MMX",
            "FXSR", "SSE", "SSE2", "SS", "HTT", "TM", "IA64", "PBE"
        };

        private static readonly string[] CpuId07Ebx = new[] {
            "FSGSBASE", "IA32_TSC_ADJUST", "SGX", "BMI1", "HLE", "AVX2", "FDP_EXCPTN_ONLY", "SMEP",
            "BMI2", "ERMS", "INVPCID", "RTM", "RDT-M", "FPU-CS Dep", "MPX", "RDT-A",
            "AVX512F", "AVX512DQ", "RDSEED", "ADX", "SMAP", "AVX512_IFMA", "CPUID[07h].EBX[22]", "CLFLUSHOPT",
            "CLWB", "INTEL_PT", "AVX512PF", "AVX512ER", "AVX512CD", "SHA", "AVX512BW", "AVX512VL"
        };

        private static readonly string[] CpuId07Ecx = new[] {
            "PREFETCHWT1", "AVX512_VBMI", "UMIP", "PKU", "OSPKE", "WAITPKG", "AVX512_VBMI2", "CET_SS",
            "GFNI", "VAES", "VPCLMULQDQ", "AVX512_VNNI", "AVX512_BITALG", "CPUID[07h].ECX[13]", "AVX512_POPCNTDQ", "5L_PAGE",
            "CPUID[07h].ECX[16]", null, null, null, null, null, "RDPID", "CPUID[07h].ECX[23]",
            "CPUID[07h].ECX[24]", "CLDEMOTE", "CPUID[07h].ECX[26]", "MOVDIRI", "MOVDIR64B", "ENQCMD", "SGX_LC", "PKS"
        };

        private static readonly string[] CpuId07Edx = new[] {
            "CPUID[07h].EDX[0]", "CPUID[07h].ECX[1]", "AVX512_4NNIW", "AVX512_4FMAPS", "FSRM", "CPUID[07h].EDX[5]", "CPUID[07h].EDX[6]", "CPUID[07h].EDX[7]",
            "AVX512_VP2INTERSECT", "SRBDS_CTRL", "MD_CLEAR", "CPUID[07h].EDX[11]", "CPUID[07h].EDX[12]", "TSX_FORCE_ABORT", "SERIALIZE", "Hybrid",
            "TSXLDTRK", "CPUID[07h].EDX[17]", "PCONFIG", "LBR", "CET_IBT", "CPUID[07h].EDX[21]", "AMX_BF16", "CPUID[07h].EDX[23]",
            "AMX_TILE", "AMX_INT8", "IBRS_IBPB", "STIBP", "L1D_FLUSH", "IA32_ARCH_CAPABILITIES", "IA32_CORE_CAPABILITIES", "SSBD"
        };

        private static readonly string[] CpuId81Ecx = new[] {
            "LAHF", "CPUID[80000001h].ECX[1]", "CPUID[80000001h].ECX[2]", "CPUID[80000001h].ECX[3]", "CPUID[80000001h].ECX[4]", "LZCNT", "CPUID[80000001h].ECX[6]", "CPUID[80000001h].ECX[7]",
            "PREFETCHW", "CPUID[80000001h].ECX[9]", "CPUID[80000001h].ECX[10]", "CPUID[80000001h].ECX[11]", "CPUID[80000001h].ECX[12]", "CPUID[80000001h].ECX[13]", "CPUID[80000001h].ECX[14]", "CPUID[80000001h].ECX[15]",
            "CPUID[80000001h].ECX[16]", "CPUID[80000001h].ECX[17]", "CPUID[80000001h].ECX[18]", "CPUID[80000001h].ECX[19]", "CPUID[80000001h].ECX[20]", "CPUID[80000001h].ECX[21]", "CPUID[80000001h].ECX[22]", "CPUID[80000001h].ECX[23]",
            "CPUID[80000001h].ECX[24]", "CPUID[80000001h].ECX[25]", "CPUID[80000001h].ECX[26]", "CPUID[80000001h].ECX[27]", "CPUID[80000001h].ECX[28]", "CPUID[80000001h].ECX[29]", "CPUID[80000001h].ECX[30]", "CPUID[80000001h].ECX[31]"
        };

        private static readonly string[] CpuId81Edx = new[] {
            "CPUID[80000001h].EDX[0]", "CPUID[80000001h].EDX[1]", "CPUID[80000001h].EDX[2]", "CPUID[80000001h].EDX[3]", "CPUID[80000001h].EDX[4]", "CPUID[80000001h].EDX[5]", "CPUID[80000001h].EDX[6]", "CPUID[80000001h].EDX[7]",
            "CPUID[80000001h].EDX[8]", "CPUID[80000001h].EDX[9]", "CPUID[80000001h].EDX[10]", "SYSCALL", "CPUID[80000001h].EDX[12]", "CPUID[80000001h].EDX[13]", "CPUID[80000001h].EDX[14]", "CPUID[80000001h].EDX[15]",
            "CPUID[80000001h].EDX[16]", "CPUID[80000001h].EDX[17]", "CPUID[80000001h].EDX[18]", "CPUID[80000001h].EDX[19]", "XD", "CPUID[80000001h].EDX[21]", "CPUID[80000001h].EDX[22]", "CPUID[80000001h].EDX[23]",
            "CPUID[80000001h].EDX[24]", "CPUID[80000001h].EDX[25]", "1GB_PAGE", "RDTSCP", "CPUID[80000001h].EDX[28]", "IA32_64", "CPUID[80000001h].EDX[30]", "CPUID[80000001h].EDX[31]"
        };

        private static void CalculateFeatures(uint reg, string[] featureSet, HashSet<string> features)
        {
            int bitMask = 1;
            for (int i = 0; i < 32; i++) {
                if ((reg & bitMask) != 0 && featureSet[i] != null) features.Add(featureSet[i]);
                bitMask <<= 1;
            }
        }

        private static void CheckFeatures(ICpuId cpu, uint id01ecx, uint id01edx)
        {
            CheckFeatures(cpu, id01ecx, id01edx, 0, 0);
        }

        private static void CheckFeatures(ICpuId cpu, uint id01ecx, uint id01edx, uint id81ecx, uint id81edx)
        {
            CheckFeatures(cpu, id01ecx, id01edx, id81ecx, id81edx, 0, 0, 0);
        }

        private static void CheckFeatures(ICpuId cpu, uint id01ecx, uint id01edx, uint id81ecx, uint id81edx, uint id07ebx, uint id07ecx, uint id07edx)
        {
            HashSet<string> features = new HashSet<string>();
            CalculateFeatures(id01ecx, CpuId01Ecx, features);
            CalculateFeatures(id01edx, CpuId01Edx, features);
            CalculateFeatures(id81ecx, CpuId81Ecx, features);
            CalculateFeatures(id81edx, CpuId81Edx, features);
            CalculateFeatures(id07ebx, CpuId07Ebx, features);
            CalculateFeatures(id07ecx, CpuId07Ecx, features);
            CalculateFeatures(id07edx, CpuId07Edx, features);
            CheckFeatures(cpu, features);
        }

        private static void CheckFeatures(ICpuId cpu, HashSet<string> features)
        {
            List<string> cpuMissingFeatures = new List<string>();
            List<string> missingFeatures = new List<string>();
            foreach (string feature in cpu.Features) {
                bool expected = features.Contains(feature);
                bool present = cpu.Features[feature];
                if (expected && !present) {
                    cpuMissingFeatures.Add(feature);
                } else if (!expected && present) {
                    missingFeatures.Add(feature);
                }
            }
            foreach (string feature in features) {
                bool expected = features.Contains(feature);
                bool present = cpu.Features[feature];
                if (expected && !present) {
                    cpuMissingFeatures.Add(feature);
                } else if (!expected && present) {
                    missingFeatures.Add(feature);
                }
            }
            if (cpuMissingFeatures.Count > 0 || missingFeatures.Count > 0) {
                StringBuilder cpuMissing = new StringBuilder();
                foreach (string feature in cpuMissingFeatures) {
                    if (cpuMissing.Length > 0) cpuMissing.Append(", ");
                    cpuMissing.Append(feature);
                }
                if (cpuMissing.Length == 0) cpuMissing.Append("-");

                StringBuilder missing = new StringBuilder();
                foreach (string feature in missingFeatures) {
                    if (missing.Length > 0) missing.Append(", ");
                    missing.Append(feature);
                }
                if (missing.Length == 0) missing.Append("-");

                string message = string.Format("Missing Features: CPU has {0}; missing {1}", cpuMissing, missing);
                Assert.Fail(message);
            }
        }

        [Test]
        public void CheckDescription()
        {
            // We just want to load any file, to get the IntelCPU to check for descriptions
            GenuineIntelCpu cpu = GetCpu("Pentium4.xml");

            HashSet<string> missing = new HashSet<string>();
            CheckDescription(cpu, CpuId01Ecx, missing);
            CheckDescription(cpu, CpuId01Edx, missing);
            CheckDescription(cpu, CpuId07Ebx, missing);
            CheckDescription(cpu, CpuId07Ecx, missing);
            CheckDescription(cpu, CpuId07Edx, missing);
            CheckDescription(cpu, CpuId81Ecx, missing);
            CheckDescription(cpu, CpuId81Edx, missing);

            if (missing.Count > 0) {
                StringBuilder missingText = new StringBuilder();
                foreach (string entry in missing) {
                    if (missingText.Length != 0) missingText.Append(", ");
                    missingText.Append(entry);
                }
                Assert.Fail("Missing descriptions for: {0}", missingText);
            }
        }

        private void CheckDescription(ICpuId cpu, IEnumerable<string> features, HashSet<string> missing)
        {
            foreach (string feature in features) {
                if (feature != null && !feature.StartsWith("CPUID[") &&
                    string.IsNullOrWhiteSpace(cpu.Features.Description(feature))) {
                    missing.Add(feature);
                }
            }
        }

        [Test]
        public void Pentium4()
        {
            GenuineIntelCpu cpu = GetCpu("Pentium4.xml");
            CheckSignature(cpu, 0xF27);
            CheckFeatures(cpu, 0x00004400, 0xBFEBFBFF);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Pentium(R) 4 CPU 2.53GHz"));
        }

        [Test]
        public void PentiumM()
        {
            GenuineIntelCpu cpu = GetCpu("Dell_M70.xml");
            CheckSignature(cpu, 0x6D8);
            CheckFeatures(cpu, 0x00000180, 0xAFE9FBFF, 0x00000000, 0x00100000);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Pentium(R) M processor 2.00GHz"));
        }

        [Test]
        public void Pentium4Mobile()
        {
            GenuineIntelCpu cpu = GetCpu("Dell_c840.xml");
            CheckSignature(cpu, 0xF24);
            CheckFeatures(cpu, 0x00000000, 0x3FEBF9FF, 0x00000000, 0x00000000);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Pentium(R) 4 Mobile CPU 1.80GHz"));
        }

        [Test]
        public void CoreQuadQ9450()
        {
            GenuineIntelCpu cpu = GetCpu("Core2Quad.xml");
            CheckSignature(cpu, 0x10677);
            CheckFeatures(cpu, 0x0008E3FD, 0xBFEBFBFF, 0x00000001, 0x20100000);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM)2 Quad  CPU   Q9450  @ 2.66GHz"));
        }

        [Test]
        public void CoreDuoT2700()
        {
            GenuineIntelCpu cpu = GetCpu("Dell_M65.xml");
            CheckSignature(cpu, 0x6EC);
            CheckFeatures(cpu, 0x0000C1A9, 0xBFE9FBFF, 0x00000000, 0x00100000);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) Duo CPU      T2700  @ 2.33GHz"));
        }

        [Test]
        public void Corei7_920()
        {
            GenuineIntelCpu cpu = GetCpu("i7-920.xml");
            CheckSignature(cpu, 0x106A5);
            CheckFeatures(cpu, 0x0098E3BD, 0xBFEBFBFF, 0x00000001, 0x28100800);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7 CPU         920  @ 2.67GHz"));
        }

        [Test]
        public void Xeon_W3540()
        {
            GenuineIntelCpu cpu = GetCpu("xeon-W3540.xml");
            CheckSignature(cpu, 0x106A5);
            CheckFeatures(cpu, 0x009CE3BD, 0xBFEBFBFF, 0x00000001, 0x28100800);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Xeon(R) CPU           W3540  @ 2.93GHz"));
        }

        [Test]
        public void Corei3_2120T()
        {
            GenuineIntelCpu cpu = GetCpu("i3-2120T.xml");
            CheckSignature(cpu, 0x206A7);
            CheckFeatures(cpu, 0x9C982203, 0x1FEBFBFF, 0x00000001, 0x28100800);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i3-2120T CPU @ 2.60GHz"));
        }

        [Test]
        public void Corei7_2630QM()
        {
            GenuineIntelCpu cpu = GetCpu("i7-2630QM.xml");
            CheckSignature(cpu, 0x206A7);
            CheckFeatures(cpu, 0x1FBAE3BF, 0xBFEBFBFF, 0x00000001, 0x28100800);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-2630QM CPU @ 2.00GHz"));
        }

        [Test]
        public void Corei5_3317U()
        {
            GenuineIntelCpu cpu = GetCpu("i5-3317U_SurfacePro.xml");
            CheckSignature(cpu, 0x306A9);
            CheckFeatures(cpu, 0x7FBAE3BF, 0xBFEBFBFF, 0x00000001, 0x28100800, 0x00000281, 0x00000000, 0x00000000);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i5-3317U CPU @ 1.70GHz"));
        }

        [Test]
        public void Corei7_3820QM()
        {
            GenuineIntelCpu cpu = GetCpu("i7-3820QM.xml");
            CheckSignature(cpu, 0x306A9);
            CheckFeatures(cpu, 0x7FBAE3FF, 0xBFEBFBFF, 0x00000001, 0x28100800, 0x00000281, 0x00000000, 0x00000000);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-3820QM CPU @ 2.70GHz"));
        }

        [Test]
        public void Corei7_4930K()
        {
            GenuineIntelCpu cpu = GetCpu("i7-4930K.xml");
            CheckSignature(cpu, 0x306E4);
            CheckFeatures(cpu, 0x7FBEE3BF, 0xBFEBFBFF, 0x00000001, 0x2C100800, 0x00000281, 0x00000000, 0x00000000);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-4930K CPU @ 3.40GHz"));
        }

        [Test]
        public void Corei7_6600U()
        {
            GenuineIntelCpu cpu = GetCpu("i7-6600U_SurfaceBook.xml");
            CheckSignature(cpu, 0x406E3);
            CheckFeatures(cpu, 0xFEDAF387, 0xBFEBFBFF, 0x00000121, 0x2C100800, 0x009C6FBB, 0x00000000, 0xBC000400);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-6600U CPU @ 2.60GHz"));
        }

        [Test]
        public void Corei7_6700K()
        {
            GenuineIntelCpu cpu = GetCpu("i7-6700K.xml");
            CheckSignature(cpu, 0x506E3);
            CheckFeatures(cpu, 0x7FFAFBBF, 0xBFEBFBFF, 0x00000121, 0x2C100800, 0x029C6FBF, 0x00000000, 0x9C002400);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-6700K CPU @ 4.00GHz"));
        }

        [Test]
        public void Corei7_9700()
        {
            GenuineIntelCpu cpu = GetCpu("i7-9700.xml");
            CheckSignature(cpu, 0x906ED);
            CheckFeatures(cpu, 0x7FFAFBFF, 0xBFEBFBFF, 0x00000121, 0x2C100800, 0x029C6FBF, 0x40000000, 0xBC000400);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i7-9700 CPU @ 3.00GHz"));
        }

        [Test]
        public void Corei9_10900K()
        {
            GenuineIntelCpu cpu = GetCpu("i9-10900K.xml");
            CheckSignature(cpu, 0xA0655);
            CheckFeatures(cpu, 0x7FFAFBFF, 0xBFEBFBFF, 0x00000121, 0x2C100000, 0x029C67AF, 0x40000008, 0xBC000400);
            Assert.That(cpu.Description, Is.EqualTo("Intel(R) Core(TM) i9-10900K CPU @ 3.70GHz"));
        }
    }
}
