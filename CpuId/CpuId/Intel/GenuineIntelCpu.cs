namespace RJCP.Diagnostics.CpuId.Intel
{
    /// <summary>
    /// Description of a GenuineIntel CPU.
    /// </summary>
    public class GenuineIntelCpu : GenericIntelCpuBase
    {
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
                TestFeature("RDT-M", features7, 1, 12);
                TestFeature("FPU-CS Dep", features7, 1, 13);
                TestFeature("MPX", features7, 1, 14);
                TestFeature("RDT-A", features7, 1, 15);
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
                TestFeature("AVX512_VPOPCNTDQ", features7, 2, 14);
                TestFeature("5L_PAGE", features7, 2, 15);              // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("RDPID", features7, 2, 22);
                TestFeature("CLDEMOTE", features7, 2, 25);
                TestFeature("MOVDIRI", features7, 2, 27);
                TestFeature("MOVDIR64B", features7, 2, 28);
                TestFeature("ENQCMD", features7, 2, 29);               // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("SGX_LC", features7, 2, 30);
                TestFeature("PKS", features7, 2, 31);
                ReservedFeature(features7, 2, 0x05BF2000);

                TestFeature("AVX512_4VNNIW", features7, 3, 2);
                TestFeature("AVX512_4FMAPS", features7, 3, 3);
                TestFeature("FSRM", features7, 3, 4);
                TestFeature("AVX512_VP2INTERSECT", features7, 3, 8);   // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("SRBDS_CTRL", features7, 3, 9);            // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("MD_CLEAR", features7, 3, 10);
                TestFeature("TSX_FORCE_ABORT", features7, 3, 13);      // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("SERIALIZE", features7, 3, 14);            // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("Hybrid", features7, 3, 15);
                TestFeature("TSXLDTRK", features7, 3, 16);             // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("PCONFIG", features7, 3, 18);              // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("LBR", features7, 3, 19);                  // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("CET_IBT", features7, 3, 20);
                TestFeature("AMX_BF16", features7, 3, 22);             // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("AMX_TILE", features7, 3, 24);             // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("AMX_INT8", features7, 3, 25);             // Wikipedia https://en.wikipedia.org/wiki/CPUID
                TestFeature("IBRS_IBPB", features7, 3, 26);
                TestFeature("STIBP", features7, 3, 27);
                TestFeature("L1D_FLUSH", features7, 3, 28);
                TestFeature("IA32_ARCH_CAPABILITIES", features7, 3, 29);
                TestFeature("IA32_CORE_CAPABILITIES", features7, 3, 30);
                TestFeature("SSBD", features7, 3, 31);
                ReservedFeature(features7, 3, 0x00A218E3);

                if (features7.Result[0] > 0) {
                    CpuIdRegister features7s1 = cpu.CpuRegisters.GetCpuId(ExtendedFeatureFunction, 1);
                    if (features7s1 != null) {
                        TestFeature("AVX_BF16", features7s1, 0, 5);         // Wikipedia https://en.wikipedia.org/wiki/CPUID
                        ReservedFeature(features7s1, 0, unchecked((int)0xFFFFFFDF));

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
    }
}
