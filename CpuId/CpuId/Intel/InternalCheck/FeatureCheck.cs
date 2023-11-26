namespace RJCP.Diagnostics.CpuId.Intel.InternalCheck
{
    using System;

    internal static class FeatureCheck
    {
        public static void CheckReservedBitMask(this MainFunction function, CpuIdRegister register, int result, int mask)
        {
            int bit = 0;
            uint checkMask = unchecked((uint)mask);
            while (checkMask != 0) {
                if ((checkMask & 0x01) != 0 &&
                    !function.Set(register.Function, register.SubFunction, result, 1 << bit)) {
                    string message = string.Format("CPUID[EAX={0:X2}h,ECX={1:X2}h].{2}[{3}] already set",
                        register.Function, register.SubFunction, GenericIntelCpuBase.GetRegisterName(result), bit);
                    throw new InvalidOperationException(message);
                }
                checkMask >>= 1;
                bit++;
            }
        }
    }
}
