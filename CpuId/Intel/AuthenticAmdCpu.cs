namespace RJCP.Diagnostics.Intel
{
    /// <summary>
    /// Description of an AuthenticAMD CPU.
    /// </summary>
    public class AuthenticAmdCpu : GenericIntelCpuBase
    {
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
                TestFeature("APIC", features, 3, 9);
                TestFeature("SEP", features, 3, 11);
                TestFeature("MTRR", features, 3, 12);
                TestFeature("PGE", features, 3, 13);
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
                TestFeature("MOVBE", features, 2, 22);
                TestFeature("POPCNT", features, 2, 23);
                TestFeature("AESNI", features, 2, 25);
                TestFeature("XSAVE", features, 2, 26);
                TestFeature("OSXSAVE", features, 2, 27);
                TestFeature("AVX", features, 2, 28);
                TestFeature("F16C", features, 2, 29);
                TestFeature("RDRAND", features, 2, 30);
                TestFeature("HYPERVISOR", features, 2, 31);
                ReservedFeature(features, 2, 0x0125CDF4);
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
                TestFeature("RDSEED", features7, 1, 18);
                TestFeature("ADX", features7, 1, 19);
                TestFeature("SMAP", features7, 1, 20);
                // bit 22 is not RDPID as given in Document #24594 r3.31 p606
                TestFeature("CLFLUSHOPT", features7, 1, 23);
                TestFeature("CLWB", features7, 1, 24);
                TestFeature("SHA", features7, 1, 29);
                ReservedFeature(features7, 1, unchecked((int)0xDE63FA56));

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
                TestFeature("CMP", extfeat, 2, 1);
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
                TestFeature("TCE", extfeat, 2, 17);                  // AMD Doc #42300_15h_Mod_10h-1Fh_BKDF.pdf
                TestFeature("NODEID", extfeat, 2, 19);               // AMD Doc #42300_15h_Mod_10h-1Fh_BKDF.pdf
                TestFeature("TBM", extfeat, 2, 21);
                TestFeature("TOPX", extfeat, 2, 22);
                TestFeature("PerfCtrExtCore", extfeat, 2, 23);
                TestFeature("PerfCtrExtNB", extfeat, 2, 24);
                TestFeature("DBE", extfeat, 2, 26);
                TestFeature("PerfTSC", extfeat, 2, 27);
                TestFeature("PerfL2I", extfeat, 2, 28);              // Sandpile.org
                TestFeature("MONITORX", extfeat, 2, 29);
                ReservedFeature(extfeat, 2, unchecked((int)0xC2144000));

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

            if (cpu.ExtendedFunctionCount < 8) return;
            CpuIdRegister extfeat8 = cpu.CpuRegisters.GetCpuId(ExtendedLmApicId, 0);
            if (extfeat8 != null) {
                TestFeature("CLZERO", extfeat8, 1, 0);
                TestFeature("IRPERF", extfeat8, 1, 1);            // Instruction Retired Counter
                TestFeature("ASRFPEP", extfeat8, 1, 2);           // Error Pointer Zero/Restore
                TestFeature("INVLPGB", extfeat8, 1, 3);
                TestFeature("RDPRU", extfeat8, 1, 4);
                TestFeature("MCOMMIT", extfeat8, 1, 8);
                TestFeature("WBNOINVD", extfeat8, 1, 9);
                TestFeature("IBPB", extfeat8, 1, 12);             // Sandpile.org
                TestFeature("INT_WBINVD", extfeat8, 1, 13);
                TestFeature("IBRS", extfeat8, 1, 14);             // Sandpile.org
                TestFeature("STIBP", extfeat8, 1, 15);            // Sandpile.org
                TestFeature("IBRS_ALL", extfeat8, 1, 16);         // Sandpile.org
                TestFeature("STIBP_ALL", extfeat8, 1, 17);        // Sandpile.org
                TestFeature("IBRS_PREF", extfeat8, 1, 18);        // Sandpile.org
                TestFeature("EFER.LMSLE", extfeat8, 1, 20);
                TestFeature("INVLPGB_NESTED", extfeat8, 1, 21);
                ReservedFeature(extfeat8, 1, unchecked((int)0xFFC80CE0));
            }

            if (cpu.ExtendedFunctionCount < 31) return;
            CpuIdRegister extfeat1f = cpu.CpuRegisters.GetCpuId(ExtendedEncMem, 0);
            if (extfeat1f != null) {
                TestFeature("SME", extfeat1f, 0, 0);
                TestFeature("SEV", extfeat1f, 0, 1);
                TestFeature("PageFlushMsr", extfeat1f, 0, 2);
                TestFeature("ES", extfeat1f, 0, 3);
                TestFeature("SNP", extfeat1f, 0, 4);
                TestFeature("VMPL", extfeat1f, 0, 5);
                ReservedFeature(extfeat1f, 0, unchecked((int)0xFFFFFFC0));
            }
        }

        /// <inheritdoc/>
        public override CpuVendor CpuVendor
        {
            get { return CpuVendor.AuthenticAmd; }
        }
    }
}
