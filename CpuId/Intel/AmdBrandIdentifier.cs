namespace RJCP.Diagnostics.Intel
{
    internal static partial class AmdBrandIdentifier
    {
        public static string GetType(AuthenticAmdCpu cpu)
        {
            switch (cpu.Family) {
            case 0x04: return "Am486(R) and Am5x86 Processor";
            case 0x05:
            case 0x06: return DefaultFamily.GetType(cpu);
            case 0x0F:
                // Document #25759 r3.79
                if (cpu.Model < 0x40) return FamilyF.GetAthlonOpteronType(cpu);
                return FamilyF.GetNptType(cpu);
            case 0x10: return Family10.GetType(cpu);
            case 0x11: return Family11.GetType(cpu);
            case 0x12: return Family12.GetType(cpu);
            case 0x14: return Family14.GetType(cpu);
            default: return string.Empty;
            }
        }

        private static int GetBrand(AuthenticAmdCpu cpu)
        {
            CpuIdRegister feature = cpu.Registers.GetCpuId(GenericIntelCpuBase.FeatureInformationFunction, 0);
            if (feature == null) return 0;
            int brand = feature.Result[1] & 0xFF;

            CpuIdRegister extFeature = cpu.Registers.GetCpuId(GenericIntelCpuBase.ExtendedInformationFunction, 0);
            if (extFeature == null) return brand;
            return brand | ((extFeature.Result[1] & 0xFFFF) << 8);
        }
    }
}
