namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.Collections.Generic;

    internal static partial class AmdBrandIdentifier
    {
        private static class DefaultFamily
        {
            private static readonly Dictionary<int, string> AmdLegacy = new() {
                // Names taken from AMD K85(TM) Family BIOS and Software Tools 21062C/0
                [0x50] = "AMD-K5 (PR75, PR90, PR100)",
                [0x51] = "AMD-K5 (PR120, PR133)",
                [0x52] = "AMD-K5 (PR150, PR166)",
                [0x53] = "AMD-K5-PR200",
                [0x56] = "AMD-K6(tm)",
                [0x57] = "AMD-K6(R) Model 7",
                [0x58] = "AMD-K6(R)-2 Model 8",
                [0x59] = "AMD-K6(R)-III Model 9",
                [0x61] = "AMD Athlon(TM) Model 1",
                [0x62] = "AMD Athlon Model 2",
                [0x63] = "AMD Duron(TM), Duron(TM) Mobile, Model 3",
                [0x64] = "AMD Athlon Model 4",
                [0x66] = "AMD Athlon MP/XP, Mobile AMD Athlon/Duron 4 Model 6",
                [0x67] = "AMD Duron, Duron Mobile, Model 7",
                [0x68] = "AMD Athlon MP/XP Model 8",
                [0x6A] = "AMD Athlon MP/XP-M, Athlon XP Model 8; Sempron, Athlon MP/XP Model 10"
            };

            public static string GetType(AuthenticAmdCpu cpu)
            {
                int key = (cpu.Family << 4) | cpu.Model;
                return AmdLegacy.TryGetValue(key, out string brandName) ?
                    brandName :
                    string.Empty;
            }
        }
    }
}
