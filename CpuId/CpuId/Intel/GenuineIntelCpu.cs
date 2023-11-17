namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Description of a GenuineIntel CPU.
    /// </summary>
    public class GenuineIntelCpu : GenericIntelCpuBase
    {
        internal const int LegacyCache = 0x02;
        internal const int LegacyTopology = 0x04;
        internal const int ExtendedTopology = 0x0B;
        internal const int AddressTranslation = 0x18;
        internal const int ExtendedTopology2 = 0x1F;

        private int m_ProcessorSignature;
        private int m_ExtendedFamily;
        private int m_ExtendedModel;
        private int m_ProcessorType;
        private int m_FamilyCode;
        private int m_ModelNumber;
        private int m_SteppingId;
        private int m_Brand;

        internal GenuineIntelCpu(BasicCpu cpu) : base(cpu)
        {
            GetProcessorSignature(cpu);
            if (m_ProcessorSignature == 0) {
                Description = string.Empty;
                return;
            }

            ProcessorSignature = m_ProcessorSignature;

            // Calculations taken from Intel Developer Manual Volume 2, 325383-072US, May 2020
            if (m_FamilyCode != 0x0F) {
                Family = m_FamilyCode;
            } else {
                Family = m_ExtendedFamily + m_FamilyCode;
            }
            if (m_FamilyCode == 0x06 || m_FamilyCode == 0x0F) {
                Model = (m_ExtendedModel << 4) + m_ModelNumber;
            } else {
                Model = m_ModelNumber;
            }

            Stepping = m_SteppingId;
            ProcessorType = m_ProcessorType;
            BrandString = IntelLegacySignatures.GetType(m_ExtendedFamily, m_ExtendedModel, m_ProcessorType, m_FamilyCode, m_ModelNumber);
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

            int eax = feature.Result[0];
            int ebx = feature.Result[1];

            if ((eax & 0xf00) == 0x300) {
                // Sounds crazy, but someone's emulating an i386. Major Stepping is the Model property.
                m_ProcessorSignature = eax & 0xFFFF;
                m_ProcessorType = (m_ProcessorSignature >> 12) & 0xF;
                m_FamilyCode = (m_ProcessorSignature >> 8) & 0xF;
                m_ModelNumber = (m_ProcessorSignature >> 4) & 0xF;
                m_SteppingId = m_ProcessorSignature & 0xF;
            } else {
                m_ProcessorSignature = eax & 0x0FFF3FFF;
                m_ExtendedFamily = (m_ProcessorSignature >> 20) & 0xFF;
                m_ExtendedModel = (m_ProcessorSignature >> 16) & 0xF;
                m_ProcessorType = (m_ProcessorSignature >> 12) & 0x3;
                m_FamilyCode = (m_ProcessorSignature >> 8) & 0xF;
                m_ModelNumber = (m_ProcessorSignature >> 4) & 0xF;
                m_SteppingId = m_ProcessorSignature & 0xF;
                m_Brand = ebx & 0xFF;
            }
        }

        private string GetDescription()
        {
            // According to 241618-39, CPU ID Application Note 485, May 2012
            // 1. Check if bit 21 is settable in FLAGS (done, else we wouldn't be here).

            // 2. Check CPUID.80000000.EAX >= 800000004. If so, get branding information from 80000002-80000004.
            string brandString = GetProcessorBrandString();
            if (brandString != null) return brandString;

            // 3. Check CPUID.00000001.EBX bits 7:0 != 0. If so, get branding information as per table 7-1.
            switch (m_Brand) {
            case 0:
                break;
            case 1:
            case 10:
            case 20:
                return "Intel(R) Celeron(R) processor";
            case 2:
            case 4:
                return "Intel(R) Pentium(R) III processor";
            case 3:
                if (m_ProcessorSignature == 0x06B1) return "Intel(R) Celeron(R) processor";
                return "Intel(R) Pentium(R) III Xeon(R) processor";
            case 6:
                return "Mobile Intel(R) Pentium(R) III processor-M";
            case 7:
            case 15:
            case 19:
            case 23:
                return "Mobile Intel(R) Celeron(R) processor";
            case 8:
            case 9:
                return "Intel(R) Pentium(R) 4 processor";
            case 11:
                if (m_ProcessorSignature == 0x0F13) return "Intel(R) Xeon(R) processor MP";
                return "Intel(R) Xeon(R) processsor";
            case 12:
                return "Intel(R) Xeon(R) processor MP";
            case 14:
                if (m_ProcessorSignature == 0xF13) return "Intel(R) Xeon(R) processsor";
                return "Mobile Intel(R) Pentium(R) 4 processor-M";
            case 17:
            case 21:
                return "Mobile Genuine Intel(R) processor";
            case 18:
                return "Intel(R) Celeron(R) M processor";
            case 22:
                return "Intel(R) Pentium(R) M processor";
            default:
                return "Intel(R) Processor";
            }

            // If 3 fails; Check processor signature (table 5.3, 5.4) and section 5.1.3
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
                TestFeature("APIC", features, 3, 9);
                TestFeature("SEP", features, 3, 11);
                TestFeature("MTRR", features, 3, 12);
                TestFeature("PGE", features, 3, 13);
                TestFeature("MCA", features, 3, 14);
                TestFeature("CMOV", features, 3, 15);
                TestFeature("PAT", features, 3, 16);
                TestFeature("PSE-36", features, 3, 17);
                TestFeature("PSN", features, 3, 18);
                TestFeature("CLFSH", features, 3, 19);
                TestFeature("DS", features, 3, 21);
                TestFeature("ACPI", features, 3, 22);
                TestFeature("MMX", features, 3, 23);
                TestFeature("FXSR", features, 3, 24);
                TestFeature("SSE", features, 3, 25);
                TestFeature("SSE2", features, 3, 26);
                TestFeature("SS", features, 3, 27);
                TestFeature("HTT", features, 3, 28);
                TestFeature("TM", features, 3, 29);
                TestFeature("IA64", features, 3, 30);                // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("PBE", features, 3, 31);
                if (Features["SEP"] && ProcessorSignature < 0x633)
                    Features["SEP"] = false;
                ReservedFeature(features, 3, 0x00100400);

                TestFeature("SSE3", features, 2, 0);
                TestFeature("PCLMULQDQ", features, 2, 1);
                TestFeature("DTES64", features, 2, 2);
                TestFeature("MONITOR", features, 2, 3);
                TestFeature("DS-CPL", features, 2, 4);
                TestFeature("VMX", features, 2, 5);
                TestFeature("SMX", features, 2, 6);
                TestFeature("EIST", features, 2, 7);
                TestFeature("TM2", features, 2, 8);
                TestFeature("SSSE3", features, 2, 9);
                TestFeature("CNXT-ID", features, 2, 10);
                TestFeature("SDBG", features, 2, 11);
                TestFeature("FMA", features, 2, 12);
                TestFeature("CMPXCHG16B", features, 2, 13);
                TestFeature("xTPR", features, 2, 14);
                TestFeature("PDCM", features, 2, 15);
                TestFeature("PCID", features, 2, 17);
                TestFeature("DCA", features, 2, 18);
                TestFeature("SSE4.1", features, 2, 19);
                TestFeature("SSE4.2", features, 2, 20);
                TestFeature("x2APIC", features, 2, 21);
                TestFeature("MOVBE", features, 2, 22);
                TestFeature("POPCNT", features, 2, 23);
                TestFeature("TSC-DEADLINE", features, 2, 24);
                TestFeature("AESNI", features, 2, 25);
                TestFeature("XSAVE", features, 2, 26);
                TestFeature("OSXSAVE", features, 2, 27);
                TestFeature("AVX", features, 2, 28);
                TestFeature("F16C", features, 2, 29);
                TestFeature("RDRAND", features, 2, 30);
                TestFeature("HYPERVISOR", features, 2, 31);          // Wikipedia https://en.wikipedia.org/wiki/CPUID
                ReservedFeature(features, 2, 0x00010000);
            }

            FindExtendedFeatures(cpu);

            if (cpu.FunctionCount < ExtendedFeatureFunction) return;
            CpuIdRegister features7 = cpu.CpuRegisters.GetCpuId(ExtendedFeatureFunction, 0);
            if (features7 != null) {
                TestFeature("FSGSBASE", features7, 1, 0);
                TestFeature("IA32_TSC_ADJUST", features7, 1, 1);
                TestFeature("SGX", features7, 1, 2);
                TestFeature("BMI1", features7, 1, 3);
                TestFeature("HLE", features7, 1, 4);
                TestFeature("AVX2", features7, 1, 5);
                TestFeature("FDP_EXCPTN_ONLY", features7, 1, 6);
                TestFeature("SMEP", features7, 1, 7);
                TestFeature("BMI2", features7, 1, 8);
                TestFeature("ERMS", features7, 1, 9);
                TestFeature("INVPCID", features7, 1, 10);
                TestFeature("RTM", features7, 1, 11);
                TestFeature("PQM", features7, 1, 12);
                TestFeature("FPU-CS Dep", features7, 1, 13);
                TestFeature("MPX", features7, 1, 14);
                TestFeature("PQE", features7, 1, 15);
                TestFeature("AVX512F", features7, 1, 16);
                TestFeature("AVX512DQ", features7, 1, 17);
                TestFeature("RDSEED", features7, 1, 18);
                TestFeature("ADX", features7, 1, 19);
                TestFeature("SMAP", features7, 1, 20);
                TestFeature("AVX512_IFMA", features7, 1, 21);
                TestFeature("CLFLUSHOPT", features7, 1, 23);
                TestFeature("CLWB", features7, 1, 24);
                TestFeature("INTEL_PT", features7, 1, 25);
                TestFeature("AVX512PF", features7, 1, 26);
                TestFeature("AVX512ER", features7, 1, 27);
                TestFeature("AVX512CD", features7, 1, 28);
                TestFeature("SHA", features7, 1, 29);
                TestFeature("AVX512BW", features7, 1, 30);
                TestFeature("AVX512VL", features7, 1, 31);
                ReservedFeature(features7, 1, 0x00400000);

                TestFeature("PREFETCHWT1", features7, 2, 0);
                TestFeature("AVX512_VBMI", features7, 2, 1);
                TestFeature("UMIP", features7, 2, 2);
                TestFeature("PKU", features7, 2, 3);
                TestFeature("OSPKE", features7, 2, 4);
                TestFeature("WAITPKG", features7, 2, 5);
                TestFeature("AVX512_VBMI2", features7, 2, 6);
                TestFeature("CET_SS", features7, 2, 7);
                TestFeature("GFNI", features7, 2, 8);
                TestFeature("VAES", features7, 2, 9);
                TestFeature("VPCLMULQDQ", features7, 2, 10);
                TestFeature("AVX512_VNNI", features7, 2, 11);
                TestFeature("AVX512_BITALG", features7, 2, 12);
                TestFeature("TME_EN", features7, 2, 13);
                TestFeature("AVX512_VPOPCNTDQ", features7, 2, 14);
                TestFeature("5L_PAGE", features7, 2, 15);              // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("LA57", features7, 2, 16);
                TestFeature("RDPID", features7, 2, 22);
                TestFeature("KL", features7, 2, 23);
                TestFeature("CLDEMOTE", features7, 2, 25);
                TestFeature("MOVDIRI", features7, 2, 27);
                TestFeature("MOVDIR64B", features7, 2, 28);
                TestFeature("ENQCMD", features7, 2, 29);               // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("SGX_LC", features7, 2, 30);
                TestFeature("PKS", features7, 2, 31);
                ReservedFeature(features7, 2, 0x053E0000);

                TestFeature("AVX512_4VNNIW", features7, 3, 2);
                TestFeature("AVX512_4FMAPS", features7, 3, 3);
                TestFeature("FSRM", features7, 3, 4);
                TestFeature("UINTR", features7, 3, 5);
                TestFeature("AVX512_VP2INTERSECT", features7, 3, 8);
                TestFeature("SRBDS_CTRL", features7, 3, 9);            // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("MD_CLEAR", features7, 3, 10);
                TestFeature("TSX_FORCE_ABORT", features7, 3, 13);      // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("SERIALIZE", features7, 3, 14);
                TestFeature("Hybrid", features7, 3, 15);
                TestFeature("TSXLDTRK", features7, 3, 16);
                TestFeature("PCONFIG", features7, 3, 18);
                TestFeature("LBR", features7, 3, 19);                  // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("CET_IBT", features7, 3, 20);
                TestFeature("AMX_BF16", features7, 3, 22);
                TestFeature("AVX512_FP16", features7, 3, 23);
                TestFeature("AMX_TILE", features7, 3, 24);
                TestFeature("AMX_INT8", features7, 3, 25);
                TestFeature("IBRS_IBPB", features7, 3, 26);
                TestFeature("STIBP", features7, 3, 27);
                TestFeature("L1D_FLUSH", features7, 3, 28);
                TestFeature("IA32_ARCH_CAPABILITIES", features7, 3, 29);
                TestFeature("IA32_CORE_CAPABILITIES", features7, 3, 30);
                TestFeature("SSBD", features7, 3, 31);
                ReservedFeature(features7, 3, 0x002218C3);

                if (features7.Result[0] > 0) {
                    CpuIdRegister features7s1 = cpu.CpuRegisters.GetCpuId(ExtendedFeatureFunction, 1);
                    if (features7s1 != null) {
                        TestFeature("AVX_VNNI", features7s1, 0, 4);
                        TestFeature("AVX512_BF16", features7s1, 0, 5);
                        TestFeature("FZMOVSB", features7s1, 0, 10);
                        TestFeature("FSSTOSB", features7s1, 0, 11);
                        TestFeature("FSCMPSB", features7s1, 0, 12);
                        TestFeature("HRESET", features7s1, 0, 22);
                        TestFeature("LAM", features7s1, 0, 26);
                        ReservedFeature(features7s1, 0, unchecked((int)0xFBBFE3CF));

                        ReservedFeature(features7s1, 1, unchecked((int)0xFFFFFFFF));
                        ReservedFeature(features7s1, 2, unchecked((int)0xFFFFFFFF));
                        ReservedFeature(features7s1, 3, unchecked((int)0xFFFFFFFF));
                    }

                    for (int subfunction = 2; subfunction < features7.Result[0]; subfunction++) {
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
                TestFeature("ABM", extfeat, 2, 5);
                TestFeature("PREFETCHW", extfeat, 2, 8);
                ReservedFeature(extfeat, 2, unchecked((int)0xFFFFFEDE));

                TestFeature("SYSCALL", extfeat, 3, 11);
                TestFeature("XD", extfeat, 3, 20);
                TestFeature("1GB_PAGE", extfeat, 3, 26);
                TestFeature("RDTSCP", extfeat, 3, 27);
                TestFeature("LM", extfeat, 3, 29);
                ReservedFeature(extfeat, 3, unchecked((int)0xD3EFF7FF));
            }
        }

        /// <inheritdoc/>
        public override CpuVendor CpuVendor
        {
            get { return CpuVendor.GenuineIntel; }
        }

        private void GetCpuTopology(BasicCpu cpu)
        {
            if (!Features["HTT"] || !Features["APIC"]) {
                if (Features["x2APIC"] && cpu.FunctionCount >= ExtendedTopology) {
                    CpuIdRegister x2apic = cpu.CpuRegisters.GetCpuId(ExtendedTopology, 0);
                    Topology.ApicId = x2apic.Result[3];
                } else if (cpu.FunctionCount >= FeatureInformationFunction) {
                    CpuIdRegister apic = cpu.CpuRegisters.GetCpuId(FeatureInformationFunction, 0);
                    Topology.ApicId = (apic.Result[1] >> 24) & 0xFF;
                } else {
                    Topology.ApicId = -1;
                    return;
                }

                int mnlpbits = Log2Pof2(GetMaxNumberOfLogicalProcessors(cpu));
                Topology.CoreTopology.Add(new CpuTopo(0, CpuTopoType.Core, ~(-1 << mnlpbits)));
                Topology.CoreTopology.Add(new CpuTopo(Topology.ApicId, CpuTopoType.Package, -1 << mnlpbits));
                return;
            }

            if (Features["x2APIC"] && cpu.FunctionCount >= ExtendedTopology2) {
                CpuIdRegister x2apic = cpu.CpuRegisters.GetCpuId(ExtendedTopology, 0);
                Topology.ApicId = x2apic.Result[3];
                GetExtendedTopology(cpu, ExtendedTopology2);
            } else if (Features["x2APIC"] && cpu.FunctionCount >= ExtendedTopology) {
                CpuIdRegister x2apic = cpu.CpuRegisters.GetCpuId(ExtendedTopology, 0);
                Topology.ApicId = x2apic.Result[3];
                GetExtendedTopology(cpu, ExtendedTopology);
            } else if (cpu.FunctionCount >= LegacyTopology) {
                CpuIdRegister apic = cpu.CpuRegisters.GetCpuId(FeatureInformationFunction, 0);
                CpuIdRegister topo = cpu.CpuRegisters.GetCpuId(LegacyTopology, 0);
                Topology.ApicId = (apic.Result[1] >> 24) & 0xFF;
                GetLegacyTopology(apic, topo);
            } else {
                CpuIdRegister apic = cpu.CpuRegisters.GetCpuId(FeatureInformationFunction, 0);
                if (Features["APIC"]) {
                    Topology.ApicId = (apic.Result[1] >> 24) & 0xFF;
                } else {
                    Topology.ApicId = 0;
                }

                int mnlpbits = Log2Pof2(GetMaxNumberOfLogicalProcessors(cpu));
                Topology.CoreTopology.Add(new CpuTopo(0, CpuTopoType.Core, ~(-1 << mnlpbits)));
                Topology.CoreTopology.Add(new CpuTopo(Topology.ApicId, CpuTopoType.Package, -1 << mnlpbits));
            }
        }

        private int GetMaxNumberOfLogicalProcessors(BasicCpu cpu)
        {
            if (Features["HTT"]) {
                CpuIdRegister apic = cpu.CpuRegisters.GetCpuId(FeatureInformationFunction, 0);
                return (apic.Result[1] >> 16) & 0xFF;
            }
            return 1;
        }

        private void GetExtendedTopology(BasicCpu cpu, int leaf)
        {
            int lwidth = 0;
            int level = 0;

            CpuIdRegister topo = cpu.CpuRegisters.GetCpuId(leaf, level);
            while (topo != null) {
                int ltype = (topo.Result[2] >> 8) & 0xFF;
                if (ltype == 0) break;

                int cwidth = topo.Result[0] & 0xF;
                int width = cwidth - lwidth;
                int mask = ~(-1 << width);
                int id = (int)((Topology.ApicId >> lwidth) & mask);

                Topology.CoreTopology.Add(new CpuTopo(id, (CpuTopoType)ltype, mask));

                level++;
                lwidth = cwidth;
                topo = cpu.CpuRegisters.GetCpuId(leaf, level);
            }

            topo = cpu.CpuRegisters.GetCpuId(FeatureInformationFunction, 0);
            int pwidth = Log2Pof2((topo.Result[1] >> 16) & 0xFF);
            int pmask = -1 << pwidth;
            Topology.CoreTopology.Add(new CpuTopo(Topology.ApicId >> pwidth, CpuTopoType.Package, pmask));
        }

        private void GetLegacyTopology(CpuIdRegister apic, CpuIdRegister topo)
        {
            int maxApic = (apic.Result[1] >> 16) & 0xFF;
            int maxCore = ((topo.Result[0] >> 26) & 0x3F) + 1;

            int smtWidth = Log2Pof2(maxApic / maxCore);
            long smtMask = ~(-1 << smtWidth);
            long smtId = Topology.ApicId & smtMask;
            Topology.CoreTopology.Add(new CpuTopo(smtId, CpuTopoType.Smt, smtMask));

            int coreWidth = Log2Pof2(maxCore);
            long coreMask = ~(-1 << coreWidth);
            long coreId = (Topology.ApicId >> smtWidth) & coreMask;
            Topology.CoreTopology.Add(new CpuTopo(coreId, CpuTopoType.Core, coreMask));

            int pkgWidth = smtWidth + coreWidth;  // Should be the same as maxApic.
            long pkgId = Topology.ApicId >> pkgWidth;
            long pkgMask = -1 << pkgWidth;
            Topology.CoreTopology.Add(new CpuTopo(pkgId, CpuTopoType.Package, pkgMask));
        }

        private void GetCacheTopology(BasicCpu cpu)
        {
            if (cpu.FunctionCount >= LegacyCache) {
                bool leafTlb = false;
                bool leafCpu = false;
                bool noExtraCache = false;
                CpuIdRegister cache = cpu.CpuRegisters.GetCpuId(LegacyCache, 0);
                if (cache != null && (cache.Result[0] & 0xFF) == 0x01) {
                    GetLegacyCacheTopology(cache.Result[0] & unchecked((int)0xFFFFFF00), ref leafCpu, ref leafTlb, ref noExtraCache);
                    GetLegacyCacheTopology(cache.Result[1], ref leafCpu, ref leafTlb, ref noExtraCache);
                    GetLegacyCacheTopology(cache.Result[2], ref leafCpu, ref leafTlb, ref noExtraCache);
                    GetLegacyCacheTopology(cache.Result[3], ref leafCpu, ref leafTlb, ref noExtraCache);
                }
                FixLegacyCacheMask(noExtraCache);

                if (leafCpu && cpu.FunctionCount > LegacyTopology) GetCacheTopologyLeaf(LegacyTopology);
                if (leafTlb && cpu.FunctionCount > AddressTranslation) GetCacheTlbTopologyLeaf(AddressTranslation);
            }
        }

        private void GetLegacyCacheTopology(int register, ref bool leafCpu, ref bool leafTlb, ref bool noExtraCache)
        {
            if ((register & unchecked((int)0x80000000)) != 0) return;
            GetLegacyCacheTopologyEntry(register & 0xFF, ref leafCpu, ref leafTlb, ref noExtraCache);
            GetLegacyCacheTopologyEntry((register >> 8) & 0xFF, ref leafCpu, ref leafTlb, ref noExtraCache);
            GetLegacyCacheTopologyEntry((register >> 16) & 0xFF, ref leafCpu, ref leafTlb, ref noExtraCache);
            GetLegacyCacheTopologyEntry((register >> 24) & 0xFF, ref leafCpu, ref leafTlb, ref noExtraCache);
        }

        private static readonly Dictionary<int, List<CacheTopo>> CacheLookup = new Dictionary<int, List<CacheTopo>>() {
            [0x01] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb4k, 4, 32) },
            [0x02] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb4M, 0, 2) },
            [0x03] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k, 4, 64) },
            [0x04] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4M, 4, 8) },
            [0x05] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4M, 4, 32) },
            [0x06] = new List<CacheTopo>() { new CacheTopoCpu(1, CacheType.Instruction, 4, 32, 8) },
            [0x08] = new List<CacheTopo>() { new CacheTopoCpu(1, CacheType.Instruction, 4, 32, 16) },
            [0x09] = new List<CacheTopo>() { new CacheTopoCpu(1, CacheType.Instruction, 4, 64, 32) },
            [0x0A] = new List<CacheTopo>() { new CacheTopoCpu(1, CacheType.Data, 2, 32, 8) },
            [0x0B] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb4M, 4, 4) },
            [0x0C] = new List<CacheTopo>() { new CacheTopoCpu(1, CacheType.Data, 4, 32, 16) },
            [0x0D] = new List<CacheTopo>() { new CacheTopoCpu(1, CacheType.Data, 4, 64, 16) },
            [0x0E] = new List<CacheTopo>() { new CacheTopoCpu(1, CacheType.Data, 6, 64, 24) },
            [0x1D] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 2, 64, 128) },
            [0x21] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 8, 64, 256) },
            [0x22] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 4, 128, 256) },
            [0x23] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 8, 128, 512) },
            [0x24] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 16, 64, 1024) },
            [0x25] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 8, 128, 2048) },
            [0x29] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 8, 128, 4096) },
            [0x2C] = new List<CacheTopo>() { new CacheTopoCpu(1, CacheType.Data, 8, 64, 32) },
            [0x30] = new List<CacheTopo>() { new CacheTopoCpu(1, CacheType.Instruction, 8, 64, 32) },
            [0x40] = null,  // No 2nd-level cache or, if processor contains a valid 2nd-level cache, no 3rd level cache
            [0x41] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 4, 32, 128) },
            [0x42] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 4, 32, 256) },
            [0x43] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 4, 32, 512) },
            [0x44] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 4, 32, 1024) },
            [0x45] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 4, 32, 2048) },
            [0x46] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 4, 64, 4096) },
            [0x47] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 8, 64, 8192) },
            [0x48] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 12, 64, 3072) },
            [0x49] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 16, 64, 4096) },     // Note Xeon 0fH, 06H is Level 3
            [0x4A] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 12, 64, 6144) },
            [0x4B] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 16, 64, 8192) },
            [0x4C] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 12, 64, 12288) },
            [0x4D] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 16, 64, 16384) },
            [0x4E] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 24, 64, 6144) },
            [0x4F] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb4k, 0, 32) },
            [0x50] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb4k2M4M, 0, 64) },
            [0x51] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb4k2M4M, 0, 128) },
            [0x52] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb4k2M4M, 0, 256) },
            [0x55] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb2M4M, 0, 7) },
            [0x56] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4M, 4, 16) },
            [0x57] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k, 4, 16) },
            [0x59] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k, 0, 16) },
            [0x5A] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb2M4M, 4, 32) },
            [0x5B] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k4M, 0, 64) },
            [0x5C] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k4M, 0, 128) },
            [0x5D] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k4M, 0, 256) },
            [0x60] = new List<CacheTopo>() { new CacheTopoCpu(1, CacheType.Data, 8, 64, 16) },
            [0x61] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb4k, 0, 48) },
            [0x63] = new List<CacheTopo>() {
                new CacheTopoTlb(1, CacheType.DataTlb2M, 4, 32),
                new CacheTopoTlb(1, CacheType.DataTlb1G, 4, 4)
            },
            [0x64] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k, 4, 512) },
            [0x66] = new List<CacheTopo>() { new CacheTopoCpu(1, CacheType.Data, 4, 64, 8) },
            [0x67] = new List<CacheTopo>() { new CacheTopoCpu(1, CacheType.Data, 4, 64, 16) },
            [0x68] = new List<CacheTopo>() { new CacheTopoCpu(1, CacheType.Data, 4, 64, 32) },
            [0x6A] = new List<CacheTopo>() { new CacheTopoTlb(2, CacheType.UnifiedTlb4k, 8, 64) },
            [0x6B] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k, 8, 256) },
            [0x6C] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb2M4M, 8, 128) },
            [0x6D] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb1G, 0, 16) },
            [0x70] = new List<CacheTopo>() { new CacheTopoTrace(CacheType.Trace, 8, 12 * 1024) },
            [0x71] = new List<CacheTopo>() { new CacheTopoTrace(CacheType.Trace, 8, 16 * 1024) },
            [0x72] = new List<CacheTopo>() { new CacheTopoTrace(CacheType.Trace, 8, 32 * 1024) },
            [0x76] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb2M4M, 0, 8) },
            [0x78] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 4, 64, 1024) },
            [0x79] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 8, 128, 128) },
            [0x7A] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 8, 128, 256) },
            [0x7B] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 8, 128, 512) },
            [0x7C] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 8, 128, 1024) },
            [0x7D] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 8, 64, 2048) },
            [0x7F] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 2, 64, 512) },
            [0x80] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 8, 64, 512) },
            [0x82] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 8, 32, 256) },
            [0x83] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 8, 32, 512) },
            [0x84] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 8, 32, 1024) },
            [0x85] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 8, 32, 2048) },
            [0x86] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 4, 64, 512) },
            [0x87] = new List<CacheTopo>() { new CacheTopoCpu(2, CacheType.Unified, 8, 64, 1024) },
            [0xA0] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k, 0, 32) },
            [0xB0] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb4k, 4, 128) },
            [0xB1] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb2M, 4, 128) },
            [0xB2] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb4k, 4, 64) },
            [0xB3] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k, 4, 128) },
            [0xB4] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k, 4, 256) },
            [0xB5] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb4k, 8, 64) },
            [0xB6] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.InstructionTlb4k, 8, 128) },
            [0xBA] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k, 4, 64) },
            [0xC0] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k4M, 4, 8) },
            [0xC1] = new List<CacheTopo>() { new CacheTopoTlb(2, CacheType.UnifiedTlb4k2M, 8, 1024) },
            [0xC2] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb4k2M, 4, 16) },
            [0xC3] = new List<CacheTopo>() {
                new CacheTopoTlb(2, CacheType.UnifiedTlb4k2M, 6, 1536),
                new CacheTopoTlb(2, CacheType.unifiedTlb1G, 4, 16)
            },
            [0xC4] = new List<CacheTopo>() { new CacheTopoTlb(1, CacheType.DataTlb2M4M, 4, 32) },
            [0xCA] = new List<CacheTopo>() { new CacheTopoTlb(2, CacheType.UnifiedTlb4k, 4, 512) },
            [0xD0] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 4, 64, 512) },
            [0xD1] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 4, 64, 1024) },
            [0xD2] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 4, 64, 2048) },
            [0xD6] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 8, 64, 1024) },
            [0xD7] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 8, 64, 2048) },
            [0xD8] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 8, 64, 4096) },
            [0xDC] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 12, 64, 1536) },
            [0xDD] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 12, 64, 3072) },
            [0xDE] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 12, 64, 6144) },
            [0xE2] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 16, 64, 2048) },
            [0xE3] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 16, 64, 4096) },
            [0xE4] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 16, 64, 8192) },
            [0xEA] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 24, 64, 12288) },
            [0xEB] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 24, 64, 18432) },
            [0xEC] = new List<CacheTopo>() { new CacheTopoCpu(3, CacheType.Unified, 24, 64, 24576) },
            [0xF0] = new List<CacheTopo>() { new CacheTopoPrefetch(CacheType.Prefetch, 64) },
            [0xF1] = new List<CacheTopo>() { new CacheTopoPrefetch(CacheType.Prefetch, 128) },
            [0xFE] = null, // CPUID Leaf 2 doesn't report TLB. Use CPUID Leaf 18h
            [0xFF] = null, // CPUID Leaf 2 doesn't report Cache Descriptor. Use CPUID Leaf 04h
        };

        private void GetLegacyCacheTopologyEntry(int value, ref bool leafCpu, ref bool leafTlb, ref bool noExtraCache)
        {
            switch (value) {
            case 0x40:
                noExtraCache = true;
                return;
            case 0xFE:
                leafTlb = true;
                return;
            case 0xFF:
                leafCpu = true;
                return;
            }

            if (!CacheLookup.TryGetValue(value, out List<CacheTopo> entries)) return;
            if (entries == null) return;

            if (value == 0x49 && Family == 0xF && Model == 0x6) {
                // Special case for 0x49 for Xeon Processor MP, Family 0Fh, Model 06h
                entries = new List<CacheTopo>() {
                    new CacheTopoCpu(3, CacheType.Unified, 16, 64, 4096)
                };
            }

            foreach (CacheTopo entry in entries) {
                Topology.CacheTopology.Add(entry);
            }
        }

        private void FixLegacyCacheMask(bool noExtraCache)
        {
            long smtMask = 0;
            long coreMask = 0;
            long pkgMask = 0;

            foreach (CpuTopo core in Topology.CoreTopology) {
                switch (core.TopoType) {
                case CpuTopoType.Smt:
                    smtMask = core.Mask;
                    break;
                case CpuTopoType.Core:
                    coreMask = core.Mask;
                    break;
                case CpuTopoType.Package:
                    pkgMask = ~core.Mask;
                    break;
                }
            }

            int maxLevel = 0;
            foreach (CacheTopoCpu entry in Topology.CacheTopology.OfType<CacheTopoCpu>()) {
                if (maxLevel < entry.Level) maxLevel = entry.Level;
            }

            foreach (CacheTopoCpu entry in Topology.CacheTopology.OfType<CacheTopoCpu>()) {
                if (entry.Mask == -1) {
                    if (entry.Level == 1) {
                        entry.Mask = smtMask;
                    } else if (entry.Level == 2) {
                        if (noExtraCache) {
                            if (maxLevel == 2) {
                                // Level 2 is shared across cores
                                entry.Mask = coreMask;
                            } else {
                                // Level 2 is shared across threads
                                entry.Mask = smtMask;
                            }
                        } else {
                            entry.Mask = smtMask;
                        }
                    } else if (entry.Level >= 3) {
                        entry.Mask = pkgMask;
                    }
                }
            }
        }
    }
}
