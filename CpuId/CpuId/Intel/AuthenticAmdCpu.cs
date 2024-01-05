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
        internal const int PowerFeature = unchecked((int)0x80000007);
        internal const int SvmFeature = unchecked((int)0x8000000A);
        internal const int CacheTlb1G = unchecked((int)0x80000019);
        internal const int PerfOptIdent = unchecked((int)0x8000001A);
        internal const int InstrSampling = unchecked((int)0x8000001B);
        internal const int LwpCaps = unchecked((int)0x8000001C);
        internal const int CacheTopo = unchecked((int)0x8000001D);
        internal const int ProcessorTopo = unchecked((int)0x8000001E);
        internal const int ExtendedEncMem = unchecked((int)0x8000001F);
        internal const int PqosExtended = unchecked((int)0x80000020);
        internal const int Extended2 = unchecked((int)0x80000021);
        internal const int PerfMonDebug = unchecked((int)0x80000022);
        internal const int MultiKeyEncMem = unchecked((int)0x80000023);

        private int m_ProcessorSignature;
        private int m_ExtendedFamily;
        private int m_ExtendedModel;
        private int m_ProcessorType;
        private int m_FamilyCode;
        private int m_ModelNumber;
        private int m_SteppingId;

        internal AuthenticAmdCpu(BasicCpu cpu) : base(cpu)
        {
            Features.DescriptionPrefix = "AMD";
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
            FeatureLevel = IdentifyFeatureLevel();
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
            null, "PCID", null, "SSE41", "SSE42", "x2APIC", "MOVBE", "POPCNT",
            null, "AES", "XSAVE", "OSXSAVE", "AVX", "F16C", "RDRAND", "HYPERVISOR"
        };

        private readonly static string[] FeaturesCpuId01Edx = new string[] {
            "FPU", "VME", "DE", "PSE", "TSC", "MSR", "PAE", "MCE",
            // PGE and APIC are set depending on CPU model
            "CMPXCHG8B", null, null, "SysEnterSysExit", "MTRR", null, "MCA", "CMOV",
            "PAT", "PSE36", null, "CLFSH", null, null, null, "MMX",
            "FXSR", "SSE", "SSE2", null, "HTT", null, null, null
        };

        private readonly static string[] FeaturesCpuId07Ebx = new string[] {
            "FSGSBASE", null, null, "BMI1", null, "AVX2", null, "SMEP",
            "BMI2", null, "INVPCID", null, "PQM", null, null, "PQE",
            // [6] has an error in the table CPUID Fn0000_0007_EBX_x0, CPUID EAX=07h, ECX=0, EBX[22] is not RDPID.
            // The instruction RPID, and table D-1, correctly specifies this register, itself specifies ECX[22].
            null, null, "RDSEED", "ADX", "SMAP", null, null, "CLFLUSHOPT",
            "CLWB", null, null, null, null, "SHA", null, null
        };

        private readonly static string[] FeaturesCpuId07Ecx = new string[] {
            null, null, "UMIP", "PKU", "OSPKE", null, null, "CET_SS",
            null, "VAES", "VPCMULQDQ", null, null, null, null, null,
            // [6] has an error in the table CPUID Fn0000_0007_EBX_x0, CPUID EAX=07h, ECX=0, EBX[22] is not RDPID.
            // The instruction RPID, and table D-1, correctly specifies this register, itself specifies ECX[22].
            "LA57", null, null, null, null, null, "RDPID", null,
            "BUSLOCKTRAP", null, null, null, null, null, null, null
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
            "LahfSahf", "CmpLegacy", "SVM", "ExtApicSpace", "AltMovCr8", "ABM", "SSE4A", "MisAlignSSE",
            "3DNowPrefetch", "OSVW", "IBS", "XOP", "SKINIT", "WDT", null, "LWP",
            "FMA4", "TCE", null, "NODEID" /* [9] */, null, "TBM", "TopologyExtensions", "PerfCtrExtCore",
            // StreamPerfMon = [6a], now reserved
            "PerfCtrExtNB", "StreamPerfMon", "DataBkptExt", "PerfTSC", "PerfCtrExtLLC", "MONITORX", "AddrMaskExt", null
        };

        private readonly static string[] FeaturesCpuId80000001Edx = new string[] {
            "FPU", "VME", "DE", "PSE", "TSC", "MSR", "PAE", "MCE",
            "CMPXCHG8B", "APIC", null, "SysCallSysRet", "MTRR", "PGE", "MCA", "CMOV",
            "PAT", "PSE36", null, "MP", "NX", null, "MmxExt", "MMX",
            "FXSR", "FFXSR", "Page1GB", "RDTSCP", null, "LM", "3DNowExt", "3DNow"
        };

        private readonly static string[] FeaturesCpuId80000007Ebx = new string[] {
            "McaOverflowRecov", "SUCCOR", "HWA", "ScalableMca", null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null
        };

        private readonly static string[] FeaturesCpuId80000007Edx = new string[] {
            "TS", "FID", "VID", "TTP", "TM", null, "100MHzSteps", "HwPstate",
            "TscInvariant", "CPB", "EffFreqRO", "ProcFeedbackInterface", "ProcPowerReporting", null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null
        };

        private readonly static string[] FeaturesCpuId80000008Ebx = new string[] {
            "CLZERO", "InstRetCntMsr", "RstrFpErrPtrs", "INVLPGB", "RDPRU", null, "BE", null,
            "MCOMMIT", "WBNOINVD", null, null, "IBPB", "INT_WBINVD", "IBRS", "STIBP",
            // PPIN = [10]
            "IbrsAlwaysOn", "StibpAlwaysOn", "IbrsPreferred", "IbrsSameMode", "EferLmsleUnsupported", "INVLPGBnestedPages", null, "PPIN",
            "SSBD", "SsbdVirtSpecCtrl", "SsbdNotRequired", "CPPC", "PSFD", "BTC_NO", "IBPB_RET", null
        };

        private readonly static string[] FeaturesCpuId8000000AEdx = new string[] {
            "NP", "LbrVirt", "SVML", "NRIPS", "TscRateMsr", "VmcbClean", "FlushByAsid", "DecodeAssists",
            null, null, "PauseFilter", null, "PauseFilterThreshold", "AVIC", null, "VMSAVEvirt",
            "VGIF", "GMET", "x2AVIC", "SSSCheck", "SpecCtrl", "ROGPT", null, "HOST_MCE_OVERRIDE",
            "TlbiCtl", "VNMI", "IbsVirt", "ExtLvtAvicAccessChg", "NestedVirtVmcbAddrChk", "BusLockThreshold", null, null
        };

        private readonly static string[] FeaturesCpuId8000001AEax = new string[] {
            "FP128", "MOVU", "FP256", null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null
        };

        private readonly static string[] FeaturesCpuId8000001BEax = new string[] {
            "IBSFFV", "FetchSam", "OpSam", "RdWrOpCnt", "OpCnt", "BrnTrgt", "OpCntExt", "RipInvalidChk",
            "OpBrnFuse", null, null, "IbsL3MissFiltering", null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null
        };

        private readonly static string[] FeaturesCpuId8000001CEax = new string[] {
            "LwpAvail", "LwpVAL", "LwpIRE", "LwpBRE", "LwpDME", "LwpCNH", "LwpRNH", null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, "LwpCont", "LwpPTSC", "LwpInt"
        };

        private readonly static string[] FeaturesCpuId8000001CEcx = new string[] {
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, "LwpBranchPrediction", "LwpIpFiltering", "LwpCacheLevels", "LwpCacheLatency"
        };

        private readonly static string[] FeaturesCpuId8000001CEdx = new string[] {
            "LwpAvail", "LwpVAL", "LwpIRE", "LwpBRE", "LwpDME", "LwpCNH", "LwpRNH", null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, "LwpCont", "LwpPTSC", "LwpInt"
        };

        private readonly static string[] FeaturesCpuId8000001FEax = new string[] {
            "SME", "SEV", "PageFlushMsr", "SEV-ES", "SEV-SNP", "VMPL", "RMPQUERY", "VmplSSS",
            "SecureTsc", "TscAuxVirtualization", "HwEnvCacheCoh", "64BitHost", "RestrictedInjection", "AlternateInjection", "DebugSwap", "PreventHostIbs",
            "VTE", "VmgexitParameter", "VirtualTomMsr", "IbsVirtGuestCtl", null, null, null, null,
            "VmsaRegProt", "SmtProtection", null, null, "SvsmCommPageMSR", "NestedVirtSnpMsr", null, null
        };

        private readonly static string[] FeaturesCpuId80000020Ebx = new string[] {
            null, "L3MBE", "L3SMBE", "BMEC", "L3RR", "ABMC", "SDCIAE", null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null
        };

        private readonly static string[] FeaturesCpuId80000020s3Ecx = new string[] {
            "L3CacheLclBwFillMon", "L3CacheRmtBwFillMon", "L3CacheLclBwNtWrMon", "L3CacheRmtBwNtWrMon", "L3CacheLclSlowBwFIllMon", "L3CacheRmtSlowBwFIllMon", "L3CacheVicMon", null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null
        };

        private readonly static string[] FeaturesCpuId80000021Eax = new string[] {
            "NoNestedDataBp", null, "LFenceAlwaysSerializing", "SmmPgCfgLock", null, null, "NullSelectClearsBase", "UpperAddressIgnore",
            "AutomaticIBRS", "NoSmmCtlMSR", null, null, null, "PrefetchCtlMsr", null, null,
            null, "CpuidUserDis", null, null, null, null, null, null,
            null, null, null, null, null, null, null, null
        };

        private readonly static string[] FeaturesCpuId80000022Eax = new string[] {
            "PerfMonV2", "LbrStack", "LbrAndPmcFreeze", null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null
        };

        private readonly static string[] FeaturesCpuId80000023Eax = new string[] {
            "MemHmk", null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null
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

                // Alias AMD features to well known Intel features. Use the description from the Intel feature name.
                Features.Add("SSE4_1", Features["SSE41"]);
                Features.Add("SSE4_2", Features["SSE42"]);
                Features.Add("AESNI", Features["AES"]);
                Features.Add("CX8", Features["CMPXCHG8B"]);
                Features.Add("SEP", Features["SysEnterSysExit"]);
                Features.Add("PSE-36", Features["PSE36"]);
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

                // Alias AMD features to well known Intel features. Use the description from the Intel feature name.
                Features.Add("VPCLMULQDQ", Features["VPCMULQDQ"]);
                Features.Add("BUS_LOCK_DETECT", Features["BUSLOCKTRAP"]);
            }

            if (cpu.FunctionCount < ExtendedProcessorState) return;
            CpuIdRegister features13 = cpu.CpuRegisters.GetCpuId(ExtendedProcessorState, 1);
            if (features13 != null) {
                TestFeatures(FeaturesCpuId0D01Eax, FeatureGroup.ExtendedState, features13, 0);
                ReservedFeature(FeatureGroup.ExtendedState, features13, 0, unchecked((int)0xFFFFFFF0));

                TestFeatures(FeaturesCpuId0D01Ecx, FeatureGroup.ExtendedState, features13, 2);
                ReservedFeature(FeatureGroup.ExtendedState, features13, 2, unchecked((int)0xFFFFE7FF));

                ReservedFeature(FeatureGroup.ExtendedState, features13, 3, unchecked((int)0xFFFFFFFF));
            }

            FindAmdFeatures(cpu);
        }

        private void FindExtendedFeatures(BasicCpu cpu)
        {
            if (cpu.ExtendedFunctionCount < ExtendedInformationFunction - MaxExtendedFunction) return;
            CpuIdRegister extfeat = cpu.CpuRegisters.GetCpuId(ExtendedInformationFunction, 0);
            if (extfeat != null) {
                TestFeatures(FeaturesCpuId80000001Ecx, FeatureGroup.ExtendedFeatures, extfeat, 2);
                ReservedFeature(FeatureGroup.ExtendedFeatures, extfeat, 2, unchecked((int)0x80144000));

                TestFeatures(FeaturesCpuId80000001Edx, FeatureGroup.ExtendedFeatures, extfeat, 3);
                // 3DNowPrefetch ECX[8] || EDX[29] || EDX[31]
                Features["3DNowPrefetch"].Value |= Features["LM"].Value || Features["3DNow"].Value;
                ReservedFeature(FeatureGroup.ExtendedFeatures, extfeat, 3, 0x10240400);

                Features.Add("AHF64", Features["LahfSahf"]);
                Features.Add("LZCNT", Features["ABM"]);
                Features.Add("PREFETCHW", Features["3DNowPrefetch"]);
                Features.Add("SYSCALL", Features["SysCallSysRet"]);
                Features.Add("XD", Features["NX"]);
                Features.Add("1GB_PAGE", Features["Page1GB"]);
            }

            if (cpu.ExtendedFunctionCount < ExtendedFeatureIds - MaxExtendedFunction) return;
            CpuIdRegister extfeat8 = cpu.CpuRegisters.GetCpuId(ExtendedFeatureIds, 0);
            if (extfeat8 != null) {
                TestFeatures(FeaturesCpuId80000008Ebx, FeatureGroup.ExtendedFeaturesIdentifiers, extfeat8, 1);
                ReservedFeature(FeatureGroup.ExtendedFeaturesIdentifiers, extfeat8, 1, unchecked((int)0x80400CA0));
            }
        }

        private void FindAmdFeatures(BasicCpu cpu)
        {
            if (cpu.ExtendedFunctionCount < PowerFeature - MaxExtendedFunction) return;
            CpuIdRegister extfeat7 = cpu.CpuRegisters.GetCpuId(PowerFeature, 0);
            if (extfeat7 != null) {
                TestFeatures(FeaturesCpuId80000007Ebx, FeatureGroup.PowerManagement, extfeat7, 1);
                ReservedFeature(FeatureGroup.PowerManagement, extfeat7, 1, unchecked((int)0xFFFFFFF0));

                TestFeatures(FeaturesCpuId80000007Edx, FeatureGroup.PowerManagement, extfeat7, 3);
                ReservedFeature(FeatureGroup.PowerManagement, extfeat7, 3, unchecked((int)0xFFFFE020));
            }

            if (cpu.ExtendedFunctionCount < SvmFeature - MaxExtendedFunction) return;
            CpuIdRegister extfeata = cpu.CpuRegisters.GetCpuId(SvmFeature, 0);
            if (extfeata != null) {
                TestFeatures(FeaturesCpuId8000000AEdx, FeatureGroup.SvmFeatures, extfeata, 3);
                ReservedFeature(FeatureGroup.SvmFeatures, extfeata, 3, unchecked((int)0xC0404B00));
            }

            if (cpu.ExtendedFunctionCount < PerfOptIdent - MaxExtendedFunction) return;
            CpuIdRegister extfeat1a = cpu.CpuRegisters.GetCpuId(PerfOptIdent, 0);
            if (extfeat1a != null) {
                TestFeatures(FeaturesCpuId8000001AEax, FeatureGroup.PerformanceOptimizations, extfeat1a, 0);
                ReservedFeature(FeatureGroup.PerformanceOptimizations, extfeat1a, 0, unchecked((int)0xFFFFFFF8));
            }

            if (cpu.ExtendedFunctionCount < InstrSampling - MaxExtendedFunction) return;
            CpuIdRegister extfeat1b = cpu.CpuRegisters.GetCpuId(InstrSampling, 0);
            if (extfeat1b != null) {
                TestFeatures(FeaturesCpuId8000001BEax, FeatureGroup.PerformanceSampling, extfeat1b, 0);
                ReservedFeature(FeatureGroup.PerformanceSampling, extfeat1b, 0, unchecked((int)0xFFFFF600));
            }

            if (cpu.ExtendedFunctionCount < LwpCaps - MaxExtendedFunction) return;
            CpuIdRegister extfeat1c = cpu.CpuRegisters.GetCpuId(LwpCaps, 0);
            if (extfeat1c != null) {
                TestFeatures(FeaturesCpuId8000001CEax, FeatureGroup.LightweightProfiling, extfeat1c, 0);
                ReservedFeature(FeatureGroup.LightweightProfiling, extfeat1c, 0, 0x1FFFFF80);

                TestFeatures(FeaturesCpuId8000001CEcx, FeatureGroup.LightweightProfiling, extfeat1c, 2);
                ReservedFeature(FeatureGroup.LightweightProfiling, extfeat1c, 2, 0x0F000000);

                TestFeatures(FeaturesCpuId8000001CEdx, FeatureGroup.LightweightProfiling, extfeat1c, 3);
                ReservedFeature(FeatureGroup.LightweightProfiling, extfeat1c, 3, 0x1FFFFF80);
            }

            if (cpu.ExtendedFunctionCount < ExtendedEncMem - MaxExtendedFunction) return;
            CpuIdRegister extfeat1f = cpu.CpuRegisters.GetCpuId(ExtendedEncMem, 0);
            if (extfeat1f != null) {
                TestFeatures(FeaturesCpuId8000001FEax, FeatureGroup.EncryptedMemory, extfeat1f, 0);
                ReservedFeature(FeatureGroup.EncryptedMemory, extfeat1f, 0, unchecked((int)0xCCF00000));
            }

            if (cpu.ExtendedFunctionCount < PqosExtended - MaxExtendedFunction) return;
            CpuIdRegister extfeat20 = cpu.CpuRegisters.GetCpuId(PqosExtended, 0);
            if (extfeat20 != null) {
                ReservedFeature(FeatureGroup.PqosExtended, extfeat20, 0, unchecked((int)0xFFFFFFFF));

                TestFeatures(FeaturesCpuId80000020Ebx, FeatureGroup.PqosExtended, extfeat20, 1);
                ReservedFeature(FeatureGroup.PqosExtended, extfeat20, 1, unchecked((int)0xFFFFFF81));

                ReservedFeature(FeatureGroup.PqosExtended, extfeat20, 2, unchecked((int)0xFFFFFFFF));

                ReservedFeature(FeatureGroup.PqosExtended, extfeat20, 3, unchecked((int)0xFFFFFFFF));
            }
            CpuIdRegister extfeat20s3 = cpu.CpuRegisters.GetCpuId(PqosExtended, 3);
            if (extfeat20s3 != null) {
                if (Features["BMEC"].Value) {
                    TestFeatures(FeaturesCpuId80000020s3Ecx, FeatureGroup.PqosExtended, extfeat20s3, 2);
                    ReservedFeature(FeatureGroup.PqosExtended, extfeat20s3, 2, unchecked((int)0xFFFFFF80));
                } else {
                    ReservedFeature(FeatureGroup.PqosExtended, extfeat20s3, 2, unchecked((int)0xFFFFFFFF));
                }
                ReservedFeature(FeatureGroup.PqosExtended, extfeat20s3, 3, unchecked((int)0xFFFFFFFF));
            }

            if (cpu.ExtendedFunctionCount < Extended2 - MaxExtendedFunction) return;
            CpuIdRegister extfeat21 = cpu.CpuRegisters.GetCpuId(Extended2, 0);
            if (extfeat21 != null) {
                TestFeatures(FeaturesCpuId80000021Eax, FeatureGroup.ExtendedFeatures, extfeat21, 0);
                ReservedFeature(FeatureGroup.ExtendedFeatures, extfeat21, 0, unchecked((int)0xFFFDDC32));

                ReservedFeature(FeatureGroup.ExtendedFeatures, extfeat21, 2, unchecked((int)0xFFFFFFFF));

                ReservedFeature(FeatureGroup.ExtendedFeatures, extfeat21, 3, unchecked((int)0xFFFFFFFF));
            }

            if (cpu.ExtendedFunctionCount < PerfMonDebug - MaxExtendedFunction) return;
            CpuIdRegister extfeat22 = cpu.CpuRegisters.GetCpuId(PerfMonDebug, 0);
            if (extfeat22 != null) {
                TestFeatures(FeaturesCpuId80000022Eax, FeatureGroup.PerfMonDebug, extfeat22, 0);
                ReservedFeature(FeatureGroup.PerfMonDebug, extfeat22, 0, unchecked((int)0xFFFFFFF8));

                ReservedFeature(FeatureGroup.PerfMonDebug, extfeat22, 2, unchecked((int)0xFFFFFFFF));

                ReservedFeature(FeatureGroup.PerfMonDebug, extfeat22, 3, unchecked((int)0xFFFFFFFF));
            }

            if (cpu.ExtendedFunctionCount < MultiKeyEncMem - MaxExtendedFunction) return;
            CpuIdRegister extfeat23 = cpu.CpuRegisters.GetCpuId(MultiKeyEncMem, 0);
            if (extfeat23 != null) {
                TestFeatures(FeaturesCpuId80000023Eax, FeatureGroup.EncryptedMemory, extfeat22, 0);
                ReservedFeature(FeatureGroup.EncryptedMemory, extfeat23, 0, unchecked((int)0xFFFFFFFE));

                ReservedFeature(FeatureGroup.EncryptedMemory, extfeat23, 2, unchecked((int)0xFFFFFFFF));

                ReservedFeature(FeatureGroup.EncryptedMemory, extfeat23, 3, unchecked((int)0xFFFFFFFF));
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
            if (!Features["HTT"].Value || !Features["CmpLegacy"].Value || cpu.ExtendedFunctionCount < ExtendedFeatureIds - MaxExtendedFunction) {
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

            if (!Features["TopologyExtensions"].Value || cpu.ExtendedFunctionCount < ProcessorTopo - MaxExtendedFunction) {
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
            if (Features["TopologyExtensions"].Value && cpu.ExtendedFunctionCount >= CacheTopo - MaxExtendedFunction) {
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
