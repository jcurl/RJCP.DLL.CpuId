namespace RJCP.Diagnostics.Intel
{
    using System.Text;

    /// <summary>
    /// Description of a GenuineIntel CPU.
    /// </summary>
    public class GenuineIntelCpu : GenericIntelCpu
    {
        private const int FeatureInformationFunction = 1;
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
            CpuIdRegister feature = cpu.CpuRegisters.GetCpuId(FeatureInformationFunction, 0);
            if ((feature.Result[1] & 0xFF) != 0) {
                switch (feature.Result[1] & 0xFF) {
                case 1:
                case 10:
                case 20:
                    return "Intel(R) Celeron(R) Processor";
                case 2:
                case 4:
                    return "Intel(R) Pentium(R) III Processor";
                case 3:
                    if (m_ProcessorSignature == 0x06b1) {
                        return "Intel(R) Celeron(R) Processor";
                    } else {
                        return "Intel(R) Pentium(R) III Xeon(R) Processor";
                    }
                case 6:
                    return "Mobile Intel(R) Pentium(R) III Processor-M";
                case 7:
                case 15:
                case 19:
                case 23:
                    return "Mobile Intel(R) Celeron(R) Processor";
                case 8:
                case 9:
                    return "Intel(R) Pentium(R) 4 Processor";
                case 11:
                    if (m_ProcessorSignature == 0x0F13) {
                        return "Intel(R) Xeon(R) Processor MP";
                    } else {
                        return "Intel(R) Xeon(R) Processor";
                    }
                case 12:
                    return "Intel(R) Xeon(R) Processor MP";
                case 14:
                    if (m_ProcessorSignature == 0x0F13) {
                        return "Intel(R) Xeon(R) Processor MP";
                    } else {
                        return "Mobile Intel(R) Pentium(R) 4 Processor-M";
                    }
                case 17:
                case 21:
                    return "Mobile Genuine Intel(R) Processor";
                case 18:
                    return "Intel(R) Celeron(R) M Processor";
                case 22:
                    return "Intel(R) Pentium(M) M Processor";
                default:
                    return "Intel(R) Processor";
                }
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

        /// <inheritdoc/>
        public override CpuVendor CpuVendor
        {
            get { return CpuVendor.GenuineIntel; }
        }
    }
}
