namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.Linq;

    /// <summary>
    /// Description of an AuthenticAMD CPU.
    /// </summary>
    public class AuthenticAmdCpu : GenericIntelCpuBase
    {
        // Reference Documents
        //  [6]  AMD, "AMD64 Architecture Programmers Manual, Volume 3: General-Purpose and System Instructions", Document #24594 Rev 3.31, October 2020
        //  [6a] AMD, "AMD64 Architecture Programmers Manual, Volume 3: General-Purpose and System Instructions", Document #24594 Rev 3.20, May 2013
        //  [8]  AMD, "Reference PPR for AMD Family 17h Model 18h, Rev B1 Processors", Document #55570-B1 Rev 3.15, Jul 9, 2020
        //  [9]  AMD, "BIOS and Kernel Developer's Guide (BKDG) for AMD Family 15h Models 10h-1Fh Processors", Document #42300 Rev 3.12, July 14, 2015
        //  [10] AMD, "Processor Programming Reference for AMD Family 17h Model 60h, Revision A1 Processors", Document #55922 Rev 3.06, September 28, 2020
        //  [17] AMD, "Processor Recognition Application Note", Document #20734 Rev P, December 1999

        internal const int CacheTlb = unchecked((int)0x80000005);
        internal const int CacheL2Tlb = unchecked((int)0x80000006);
        internal const int CacheTlb1G = unchecked((int)0x80000019);
        internal const int ExtendedFeatureIds = unchecked((int)0x80000008);
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

        private void FindFeatures(BasicCpu cpu)
        {
            if (cpu.FunctionCount < FeatureInformationFunction) return;

            CpuIdRegister features = cpu.CpuRegisters.GetCpuId(FeatureInformationFunction, 0);
            if (features != null) {
                TestFeature("FPU", features, 3, 0);
                TestFeature("VME", features, 3, 1);
                TestFeature("DE", features, 3, 2);
                TestFeature("PSE", features, 3, 3);
                TestFeature("TSC", features, 3, 4);
                TestFeature("MSR", features, 3, 5);
                TestFeature("PAE", features, 3, 6);
                TestFeature("MCE", features, 3, 7);
                TestFeature("CX8", features, 3, 8);
                TestFeature("SEP", features, 3, 11);
                TestFeature("MTRR", features, 3, 12);
                TestFeature("MCA", features, 3, 14);
                TestFeature("CMOV", features, 3, 15);
                TestFeature("PAT", features, 3, 16);
                TestFeature("PSE-36", features, 3, 17);
                TestFeature("CLFSH", features, 3, 19);
                TestFeature("MMX", features, 3, 23);
                TestFeature("FXSR", features, 3, 24);
                TestFeature("SSE", features, 3, 25);
                TestFeature("SSE2", features, 3, 26);
                TestFeature("HTT", features, 3, 28);
                if (Family == 5 && Model == 0) {
                    // [17] says AMD K5 Model 0, bit 9 is for PGE, where bit 13 is reserved.
                    TestFeature("PGE", features, 3, 9);
                } else {
                    TestFeature("APIC", features, 3, 9);
                    TestFeature("PGE", features, 3, 13);
                }
                ReservedFeature(features, 3, unchecked((int)0xE8740400));

                TestFeature("SSE3", features, 2, 0);
                TestFeature("PCLMULQDQ", features, 2, 1);
                TestFeature("MONITOR", features, 2, 3);
                TestFeature("SSSE3", features, 2, 9);
                TestFeature("FMA", features, 2, 12);
                TestFeature("CMPXCHG16B", features, 2, 13);
                TestFeature("PCID", features, 2, 17);
                TestFeature("SSE4.1", features, 2, 19);
                TestFeature("SSE4.2", features, 2, 20);
                TestFeature("x2APIC", features, 2, 21);                         // [8]
                TestFeature("MOVBE", features, 2, 22);
                TestFeature("POPCNT", features, 2, 23);
                TestFeature("AESNI", features, 2, 25);
                TestFeature("XSAVE", features, 2, 26);
                TestFeature("OSXSAVE", features, 2, 27);
                TestFeature("AVX", features, 2, 28);
                TestFeature("F16C", features, 2, 29);
                TestFeature("RDRAND", features, 2, 30);
                TestFeature("HYPERVISOR", features, 2, 31);
                ReservedFeature(features, 2, 0x0105CDF4);
            }

            FindExtendedFeatures(cpu);

            if (cpu.FunctionCount < ExtendedFeatureFunction) return;
            CpuIdRegister features7 = cpu.CpuRegisters.GetCpuId(ExtendedFeatureFunction, 0);
            if (features7 != null) {
                TestFeature("FSGSBASE", features7, 1, 0);
                TestFeature("BMI1", features7, 1, 3);
                TestFeature("AVX2", features7, 1, 5);
                TestFeature("SMEP", features7, 1, 7);
                TestFeature("BMI2", features7, 1, 8);
                TestFeature("INVPCID", features7, 1, 10);
                TestFeature("PQM", features7, 1, 12);
                TestFeature("PQE", features7, 1, 15);
                TestFeature("RDSEED", features7, 1, 18);
                TestFeature("ADX", features7, 1, 19);
                TestFeature("SMAP", features7, 1, 20);
                // [6] has an error, bit 22 is not RDPID as given on p606
                TestFeature("CLFLUSHOPT", features7, 1, 23);
                TestFeature("CLWB", features7, 1, 24);
                TestFeature("SHA", features7, 1, 29);
                ReservedFeature(features7, 1, unchecked((int)0xDE636A56));

                TestFeature("UMIP", features7, 2, 2);
                TestFeature("PKU", features7, 2, 3);
                TestFeature("OSPKE", features7, 2, 4);
                TestFeature("CET_SS", features7, 2, 7);
                TestFeature("VAES", features7, 2, 9);
                TestFeature("VPCLMULQDQ", features7, 2, 10);
                ReservedFeature(features7, 2, unchecked((int)0xFFBFF963));

                TestFeature("RDPID", features7, 3, 22);
                ReservedFeature(features7, 3, unchecked((int)0xFFBFFFFF));

                if (features7.Result[0] > 0) {
                    for (int subfunction = 1; subfunction < features7.Result[0]; subfunction++) {
                        CpuIdRegister features7sX = cpu.CpuRegisters.GetCpuId(ExtendedFeatureFunction, subfunction);
                        if (features7sX != null) {
                            ReservedFeature(features7sX, 0, unchecked((int)0xFFFFFFFF));
                            ReservedFeature(features7sX, 1, unchecked((int)0xFFFFFFFF));
                            ReservedFeature(features7sX, 2, unchecked((int)0xFFFFFFFF));
                            ReservedFeature(features7sX, 3, unchecked((int)0xFFFFFFFF));
                        }
                    }
                }
            }

            if (cpu.FunctionCount < ExtendedProcessorState) return;
            CpuIdRegister features13 = cpu.CpuRegisters.GetCpuId(ExtendedProcessorState, 1);
            if (features13 != null) {
                TestFeature("XSAVEOPT", features13, 0, 0);
                TestFeature("XSAVEC", features13, 0, 1);
                TestFeature("XGETBV", features13, 0, 2);
                TestFeature("XSAVES", features13, 0, 3);
            }
        }

        private void FindExtendedFeatures(BasicCpu cpu)
        {
            if (cpu.ExtendedFunctionCount < 1) return;

            CpuIdRegister extfeat = cpu.CpuRegisters.GetCpuId(ExtendedInformationFunction, 0);
            if (extfeat != null) {
                TestFeature("AHF64", extfeat, 2, 0);
                TestFeature("CMP", extfeat, 2, 1);                   // CmpLegacy
                TestFeature("SVM", extfeat, 2, 2);
                TestFeature("ExtApicSpace", extfeat, 2, 3);
                TestFeature("AM", extfeat, 2, 4);
                TestFeature("ABM", extfeat, 2, 5);
                TestFeature("SSE4A", extfeat, 2, 6);
                TestFeature("MisAlignSSE", extfeat, 2, 7);
                TestFeature("PREFETCHW", extfeat, 2, 8);
                TestFeature("OSVW", extfeat, 2, 9);
                TestFeature("IBS", extfeat, 2, 10);
                TestFeature("XOP", extfeat, 2, 11);
                TestFeature("SKINIT", extfeat, 2, 12);
                TestFeature("WDT", extfeat, 2, 13);
                TestFeature("LWP", extfeat, 2, 15);
                TestFeature("FMA4", extfeat, 2, 16);
                TestFeature("TCE", extfeat, 2, 17);                  // [9]
                TestFeature("NODEID", extfeat, 2, 19);               // [9]
                TestFeature("TBM", extfeat, 2, 21);                  // [9]
                TestFeature("TOPX", extfeat, 2, 22);                 // TopologyExtensions
                TestFeature("PerfCtrExtCore", extfeat, 2, 23);
                TestFeature("PerfCtrExtNB", extfeat, 2, 24);
                TestFeature("StreamPerfMon", extfeat, 2, 25);        // [6a]
                TestFeature("DBE", extfeat, 2, 26);
                TestFeature("PerfTSC", extfeat, 2, 27);
                TestFeature("PerfL2I", extfeat, 2, 28);              // Sandpile.org
                TestFeature("MONITORX", extfeat, 2, 29);
                TestFeature("ADMSK", extfeat, 2, 30);                // [10]
                ReservedFeature(extfeat, 2, unchecked((int)0x80144000));

                TestFeature("SYSCALL", extfeat, 3, 11);
                TestFeature("MP", extfeat, 3, 19);                   // Sandpile.org
                TestFeature("XD", extfeat, 3, 20);
                TestFeature("MMXEXT", extfeat, 3, 22);
                TestFeature("FFXSR", extfeat, 3, 25);
                TestFeature("1GB_PAGE", extfeat, 3, 26);
                TestFeature("RDTSCP", extfeat, 3, 27);
                TestFeature("LM", extfeat, 3, 29);
                TestFeature("3DNowExt", extfeat, 3, 30);
                TestFeature("3DNow", extfeat, 3, 31);
                // 3DNowPrefetch ECX[8] || EDX[29] || EDX[31]
                Features["PREFETCHW"] |= Features["LM"] || Features["3DNow"];
                // The following are duplicated from 0000_0001h.EDX
                //  0: FPU       8: CX8      16: PAT      24: FXSR
                //  1: VME       9: APIC     17: PSE-36   25:
                //  2: DE       10: (RSVD)   18: (RSVD)   26:
                //  3: PSE      11: SEP      19:          27:
                //  4: TSC      12: MTRR     20:          28: (RSVD)
                //  5: MSR      13: PGE      21: (RSVD)   29:
                //  6: PAE      14: MCA      22:          30:
                //  7: MCE      15: CMOV     23: MMX      31:
                ReservedFeature(extfeat, 3, 0x10240400);
            }

            if (cpu.ExtendedFunctionCount < ExtendedLmApicId - MaxExtendedFunction) return;
            CpuIdRegister extfeat8 = cpu.CpuRegisters.GetCpuId(ExtendedLmApicId, 0);
            if (extfeat8 != null) {
                TestFeature("CLZERO", extfeat8, 1, 0);
                TestFeature("IRPERF", extfeat8, 1, 1);            // Instruction Retired Counter
                TestFeature("ASRFPEP", extfeat8, 1, 2);           // Error Pointer Zero/Restore
                TestFeature("INVLPGB", extfeat8, 1, 3);
                TestFeature("RDPRU", extfeat8, 1, 4);
                TestFeature("MBE", extfeat8, 1, 6);               // [10]
                TestFeature("MCOMMIT", extfeat8, 1, 8);
                TestFeature("WBNOINVD", extfeat8, 1, 9);
                TestFeature("IBPB", extfeat8, 1, 12);             // [10]
                TestFeature("INT_WBINVD", extfeat8, 1, 13);       // [10]
                TestFeature("IBRS", extfeat8, 1, 14);             // [10]
                TestFeature("STIBP", extfeat8, 1, 15);            // [10]
                TestFeature("IBRS_ALL", extfeat8, 1, 16);         // Sandpile.org
                TestFeature("STIBP_ALL", extfeat8, 1, 17);        // [10]
                TestFeature("IBRS_PREF", extfeat8, 1, 18);        // [10]
                TestFeature("IBRS_SMP", extfeat8, 1, 19);         // [10]
                TestFeature("EFER.LMSLE", extfeat8, 1, 20);
                TestFeature("INVLPGB_NESTED", extfeat8, 1, 21);
                TestFeature("PPIN", extfeat8, 1, 23);             // [10]
                TestFeature("SSBD", extfeat8, 1, 24);             // [10]
                ReservedFeature(extfeat8, 1, unchecked((int)0xFE400CA0));
            }

            if (cpu.ExtendedFunctionCount < ExtendedEncMem - MaxExtendedFunction) return;
            CpuIdRegister extfeat1f = cpu.CpuRegisters.GetCpuId(ExtendedEncMem, 0);
            if (extfeat1f != null) {
                TestFeature("SME", extfeat1f, 0, 0);
                TestFeature("SEV", extfeat1f, 0, 1);
                TestFeature("PageFlushMsr", extfeat1f, 0, 2);
                TestFeature("ES", extfeat1f, 0, 3);
                TestFeature("SNP", extfeat1f, 0, 4);
                TestFeature("VMPL", extfeat1f, 0, 5);
                TestFeature("VTE", extfeat1f, 0, 16);
                ReservedFeature(extfeat1f, 0, unchecked((int)0xFFFEFFC0));
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
            if (!Features["HTT"] || !Features["CMP"] || cpu.ExtendedFunctionCount < ExtendedFeatureIds - MaxExtendedFunction) {
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

            if (!Features["TOPX"] || cpu.ExtendedFunctionCount < ProcessorTopo - MaxExtendedFunction) {
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
            if (Features["TOPX"] && cpu.ExtendedFunctionCount >= CacheTopo - MaxExtendedFunction) {
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

            if (!HasCacheType(2, CacheType.Unified)) {
                GetLegacyCacheL3Unified(cpu);
            }

            GetLegacyCacheL1DataTlb(cpu);
            GetLegacyCacheL1InstructionTlb(cpu);
            GetLegacyCacheL2DataTlb(cpu);
            GetLegacyCacheL2InstructionTlb(cpu);
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

        private int GetL2Associativity(int associativity)
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
            int size = (cacheL3.Result[3] >> 16) & 0xFFFF;
            Topology.CacheTopology.Add(new CacheTopoCpu(2, CacheType.Unified, ways, lineSize * linesPerTag, size * 512));
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
