namespace RJCP.Diagnostics.CpuId.Intel
{
    using FamilyTree;

    internal static partial class AmdBrandIdentifier
    {
        private static class Family12
        {
            // [PkgType][StringN][Pg][NC][StringIndex]
            private static readonly Node Family11hTree = new(18) {
                new Node(1) {
                    new Node(1) {
                        new Node(0) {
                            new Node(1) {
                                new Node(3, "AMD A4-33"),
                                new Node(5, "AMD E2-30")
                            },
                            new Node(3) {
                                new Node(1, "AMD A8-35"),
                                new Node(1, "AMD A6-34")
                            }
                        }
                    },
                    new Node(2) {
                        new Node(0) {
                            new Node(1) {
                                new Node(1, "M APU with Radeon(tm) HD Graphics"),
                                new Node(2, "MX APU with Radeon(tm) HD Graphics")
                            },
                            new Node(3) {
                                new Node(1, "M APU with Radeon(tm) HD Graphics"),
                                new Node(2, "MX APU with Radeon(tm) HD Graphics")
                            }
                        }
                    }
                },
                new Node(2) {
                    new Node(1) {
                        new Node(0) {
                            new Node(1) {
                                new Node(1, "AMD A4-33"),
                                new Node(2, "AMD E2-32"),
                                new Node(4, "AMD Athlon(tm) II X2 2"),
                                new Node(5, "AMD A4-34"),
                                new Node(12, "AMD Sempron(tm) X2 1")
                            },
                            new Node(2) {
                                new Node(5, "AMD A6-35")
                            },
                            new Node(3) {
                                new Node(5, "AMD A8-38"),
                                new Node(6, "AMD A6-36"),
                                new Node(13, "AMD Athlon(tm) II X4 6")
                            }
                        }
                    },
                    new Node(2) {
                        new Node(0) {
                            new Node(1) {
                                new Node(1, " APU with Radeon(tm) HD Graphics"),
                                new Node(2, " Dual-Core Processor")
                            },
                            new Node(2) {
                                new Node(1, " APU with Radeon(tm) HD Graphics"),
                            },
                            new Node(3) {
                                new Node(1, " APU with Radeon(tm) HD Graphics"),
                                new Node(3, " Quad-Core Processor")
                            }
                        }
                    }
                }
            };

            public static string GetType(AuthenticAmdCpu cpu)
            {
                int brand = GetBrand(cpu);
                if (brand == 0) return "AMD Engineering Sample";

                int str1 = (brand & 0x780000) >> 19;
                int str2 = (brand & 0xF00) >> 8;
                int pm = ((brand & 0x7F000) >> 12) - 1;
                int pg = (brand & 0x800000) >> 23;

                CpuIdRegister ext81Reg = cpu.Registers.GetCpuId(GenericIntelCpuBase.ExtendedInformationFunction, 0);
                int pkgType = (ext81Reg.Result[1] >> 28) & 0xF;

                CpuIdRegister ext88Reg = cpu.Registers.GetCpuId(GenericIntelCpuBase.ExtendedFeatureIds, 0);
                int nc = ext88Reg.Result[2] & 0xFF;

                string pma = string.Format("{0:D02}", pm);
                string s1 = Family11hTree[pkgType][1][pg][nc][str1].Value;
                if (!string.IsNullOrEmpty(s1)) {
                    string s2 = Family11hTree[pkgType][2][pg][nc][str2].Value;
                    return string.IsNullOrEmpty(s2) ?
                        string.Format("{0}{1}", s1, pma) :
                        string.Format("{0}{1}{2}", s1, pma, s2);
                }
                return "AMD Processor Model Unknown";
            }
        }
    }
}
