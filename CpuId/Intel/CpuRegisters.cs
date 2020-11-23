namespace RJCP.Diagnostics.Intel
{
    using System;
    using System.Runtime.InteropServices;
    using Native;

    internal class CpuRegisters : CpuRegistersBase
    {
        public CpuRegisters()
        {
            if (CpuIdLib.hascpuid() == 0)
                throw new PlatformNotSupportedException("CPUID instruction not supported");

            CpuIdLib.CpuIdInfo[] data = new CpuIdLib.CpuIdInfo[256];
            int r = CpuIdLib.iddump(data, Marshal.SizeOf(data[0]) * data.Length);
            for (int i = 0; i < r; i++) {
                CpuIdRegister result = new CpuIdRegister(data[i].veax, data[i].vecx,
                    new int[] { data[i].peax, data[i].pebx, data[i].pecx, data[i].pedx });
                AddRegister(result);
            }
        }

        public override CpuIdRegister GetCpuId(int function, int subfunction)
        {
            CpuIdRegister result = base.GetCpuId(function, subfunction);
            if (result == null) {
                // It's not cached, so query the CPU for it. Note, we assume that each EAX/ECX pair always returns the
                // same result for the same CPU core/thread.
                CpuIdLib.cpuid(function, subfunction, out int eax, out int ebx, out int ecx, out int edx);
                result = new CpuIdRegister(function, subfunction, new int[] { eax, ebx, ecx, edx });
                AddRegister(result);
            }

            return result;
        }
    }
}
