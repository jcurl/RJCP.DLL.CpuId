namespace RJCP.Diagnostics.CpuId.Intel
{
    /// <summary>
    /// CPU information for a generic Intel Clone.
    /// </summary>
    public class GenericIntelCpu : GenericIntelCpuBase
    {
        internal GenericIntelCpu(BasicCpu cpu) : base(cpu)
        {
            if (cpu.FunctionCount == 0) return;

            CpuIdRegister feature = cpu.CpuRegisters.GetCpuId(FeatureInformationFunction, 0);
            int eax = feature.Result[0];
            ProcessorSignature = eax & 0x0FFF3FFF;

            int extendedFamily = (ProcessorSignature >> 20) & 0xFF;
            int extendedModel = (ProcessorSignature >> 16) & 0xF;
            int familyCode = (ProcessorSignature >> 8) & 0xF;
            int modelNumber = (ProcessorSignature >> 4) & 0xF;

            Family = extendedFamily + familyCode;
            Model = (extendedModel << 4) + modelNumber;
            ProcessorType = (ProcessorSignature >> 12) & 0x3;
            Stepping = ProcessorSignature & 0xF;
            Description = string.Empty;
        }
    }
}
