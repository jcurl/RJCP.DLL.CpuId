namespace RJCP.Diagnostics.Intel
{
    using FamilyTree;

    internal static partial class AmdBrandIdentifier
    {
        private static class Family14
        {
            // [PkgType][StringN][Pg][NC][StringIndex]
            private static readonly Node Family11hTree = new Node(20) {
                new Node(0) {
                    new Node(1) {
                        new Node(0) {
                            new Node(0) {
                                new Node(1, "AMD C-"),
                                new Node(2, "AMD E-"),
                                new Node(4, "AMD G-T")
                            },
                            new Node(1) {
                                new Node(1, "AMD C-"),
                                new Node(2, "AMD E-"),
                                new Node(3, "AMD Z-"),
                                new Node(4, "AMD G-T"),
                                new Node(5, "AMD E1-1"),
                                new Node(6, "AMD E2-1"),
                                new Node(7, "AMD E2-2"),
                            }
                        }
                    },
                    new Node(2) {
                        new Node(0) {
                            new Node(0) {
                                new Node(1, " Processor"),
                                new Node(2, "0 Processor"),
                                new Node(3, "5 Processor"),
                                new Node(4, "0x Processor"),
                                new Node(5, "5x Processor"),
                                new Node(6, "x Processor"),
                                new Node(7, "L Processor"),
                                new Node(8, "N Processor"),
                                new Node(9, "R Processor"),
                                new Node(10, "0 APU with Radeon(tm) HD Graphics"),
                                new Node(11, "5 APU with Radeon(tm) HD Graphics"),
                                new Node(12, " APU with Radeon(tm) HD Graphics"),
                                new Node(13, "0D APU with Radeon(tm) HD Graphics")
                            },
                            new Node(1) {
                                new Node(1, " Processor"),
                                new Node(2, "0 Processor"),
                                new Node(3, "5 Processor"),
                                new Node(4, "0x Processor"),
                                new Node(5, "5x Processor"),
                                new Node(6, "x Processor"),
                                new Node(7, "L Processor"),
                                new Node(8, "N Processor"),
                                new Node(9, "0 APU with Radeon(tm) HD Graphics"),
                                new Node(10, "5 APU with Radeon(tm) HD Graphics"),
                                new Node(11, " APU with Radeon(tm) HD Graphics"),
                                new Node(12, "E Processor"),
                                new Node(13, "0D APU with Radeon(tm) HD Graphics")
                            }
                        }
                    }
                }
            };

            public static string GetType(AuthenticAmdCpu cpu)
            {
                int brand = GetBrand(cpu);
                int str1 = (brand & 0x780000) >> 19;
                int str2 = (brand & 0xF00) >> 8;
                int pm = (brand & 0x3F000) >> 12;
                int pg = (brand & 0x800000) >> 23;

                CpuIdRegister ext81Reg = cpu.Registers.GetCpuId(GenericIntelCpuBase.ExtendedInformationFunction, 0);
                int pkgType = (ext81Reg.Result[1] >> 28) & 0xF;

                CpuIdRegister ext88Reg = cpu.Registers.GetCpuId(GenericIntelCpuBase.ExtendedLmApicId, 0);
                int nc = ext88Reg.Result[2] & 0xFF;

                if (pkgType >= 2) pm -= 1;
                string pma = string.Format("{0:D02}", pm);
                string s1 = Family11hTree[pkgType][1][pg][nc][str1].Value;
                if (!string.IsNullOrEmpty(s1)) {
                    string s2 = Family11hTree[pkgType][2][pg][nc][str2].Value;
                    if (string.IsNullOrEmpty(s2)) return string.Format("{0}{1}", s1, pma);
                    return string.Format("{0}{1}{2}", s1, pma, s2);
                }
                return "AMD Processor Model Unknown";
            }
        }
    }
}
