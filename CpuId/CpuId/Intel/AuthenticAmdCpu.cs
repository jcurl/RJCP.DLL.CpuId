namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.Linq;

    /// <summary>
    /// Description of an AuthenticAMD CPU.
    /// </summary>
    public class AuthenticAmdCpu : GenericIntelCpuBase
    {
        // Reference Documents
        //  [6]   AMD, "AMD64 Architecture Programmers Manual, Volume 3: General-Purpose and System Instructions", Document #24594 Rev 3.35, June 2023
        //  [6a]  AMD, "AMD64 Architecture Programmers Manual, Volume 3: General-Purpose and System Instructions", Document #24594 Rev 3.20, May 2013
        //  [8]   AMD, "Reference PPR for AMD Family 17h Model 18h, Rev B1 Processors", Document #55570-B1 Rev 3.15, Jul 9, 2020
        //  [9]   AMD, "BIOS and Kernel Developer's Guide (BKDG) for AMD Family 15h Models 10h-1Fh Processors", Document #42300 Rev 3.12, July 14, 2015
        //  [10]  AMD, "Processor Programming Reference for AMD Family 17h Model 60h, Revision A1 Processors", Document #55922 Rev 3.06, September 28, 2020
        //  [17]  AMD, "Processor Recognition Application Note", Document #20734 Rev P, December 1999
        //  [17a] AMD, "Processor Recognition Application Note", Document #20734 Rev 3.00, April 2003

        internal const int CacheTlb = unchecked((int)0x80000005);
        internal const int CacheL2Tlb = unchecked((int)0x80000006);
        internal const int CacheTlb1G = unchecked((int)0x80000019);
        internal const int PerfOptIdent = unchecked((int)0x8000001A);
        internal const int CacheTopo = unchecked((int)0x8000001D);
        internal const int ProcessorTopo = unchecked((int)0x8000001E);

        private int m_ProcessorSignature;
        private int m_ExtendedFamily;
        private int m_ExtendedModel;
        private int m_ProcessorType;
        private int m_FamilyCode;
        private int m_ModelNumber;
        private int m_SteppingId;

        internal AuthenticAmdCpu(BasicCpu cpu) : base(cpu)
        {
            GetProcessorSignature(cpu);
            if (m_ProcessorSignature == 0) {
                Description = string.Empty;
                return;
            }

            ProcessorSignature = m_ProcessorSignature;

            if (m_FamilyCode != 0x0F) {
                Family = m_FamilyCode;
            } else {
                Family = m_ExtendedFamily + m_FamilyCode;
            }
            if (m_FamilyCode == 0x0F) {
                Model = (m_ExtendedModel << 4) + m_ModelNumber;
            } else {
                Model = m_ModelNumber;
            }

            Stepping = m_SteppingId;
            ProcessorType = m_ProcessorType;
            BrandString = AmdBrandIdentifier.GetType(this);
            Description = GetDescription();
            FindFeatures(cpu);
            GetCpuTopology(cpu);
            GetCacheTopology(cpu);
        }

        private void GetProcessorSignature(BasicCpu cpu)
        {
            if (cpu.FunctionCount == 0) return;

            CpuIdRegister feature = cpu.CpuRegisters.GetCpuId(FeatureInformationFunction, 0);
            if (feature == null) return;

            m_ProcessorSignature = feature.Result[0] & 0x0FFF3FFF;
            m_ExtendedFamily = (m_ProcessorSignature >> 20) & 0xFF;
            m_ExtendedModel = (m_ProcessorSignature >> 16) & 0xF;
            m_ProcessorType = (m_ProcessorSignature >> 12) & 0x3;
            m_FamilyCode = (m_ProcessorSignature >> 8) & 0xF;
            m_ModelNumber = (m_ProcessorSignature >> 4) & 0xF;
            m_SteppingId = m_ProcessorSignature & 0xF;
        }

        private string GetDescription()
        {
            string brandString = GetProcessorBrandString();
            if (brandString != null) return brandString;

            return BrandString;
        }

        private readonly static string[] FeaturesCpuId01Ecx = new string[] {
            "SSE3", "PCLMULQDQ", null, "MONITOR", null, null, null, null,
            null, "SSSE3", null, null, "FMA", "CMPXCHG16B", null, null,
            null, "PCID", null, "SSE4.1", "SSE4.2", "x2APIC", "MOVBE", "POPCNT",
            // HYPERVISOR = Wikipedia https://en.wikipedia.org/wiki/CPUID
            null, "AESNI", "XSAVE", "OSXSAVE", "AVX", "F16C", "RDRAND", "HYPERVISOR"
        };

        private readonly static string[] FeaturesCpuId01Edx = new string[] {
            "FPU", "VME", "DE", "PSE", "TSC", "MSR", "PAE", "MCE",
            "CX8", "", null, "SEP", "MTRR", "", "MCA", "CMOV",
            "PAT", "PSE-36", null, "CLFSH", null, null, null, "MMX",
            // IA64 = Wikipedia https://en.wikipedia.org/wiki/CPUID
            "FXSR", "SSE", "SSE2", null, "HTT", null, null, null
        };

        private readonly static string[] FeaturesCpuId07Ebx = new string[] {
            "FSGSBASE", null, null, "BMI1", null, "AVX2", null, "SMEP",
            "BMI2", null, "INVPCID", null, "PQM", null, null, "PQE",
            null, null, "RDSEED", "ADX", "SMAP", null, null, "CLFLUSHOPT",
            "CLWB", null, null, null, null, "SHA", null, null
        };

        private readonly static string[] FeaturesCpuId07Ecx = new string[] {
            null, null, "UMIP", "PKU", "OSPKE", null, null, "CET_SS",
            null, "VAES", "VPCLMULQDQ", null, null, null, null, null,
            // [6] has an error in the table CPUID Fn0000_0007_EBX_x0, CPUID EAX=07h, ECX=0, EBX[22] is not RDPID.
            // The instruction RPID, and table D-1, correctly specifies this register, itself specifies ECX[22].
            "LA57", null, null, null, null, null, "RDPID", null,
            "BUS_LOCK_DETECT", null, null, null, null, null, null, null
        };

        private readonly static string[] FeaturesCpuId0D01Eax = new string[] {
            "XSAVEOPT", "XSAVEC", "XGETBV", "XSAVES", null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null
        };

        private readonly static string[] FeaturesCpuId0D01Ecx = new string[] {
            null, null, null, null, null, null, null, null,
            null, null, null, "CET_U", "CET_S", null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null
        };

        private readonly static string[] FeaturesCpuId80000001Ecx = new string[] {
            // CMP = CmpLegacy
            "AHF64", "CMP", "SVM", "ExtApicSpace", "AM", "ABM", "SSE4A", "MisAlignSSE",
            "PREFETCHW", "OSVW", "IBS", "XOP", "SKINIT", "WDT", null, "LWP",
            // TOPX = TopologyExtensions
            "FMA4", "TCE", null, "NODEID" /* [9] */, null, "TBM", "TOPX", "PerfCtrExtCore",
            // StreamPerfMon = [6a], now reserved
            "PerfCtrExtNB", "StreamPerfMon", "DBE", "PerfTSC", "PerfL2I", "MONITORX", "ADMSK", null
        };

        private readonly static string[] FeaturesCpuId80000001Edx = new string[] {
            "FPU", "VME", "DE", "PSE", "TSC", "MSR", "PAE", "MCE",
            "CX8", "APIC", null, "SYSCALL", "MTRR", "PGE", "MCA", "CMOV",
            "PAT", "PSE-36", null, "MP", "XD", null, "MMXEXT", "MMX",
            "FXSR", "FFXSR", "1GB_PAGE", "RDTSCP", null, "LM", "3DNowExt", "3DNow"
        };

        private readonly static string[] FeaturesCpuId80000008Ebx = new string[] {
            // IRPERF = Instruction Retired Counter
            // ASRFPEP = Error Pointer Zero/Restore
            "CLZERO", "IRPERF", "ASRFPEP", "INVLPGB", "RDPRU", null, "MBE", null,
            "MCOMMIT", "WBNOINVD", null, null, "IBPB", "INT_WBINVD", "IBRS", "STIBP",
            // PPIN = [10]
            "IBRS_ALL", "STIBP_ALL", "IBRS_PREF", "IBRS_SMP", "EFER.LMSLE", "INVLPGB_NESTED", null, "PPIN",
            "SSBD", "SSBD_VirtSpecCtrl", "SSBD_NotRequired", "CPPC", "PSFD", "BTC_NO", "IPBP_RET", null
        };

        private readonly static string[] FeaturesCpuId8000001AEax = new string[] {
            "FP128", "MOVU", "FP256", null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null
        };

        private readonly static string[] FeaturesCpuId8000001FEax = new string[] {
            "SME", "SEV", "PageFlushMsr", "SEV-ES", "SEV-SNP", "VMPL", "RMPQUERY", "VmplSSS",
            "SecureTsc", "TscAuxVirtualization", "HwEnvCacheCoh", "SEV-64", "RestrictedInjection", "AlternateInjection", "DebugSwap", "PreventHostIbs",
            "VTE", "VmgexitParameter", "VirtualTomMsr", "IbsVirtGuestCtl", null, null, null, null,
            "VmsaRegProt", "SmtProtection", null, null, "SvsmCommPageMSR", "NestedVirtSnpMsr", null, null
        };

        private void FindFeatures(BasicCpu cpu)
        {
            if (cpu.FunctionCount < FeatureInformationFunction) return;

            CpuIdRegister features = cpu.CpuRegisters.GetCpuId(FeatureInformationFunction, 0);
            if (features != null) {
                TestFeatures(FeaturesCpuId01Edx, FeatureGroup.StandardFeatures, features, 3);
                if (Family == 5 && Model == 0) {
                    // [17] says AMD K5 Model 0, bit 9 is for PGE, where bit 13 is reserved.
                    TestFeature("PGE", FeatureGroup.StandardFeatures, features, 3, 9);
                    ReservedFeature(FeatureGroup.StandardFeatures, features, 3, unchecked((int)0xE8742400));
                } else {
                    TestFeature("APIC", FeatureGroup.StandardFeatures, features, 3, 9);
                    TestFeature("PGE", FeatureGroup.StandardFeatures, features, 3, 13);
                    ReservedFeature(FeatureGroup.StandardFeatures, features, 3, unchecked((int)0xE8740400));
                }

                TestFeatures(FeaturesCpuId01Ecx, FeatureGroup.StandardFeatures, features, 2);
                ReservedFeature(FeatureGroup.StandardFeatures, features, 2, 0x0105CDF4);
            }

            FindExtendedFeatures(cpu);

            if (cpu.FunctionCount < ExtendedFeatureFunction) return;
            CpuIdRegister features7 = cpu.CpuRegisters.GetCpuId(ExtendedFeatureFunction, 0);
            if (features7 != null) {
                TestFeatures(FeaturesCpuId07Ebx, FeatureGroup.StructuredExtendedFeatures, features7, 1);
                ReservedFeature(FeatureGroup.StructuredExtendedFeatures, features7, 1, unchecked((int)0xDE636A56));

                TestFeatures(FeaturesCpuId07Ecx, FeatureGroup.StructuredExtendedFeatures, features7, 2);
                ReservedFeature(FeatureGroup.StructuredExtendedFeatures, features7, 2, unchecked((int)0xFEBEF963));

                ReservedFeature(FeatureGroup.StructuredExtendedFeatures, features7, 3, unchecked((int)0xFFFFFFFF));

                if (features7.Result[0] > 0) {
                    for (int subfunction = 1; subfunction < features7.Result[0]; subfunction++) {
                        CpuIdRegister features7sX = cpu.CpuRegisters.GetCpuId(ExtendedFeatureFunction, subfunction);
                        if (features7sX != null) {
                            ReservedFeature(FeatureGroup.StructuredExtendedFeatures, features7sX, 0, unchecked((int)0xFFFFFFFF));
                            ReservedFeature(FeatureGroup.StructuredExtendedFeatures, features7sX, 1, unchecked((int)0xFFFFFFFF));
                            ReservedFeature(FeatureGroup.StructuredExtendedFeatures, features7sX, 2, unchecked((int)0xFFFFFFFF));
                            ReservedFeature(FeatureGroup.StructuredExtendedFeatures, features7sX, 3, unchecked((int)0xFFFFFFFF));
                        }
                    }
                }
            }

            if (cpu.FunctionCount < ExtendedProcessorState) return;
            CpuIdRegister features13 = cpu.CpuRegisters.GetCpuId(ExtendedProcessorState, 1);
            if (features13 != null) {
                TestFeatures(FeaturesCpuId0D01Eax, FeatureGroup.ExtendedState, features13, 0);
                ReservedFeature(FeatureGroup.ExtendedState, features13, 0, unchecked((int)0xFFFFFFF0));

                TestFeatures(FeaturesCpuId0D01Ecx, FeatureGroup.ExtendedState, features13, 2);
                ReservedFeature(FeatureGroup.ExtendedState, features13, 2, unchecked((int)0xFFFFE7FF));
            }
        }

        private void FindExtendedFeatures(BasicCpu cpu)
        {
            if (cpu.ExtendedFunctionCount < 1) return;

            CpuIdRegister extfeat = cpu.CpuRegisters.GetCpuId(ExtendedInformationFunction, 0);
            if (extfeat != null) {
                TestFeatures(FeaturesCpuId80000001Ecx, FeatureGroup.ExtendedFeatures, extfeat, 2);
                ReservedFeature(FeatureGroup.ExtendedFeatures, extfeat, 2, unchecked((int)0x80144000));

                TestFeatures(FeaturesCpuId80000001Edx, FeatureGroup.ExtendedFeatures, extfeat, 3);
                // 3DNowPrefetch ECX[8] || EDX[29] || EDX[31]
                Features["PREFETCHW"].Value |= Features["LM"].Value || Features["3DNow"].Value;
                ReservedFeature(FeatureGroup.ExtendedFeatures, extfeat, 3, 0x10240400);
            }

            if (cpu.ExtendedFunctionCount < ExtendedFeatureIds - MaxExtendedFunction) return;
            CpuIdRegister extfeat8 = cpu.CpuRegisters.GetCpuId(ExtendedFeatureIds, 0);
            if (extfeat8 != null) {
                TestFeatures(FeaturesCpuId80000008Ebx, FeatureGroup.ExtendedFeaturesIdentifiers, extfeat8, 1);
                ReservedFeature(FeatureGroup.ExtendedFeaturesIdentifiers, extfeat8, 1, unchecked((int)0x80400CA0));
            }

            if (cpu.ExtendedFunctionCount < PerfOptIdent - MaxExtendedFunction) return;
            CpuIdRegister extfeat1a = cpu.CpuRegisters.GetCpuId(PerfOptIdent, 0);
            if (extfeat1a != null) {
                TestFeatures(FeaturesCpuId8000001AEax, FeatureGroup.PerformanceOptimizations, extfeat1a, 0);
                ReservedFeature(FeatureGroup.PerformanceOptimizations, extfeat1a, 0, unchecked((int)0xFFFFFFF8));
            }

            if (cpu.ExtendedFunctionCount < ExtendedEncMem - MaxExtendedFunction) return;
            CpuIdRegister extfeat1f = cpu.CpuRegisters.GetCpuId(ExtendedEncMem, 0);
            if (extfeat1f != null) {
                TestFeatures(FeaturesCpuId8000001FEax, FeatureGroup.EncryptedMemory, extfeat1f, 0);
                ReservedFeature(FeatureGroup.EncryptedMemory, extfeat1f, 0, unchecked((int)0xCCF00000));
            }
        }

        /// <inheritdoc/>
        public override CpuVendor CpuVendor
        {
            get { return CpuVendor.AuthenticAmd; }
        }

        private void GetCpuTopology(BasicCpu cpu)
        {
            CpuIdRegister apic = cpu.CpuRegisters.GetCpuId(FeatureInformationFunction, 0);
            if (!Features["HTT"].Value || !Features["CMP"].Value || cpu.ExtendedFunctionCount < ExtendedFeatureIds - MaxExtendedFunction) {
                Topology.ApicId = (apic.Result[1] >> 24) & 0xFF;
                Topology.CoreTopology.Add(new CpuTopo(0, CpuTopoType.Core, 0));
                Topology.CoreTopology.Add(new CpuTopo(Topology.ApicId, CpuTopoType.Package, -1));
                return;
            }

            CpuIdRegister extFeatureIds = cpu.CpuRegisters.GetCpuId(ExtendedFeatureIds, 0);
            int apicIdSize = (extFeatureIds.Result[2] >> 12) & 0xF;
            int coreBits;
            if (apicIdSize == 0) {
                // There are two possible places to get this:
                // - from LogicalProcessorCount = CPUID(01h).EBX[23:16] (Intel); or
                // - from NC = CPUID(80000008h).ECX[7:0] (from AMD).
                int mnlp = (extFeatureIds.Result[2] >> 16) & 0xF + 1;
                coreBits = Log2Pof2(mnlp);
            } else {
                coreBits = apicIdSize;
            }
            long coreMask = ~(-1 << coreBits);
            long pkgMask = ~coreMask;

            if (!Features["TOPX"].Value || cpu.ExtendedFunctionCount < ProcessorTopo - MaxExtendedFunction) {
                Topology.ApicId = (apic.Result[1] >> 24) & 0xFF;
                Topology.CoreTopology.Add(new CpuTopo(Topology.ApicId & coreMask, CpuTopoType.Core, coreMask));
                Topology.CoreTopology.Add(new CpuTopo(Topology.ApicId >> coreBits, CpuTopoType.Package, pkgMask));
                return;
            }

            // Topology Extensions
            CpuIdRegister topoCpu = cpu.CpuRegisters.GetCpuId(ProcessorTopo, 0);
            Topology.ApicId = topoCpu.Result[0];
            int smt = ((topoCpu.Result[1] >> 8) & 0xFF) + 1;
            int smtBits = Log2Pof2(smt);
            int smtMask = ~(-1 << smtBits);

            int core = topoCpu.Result[1] & 0xFF;

            // [6] describes NodesPerProcessor, but doesn't describe how to calculate the actual socket number, which
            // this software provides as the "Package". So the Package for now may not reflect the number of sockets if
            // NodesPerProcessor is non-zero (1 node per processor).

            int die = topoCpu.Result[2] & 0xFF;  // AMD calls this the NodeId.
            Topology.CoreTopology.Add(new CpuTopo(Topology.ApicId & smtMask, CpuTopoType.Smt, smtMask));
            Topology.CoreTopology.Add(new CpuTopo(core, CpuTopoType.Core, coreMask));
            Topology.CoreTopology.Add(new CpuTopo(die, CpuTopoType.Node, coreMask));
            Topology.CoreTopology.Add(new CpuTopo(Topology.ApicId >> coreBits, CpuTopoType.Package, pkgMask));
        }

        private void GetCacheTopology(BasicCpu cpu)
        {
            if (Features["TOPX"].Value && cpu.ExtendedFunctionCount >= CacheTopo - MaxExtendedFunction) {
                GetCacheTopologyLeaf(CacheTopo);
            }

            if (!HasCacheType(1, CacheType.Instruction)) {
                GetLegacyCacheL1Instruction(cpu);
            }

            if (!HasCacheType(1, CacheType.Data)) {
                GetLegacyCacheL1Data(cpu);
            }

            if (!HasCacheType(2, CacheType.Unified)) {
                GetLegacyCacheL2Unified(cpu);
            }

            if (!HasCacheType(3, CacheType.Unified)) {
                GetLegacyCacheL3Unified(cpu);
            }

            GetLegacyCacheL1InstructionTlb(cpu);
            GetLegacyCacheL1DataTlb(cpu);
            GetLegacyCacheL2InstructionTlb(cpu);
            GetLegacyCacheL2DataTlb(cpu);
        }

        private bool HasCacheType(int level, CacheType cacheType)
        {
            int elements = Topology.CacheTopology.Count(t => {
                return t.Level == level && ((int)t.CacheType & (int)CacheType.TypeMask) == (int)cacheType;
            });
            return elements > 0;
        }

        private void GetLegacyCacheL1Data(BasicCpu cpu)
        {
            if (cpu.ExtendedFunctionCount < CacheTlb - MaxExtendedFunction) return;
            CpuIdRegister cacheTlb = cpu.CpuRegisters.GetCpuId(CacheTlb, 0);
            if (cacheTlb == null) return;

            int ways = (cacheTlb.Result[2] >> 16) & 0xFF;
            if (ways == 0) return;

            if (ways == 255) ways = 0;
            int lineSize = cacheTlb.Result[2] & 0xFF;
            int linesPerTag = (cacheTlb.Result[2] >> 8) & 0xFF;
            int size = (cacheTlb.Result[2] >> 24) & 0xFF;
            Topology.CacheTopology.Add(new CacheTopoCpu(1, CacheType.Data, ways, lineSize * linesPerTag, size));
        }

        private void GetLegacyCacheL1Instruction(BasicCpu cpu)
        {
            if (cpu.ExtendedFunctionCount < CacheTlb - MaxExtendedFunction) return;
            CpuIdRegister cacheTlb = cpu.CpuRegisters.GetCpuId(CacheTlb, 0);
            if (cacheTlb == null) return;

            int ways = (cacheTlb.Result[3] >> 16) & 0xFF;
            if (ways == 0) return;

            if (ways == 255) ways = 0;
            int lineSize = cacheTlb.Result[3] & 0xFF;
            int linesPerTag = (cacheTlb.Result[3] >> 8) & 0xFF;
            int size = (cacheTlb.Result[3] >> 24) & 0xFF;
            Topology.CacheTopology.Add(new CacheTopoCpu(1, CacheType.Instruction, ways, lineSize * linesPerTag, size));
        }

        private static int GetL2Associativity(int associativity)
        {
            switch (associativity) {
            case 0: return -1;    // Disabled.
            case 1: return 1;
            case 2: return 2;
            case 3: return 3;
            case 4: return 4;
            case 5: return 6;
            case 6: return 8;
            case 8: return 16;
            case 9: return -2;    // Get from 8000001Dh.
            case 10: return 32;
            case 11: return 48;
            case 12: return 64;
            case 13: return 96;
            case 14: return 128;
            case 15: return 0;
            default: return -1;   // Reserved.
            }
        }

        private void GetLegacyCacheL2Unified(BasicCpu cpu)
        {
            if (cpu.ExtendedFunctionCount < CacheL2Tlb - MaxExtendedFunction) return;
            CpuIdRegister cacheL2 = cpu.CpuRegisters.GetCpuId(CacheL2Tlb, 0);
            if (cacheL2 == null) return;

            int ways = GetL2Associativity((cacheL2.Result[2] >> 12) & 0xF);
            if (ways < 0) return;
            int lineSize = cacheL2.Result[2] & 0xFF;
            int linesPerTag = (cacheL2.Result[2] >> 8) & 0xF;
            int size = (cacheL2.Result[2] >> 16) & 0xFFFF;
            Topology.CacheTopology.Add(new CacheTopoCpu(2, CacheType.Unified, ways, lineSize * linesPerTag, size));
        }

        private void GetLegacyCacheL3Unified(BasicCpu cpu)
        {
            if (cpu.ExtendedFunctionCount < CacheL2Tlb - MaxExtendedFunction) return;
            CpuIdRegister cacheL3 = cpu.CpuRegisters.GetCpuId(CacheL2Tlb, 0);
            if (cacheL3 == null) return;

            int ways = GetL2Associativity((cacheL3.Result[3] >> 12) & 0xF);
            if (ways < 0) return;
            int lineSize = cacheL3.Result[3] & 0xFF;
            int linesPerTag = (cacheL3.Result[3] >> 8) & 0xF;
            int size = (cacheL3.Result[3] >> 18) & 0x3FFF;
            Topology.CacheTopology.Add(new CacheTopoCpu(3, CacheType.Unified, ways, lineSize * linesPerTag, size * 512));
        }

        private void GetLegacyCacheL1DataTlb(BasicCpu cpu)
        {
            if (cpu.ExtendedFunctionCount < CacheTlb - MaxExtendedFunction) return;
            CpuIdRegister cacheTlb = cpu.CpuRegisters.GetCpuId(CacheTlb, 0);
            if (cacheTlb == null) return;

            int ways2m = (cacheTlb.Result[0] >> 24) & 0xFF;
            if (ways2m != 0) {
                if (ways2m == 255) ways2m = 0;
                int entries2m = (cacheTlb.Result[0] >> 16) & 0xFF;
                Topology.CacheTopology.Add(new CacheTopoTlb(1, CacheType.DataTlb2M4M, ways2m, entries2m));
            }

            int ways4k = (cacheTlb.Result[1] >> 24) & 0xFF;
            if (ways4k != 0) {
                if (ways4k == 255) ways4k = 0;
                int entries4k = (cacheTlb.Result[1] >> 16) & 0xFF;
                Topology.CacheTopology.Add(new CacheTopoTlb(1, CacheType.DataTlb4k, ways4k, entries4k));
            }

            if (cpu.ExtendedFunctionCount < CacheTlb1G - MaxExtendedFunction) return;
            CpuIdRegister cacheTlb1g = cpu.CpuRegisters.GetCpuId(CacheTlb1G, 0);
            if (cacheTlb1g == null) return;

            int ways1g = GetL2Associativity((cacheTlb1g.Result[0] >> 28) & 0xF);
            if (ways1g >= 0) {
                int entries1g = (cacheTlb1g.Result[0] >> 16) & 0xFFF;
                Topology.CacheTopology.Add(new CacheTopoTlb(1, CacheType.DataTlb1G, ways1g, entries1g));
            }
        }

        private void GetLegacyCacheL1InstructionTlb(BasicCpu cpu)
        {
            if (cpu.ExtendedFunctionCount < CacheTlb - MaxExtendedFunction) return;
            CpuIdRegister cacheTlb = cpu.CpuRegisters.GetCpuId(CacheTlb, 0);
            if (cacheTlb == null) return;

            int ways2m = (cacheTlb.Result[0] >> 8) & 0xFF;
            if (ways2m != 0) {
                if (ways2m == 255) ways2m = 0;
                int entries2m = cacheTlb.Result[0] & 0xFF;
                Topology.CacheTopology.Add(new CacheTopoTlb(1, CacheType.InstructionTlb2M4M, ways2m, entries2m));
            }

            int ways4k = (cacheTlb.Result[1] >> 8) & 0xFF;
            if (ways4k != 0) {
                if (ways4k == 255) ways4k = 0;
                int entries4k = cacheTlb.Result[1] & 0xFF;
                Topology.CacheTopology.Add(new CacheTopoTlb(1, CacheType.InstructionTlb4k, ways4k, entries4k));
            }

            if (cpu.ExtendedFunctionCount < CacheTlb1G - MaxExtendedFunction) return;
            CpuIdRegister cacheTlb1g = cpu.CpuRegisters.GetCpuId(CacheTlb1G, 0);
            if (cacheTlb1g == null) return;

            int ways1g = GetL2Associativity((cacheTlb1g.Result[0] >> 12) & 0xF);
            if (ways1g >= 0) {
                int entries1g = cacheTlb1g.Result[0] & 0xFFF;
                Topology.CacheTopology.Add(new CacheTopoTlb(1, CacheType.InstructionTlb1G, ways1g, entries1g));
            }
        }

        private void GetLegacyCacheL2DataTlb(BasicCpu cpu)
        {
            if (cpu.ExtendedFunctionCount < CacheL2Tlb - MaxExtendedFunction) return;
            CpuIdRegister cacheL2Tlb = cpu.CpuRegisters.GetCpuId(CacheL2Tlb, 0);
            if (cacheL2Tlb == null) return;

            int ways2m = GetL2Associativity((cacheL2Tlb.Result[0] >> 28) & 0xF);
            if (ways2m >= 0) {
                int entries2m = (cacheL2Tlb.Result[0] >> 16) & 0xFFF;
                Topology.CacheTopology.Add(new CacheTopoTlb(2, CacheType.DataTlb2M4M, ways2m, entries2m));
            }

            int ways4k = GetL2Associativity((cacheL2Tlb.Result[1] >> 28) & 0xF);
            if (ways4k >= 0) {
                int entries4k = (cacheL2Tlb.Result[1] >> 16) & 0xFFF;
                Topology.CacheTopology.Add(new CacheTopoTlb(2, CacheType.DataTlb4k, ways4k, entries4k));
            }

            if (cpu.ExtendedFunctionCount < CacheTlb1G - MaxExtendedFunction) return;
            CpuIdRegister cacheTlb1g = cpu.CpuRegisters.GetCpuId(CacheTlb1G, 0);
            if (cacheTlb1g == null) return;

            int ways1g = GetL2Associativity((cacheTlb1g.Result[1] >> 28) & 0xF);
            if (ways1g >= 0) {
                int entries1g = (cacheTlb1g.Result[1] >> 16) & 0xFFF;
                Topology.CacheTopology.Add(new CacheTopoTlb(2, CacheType.DataTlb1G, ways1g, entries1g));
            }
        }

        private void GetLegacyCacheL2InstructionTlb(BasicCpu cpu)
        {
            if (cpu.ExtendedFunctionCount < CacheL2Tlb - MaxExtendedFunction) return;
            CpuIdRegister cacheL2Tlb = cpu.CpuRegisters.GetCpuId(CacheL2Tlb, 0);
            if (cacheL2Tlb == null) return;

            int ways2m = GetL2Associativity((cacheL2Tlb.Result[0] >> 12) & 0xF);
            if (ways2m >= 0) {
                int entries2m = cacheL2Tlb.Result[0] & 0xFFF;
                Topology.CacheTopology.Add(new CacheTopoTlb(2, CacheType.InstructionTlb2M4M, ways2m, entries2m));
            }

            int ways4k = GetL2Associativity((cacheL2Tlb.Result[1] >> 12) & 0xF);
            if (ways4k >= 0) {
                int entries4k = cacheL2Tlb.Result[1] & 0xFFF;
                Topology.CacheTopology.Add(new CacheTopoTlb(2, CacheType.InstructionTlb4k, ways4k, entries4k));
            }

            if (cpu.ExtendedFunctionCount < CacheTlb1G - MaxExtendedFunction) return;
            CpuIdRegister cacheTlb1g = cpu.CpuRegisters.GetCpuId(CacheTlb1G, 0);
            if (cacheTlb1g == null) return;

            int ways1g = GetL2Associativity((cacheTlb1g.Result[1] >> 12) & 0xF);
            if (ways1g >= 0) {
                int entries1g = cacheTlb1g.Result[0] & 0xFFF;
                Topology.CacheTopology.Add(new CacheTopoTlb(2, CacheType.InstructionTlb1G, ways1g, entries1g));
            }
        }
    }
}
