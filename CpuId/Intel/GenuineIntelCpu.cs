namespace RJCP.Diagnostics.Intel
{
    using System.Text;

    /// <summary>
    /// Description of a GenuineIntel CPU.
    /// </summary>
    public class GenuineIntelCpu : GenericIntelCpu
    {
        private const int FeatureInformationFunction = 1;
        private const int ExtendedFeatureFunction = 7;
        private const int ExtendedInformationFunction = unchecked((int)0x80000001);
        private const int ProcessorBrand1Function = unchecked((int)0x80000002);
        private const int ProcessorBrand2Function = unchecked((int)0x80000003);
        private const int ProcessorBrand3Function = unchecked((int)0x80000004);

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
                Family = m_ExtendedModel + m_FamilyCode;
            }
            if (m_FamilyCode == 0x06 || m_FamilyCode == 0x0F) {
                Model = (m_ExtendedModel << 4) + m_ModelNumber;
            } else {
                Model = m_ModelNumber;
            }

            Stepping = m_SteppingId;
            ProcessorType = m_ProcessorType;
            Description = GetDescription(cpu);
            FindFeatures(cpu);
        }

        private void GetProcessorSignature(BasicCpu cpu)
        {
            if (cpu.FunctionCount == 0) return;

            CpuIdRegister feature = cpu.CpuRegisters.GetCpuId(FeatureInformationFunction, 0);
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

                ApicId = (ebx >> 24) & 0xFF;
                ApicMaxThreads = (ebx >> 16) & 0xFF;
            }
        }

        private string GetDescription(BasicCpu cpu)
        {
            // According to 241618-39, CPU ID Application Note 485, May 2012
            // 1. Check if bit 21 is settable in FLAGS (done, else we wouldn't be here).

            // 2. Check CPUID.80000000.EAX >= 800000004. If so, get branding information from 80000002-80000004.
            if (cpu.ExtendedFunctionCount >= 4) {
                StringBuilder brand = new StringBuilder(50);
                WriteDescription(brand, cpu.CpuRegisters.GetCpuId(ProcessorBrand1Function, 0));
                WriteDescription(brand, cpu.CpuRegisters.GetCpuId(ProcessorBrand2Function, 0));
                WriteDescription(brand, cpu.CpuRegisters.GetCpuId(ProcessorBrand3Function, 0));
                return brand.ToString().Trim();
            }

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
            return IntelLegacySignatures.GetType(m_ExtendedFamily, m_ExtendedModel, m_ProcessorType, m_FamilyCode, m_ModelNumber, m_SteppingId);
        }

        private void WriteDescription(StringBuilder brand, CpuIdRegister register)
        {
            int eax = register.Result[0];
            int ebx = register.Result[1];
            int ecx = register.Result[2];
            int edx = register.Result[3];

            Append(brand, eax & 0xFF);
            Append(brand, (eax >> 8) & 0xFF);
            Append(brand, (eax >> 16) & 0xFF);
            Append(brand, (eax >> 24) & 0xFF);
            Append(brand, ebx & 0xFF);
            Append(brand, (ebx >> 8) & 0xFF);
            Append(brand, (ebx >> 16) & 0xFF);
            Append(brand, (ebx >> 24) & 0xFF);
            Append(brand, ecx & 0xFF);
            Append(brand, (ecx >> 8) & 0xFF);
            Append(brand, (ecx >> 16) & 0xFF);
            Append(brand, (ecx >> 24) & 0xFF);
            Append(brand, edx & 0xFF);
            Append(brand, (edx >> 8) & 0xFF);
            Append(brand, (edx >> 16) & 0xFF);
            Append(brand, (edx >> 24) & 0xFF);
        }

        private void Append(StringBuilder brand, int value)
        {
            if (value == 0) return;
            brand.Append((char)value);
        }

        private void FindFeatures(BasicCpu cpu)
        {
            if (cpu.FunctionCount < FeatureInformationFunction) return;

            CpuIdRegister features = cpu.CpuRegisters.GetCpuId(FeatureInformationFunction, 0);
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
            TestFeature("PBE", features, 3, 31);
            if (Features["SEP"] && ProcessorSignature < 0x633)
                Features["SEP"] = false;

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

            FindExtendedFeatures(cpu);

            if (cpu.FunctionCount < ExtendedFeatureFunction) return;
            CpuIdRegister extfeat = cpu.CpuRegisters.GetCpuId(ExtendedFeatureFunction, 0);

            TestFeature("FSGSBASE", extfeat, 1, 0);
            TestFeature("IA32_TSC_ADJUST", extfeat, 1, 1);
            TestFeature("SGX", extfeat, 1, 2);
            TestFeature("BMI1", extfeat, 1, 3);
            TestFeature("HLE", extfeat, 1, 4);
            TestFeature("AVX2", extfeat, 1, 5);
            TestFeature("FDP_EXCPTN_ONLY", extfeat, 1, 6);
            TestFeature("SMEP", extfeat, 1, 7);
            TestFeature("BMI2", extfeat, 1, 8);
            TestFeature("REP MOVSB/STOSB", extfeat, 1, 9);
            TestFeature("INVPCID", extfeat, 1, 10);
            TestFeature("RTM", extfeat, 1, 11);
            TestFeature("RDT-M", extfeat, 1, 12);
            TestFeature("FPU-CS Dep", extfeat, 1, 13);
            TestFeature("MPX", extfeat, 1, 14);
            TestFeature("RDT-A", extfeat, 1, 15);
            TestFeature("AVX512F", extfeat, 1, 16);
            TestFeature("AVX512DQ", extfeat, 1, 17);
            TestFeature("RDSEED", extfeat, 1, 18);
            TestFeature("ADX", extfeat, 1, 19);
            TestFeature("SMAP", extfeat, 1, 20);
            TestFeature("AVX512_IFMA", extfeat, 1, 21);
            TestFeature("CLFLUSHOPT", extfeat, 1, 23);
            TestFeature("CLWB", extfeat, 1, 24);
            TestFeature("PROCTRC", extfeat, 1, 25);
            TestFeature("AVX512PF", extfeat, 1, 26);
            TestFeature("AVX512ER", extfeat, 1, 27);
            TestFeature("AVX512CD", extfeat, 1, 28);
            TestFeature("SHA", extfeat, 1, 29);
            TestFeature("AVX512BW", extfeat, 1, 30);
            TestFeature("AVX512VL", extfeat, 1, 31);

            TestFeature("PREFETCHWT1", extfeat, 2, 0);
            TestFeature("AVX512_VBMI", extfeat, 2, 1);
            TestFeature("UMIP", extfeat, 2, 2);
            TestFeature("PKU", extfeat, 2, 3);
            TestFeature("OSPKE", extfeat, 2, 4);
            TestFeature("WAITPKG", extfeat, 2, 5);
            TestFeature("AVX512_VBMI2", extfeat, 2, 6);
            TestFeature("CET_SS", extfeat, 2, 7);
            TestFeature("GFNI", extfeat, 2, 8);
            TestFeature("VAES", extfeat, 2, 9);
            TestFeature("VPCLMULQDQ", extfeat, 2, 10);
            TestFeature("AVX512_VNNI", extfeat, 2, 11);
            TestFeature("AVX512_BITALG", extfeat, 2, 12);
            TestFeature("AVX512_VPOPCNTDQ", extfeat, 2, 14);
            TestFeature("RDPID", extfeat, 2, 22);
            TestFeature("CLDEMOTE", extfeat, 2, 25);
            TestFeature("MOVDIRI", extfeat, 2, 27);
            TestFeature("MOVDIR64B", extfeat, 2, 28);
            TestFeature("SGX_LC", extfeat, 2, 30);
            TestFeature("PKS", extfeat, 2, 31);

            TestFeature("AVX512_4VNNIW", extfeat, 3, 2);
            TestFeature("AVX512_4FMAPS", extfeat, 3, 3);
            TestFeature("Fast REP MOV", extfeat, 3, 4);
            TestFeature("MD_CLEAR", extfeat, 3, 10);
            TestFeature("Hybrid", extfeat, 3, 15);
            TestFeature("CET_IBT", extfeat, 3, 20);
            TestFeature("IBRS/IBPB", extfeat, 3, 26);
            TestFeature("STIBP", extfeat, 3, 27);
            TestFeature("L1D_FLUSH", extfeat, 3, 28);
            TestFeature("IA32_ARCH_CAPABILITIES", extfeat, 3, 29);
            TestFeature("IA32_CORE_CAPABILITIES", extfeat, 3, 30);
            TestFeature("SSBD", extfeat, 3, 31);
        }

        private void FindExtendedFeatures(BasicCpu cpu)
        {
            if (cpu.FunctionCount < 1) return;
            CpuIdRegister features = cpu.CpuRegisters.GetCpuId(ExtendedInformationFunction, 0);

            TestFeature("LAHF", features, 2, 0);
            TestFeature("LZCNT", features, 2, 5);
            TestFeature("PREFETCHW", features, 2, 8);

            TestFeature("SYSCALL", features, 3, 11);
            TestFeature("XD", features, 3, 20);
            TestFeature("1GB_PAGE", features, 3, 26);
            TestFeature("RDTSCP", features, 3, 27);
            TestFeature("IA64", features, 3, 29);
        }

        private void TestFeature(string feature, CpuIdRegister register, int result, int bit)
        {
            TestFeature(feature, register, result, bit, false);
        }

        private void TestFeature(string feature, CpuIdRegister register, int result, int bit, bool invert)
        {
            bool value = (register.Result[result] & (1 << bit)) != 0;
            if (invert) value = !value;
            Features[feature] = value;
        }

        /// <inheritdoc/>
        public override CpuVendor CpuVendor
        {
            get { return CpuVendor.GenuineIntel; }
        }
    }
}
