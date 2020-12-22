namespace RJCP.Diagnostics.CpuId.Intel
{
    using FamilyTree;

    internal static partial class AmdBrandIdentifier
    {
        private static class Family10
        {
            // [PkgType][StringN][Pg][NC][StringIndex]
            private static readonly Node Family10hTree = new Node(16) {
                // PkgType 0
                new Node(0) {
                    // Table 14
                    new Node(1) {
                        new Node(0) {
                            new Node(3) {
                                new Node(0, "Quad-Core AMD Opteron(tm) Processor 83"),
                                new Node(1, "Quad-Core AMD Opteron(tm) Processor 23")
                            },
                            new Node(5) {
                                new Node(0, "Six-Core AMD Opteron(tm) Processor 84"),
                                new Node(1, "Six-Core AMD Opteron(tm) Processor 24")
                            }
                        },
                        new Node(1) {
                            new Node(3) {
                                new Node(1, "Embedded AMD Opteron(tm) Processor")
                            },
                            new Node(5) {
                                new Node(1, "Embedded AMD Opteron(tm) Processor")
                            }
                        }
                    },
                    // Table 15
                    new Node(2) {
                        new Node(0) {
                            new Node(3) {
                                new Node(10, " SE"),
                                new Node(11, " HE"),
                                new Node(12, " EE")
                            },
                            new Node(5) {
                                new Node(0, " SE"),
                                new Node(1, " HE"),
                                new Node(2, " EE"),
                            },
                        },
                        new Node(1) {
                            new Node(3) {
                                new Node(1, "GF HE"),
                                new Node(2, "HF HE"),
                                new Node(3, "VS"),
                                new Node(4, "QS HE"),
                                new Node(5, "NP HE"),
                                new Node(6, "KH HE"),
                                new Node(7, "KS EE")
                            },
                            new Node(5) {
                                new Node(1, "QS"),
                                new Node(2, "KS HE")
                            }
                        }
                    }
                },
                // PkgType 1
                new Node(1) {
                    // Table 16
                    new Node(1) {
                        new Node(0) {
                            new Node(0) {
                                new Node(2, "AMD Sempron(tm) 1"),
                                new Node(3, "AMD Athlon(tm) II 1")
                            },
                            new Node(1) {
                                new Node(1, "AMD Athlon(tm)"),
                                new Node(3, "AMD Athlon(tm) II X2 2"),
                                new Node(4, "AMD Athlon(tm) II X2 B"),
                                new Node(5, "AMD Athlon(tm) II X2"),
                                new Node(7, "AMD Phenom(tm) II X2 5"),
                                new Node(10, "AMD Phenom(tm) II X2"),
                                new Node(11, "AMD Phenom(tm) II X2 B"),
                                new Node(12, "AMD Sempron(tm) X2 1")
                            },
                            new Node(2) {
                                new Node(0, "AMD Phenom(tm)"),
                                new Node(3, "AMD Phenom(tm) II X3 B"),
                                new Node(4, "AMD Phenom(tm) II X3"),
                                new Node(7, "AMD Athlon(tm) II X2 4"),
                                new Node(8, "AMD Phenom(tm) II X3 7"),
                                new Node(10, "AMD Athlon(tm) II X3")
                            },
                            new Node(3) {
                                new Node(0, "Quad-Core AMD Opteron(tm) Processor 13"),
                                new Node(2, "AMD Phenom(tm)"),
                                new Node(3, "AMD Phenom(tm) II X4 9"),
                                new Node(4, "AMD Phenom(tm) II X4 8"),
                                new Node(7, "AMD Phenom(tm) II X4 B"),
                                new Node(8, "AMD Phenom(tm) II X4"),
                                new Node(10, "AMD Athlon(tm) II X4 6"),
                                new Node(15, "AMD Athlon(tm) II X4")
                            },
                            new Node(5) {
                                new Node(0, "AMD Phenom(tm) II X6 1")
                            }
                        },
                        new Node(1) {
                            new Node(1) {
                                new Node(1, "AMD Athlon(tm) II XLT V"),
                                new Node(2, "AMD Athlon(tm) II XL V")
                            },
                            new Node(3) {
                                new Node(1, "AMD Phenom(tm) II XLT Q"),
                                new Node(2, "AMD Phenom(tm) II X4 9"),
                                new Node(3, "AMD Phenom(tm) II X4 8"),
                                new Node(4, "AMD Phenom(tm) II X4 6")
                            }
                        }
                    },
                    // Table 17
                    new Node(2) {
                        new Node(0) {
                            new Node(0) {
                                new Node(10, " Processor"),
                                new Node(11, "u Processor")
                            },
                            new Node(1) {
                                new Node(3, "50 Dual-Core Processor"),
                                new Node(6, " Processor"),
                                new Node(7, "e Processor"),
                                new Node(9, "0 Processor"),
                                new Node(10, "0e Processor"),
                                new Node(11, "u Processor")
                            },
                            new Node(2) {
                                new Node(0, "00 Triple-Core Processor"),
                                new Node(1, "00e Triple-Core Processor"),
                                new Node(2, "00B Triple-Core Processor"),
                                new Node(3, "50 Triple-Core Processor"),
                                new Node(4, "50e Triple-Core Processor"),
                                new Node(5, "50B Triple-Core Processor"),
                                new Node(6, " Processor"),
                                new Node(7, "e Processor"),
                                new Node(9, "0e Processor"),
                                new Node(10, "0 Processor")
                            },
                            new Node(3) {
                                new Node(0, "00 Quad-Core Processor"),
                                new Node(1, "00e Quad-Core Processor"),
                                new Node(2, "00B Quad-Core Processor"),
                                new Node(3, "50 Quad-Core Processor"),
                                new Node(4, "50e Quad-Core Processor"),
                                new Node(5, "50B Quad-Core Processor"),
                                new Node(6, " Processor"),
                                new Node(7, "e Processor"),
                                new Node(8, "0e Processor"),
                                new Node(14, "0 Processor")
                            },
                            new Node(5) {
                                new Node(0, "5T Processor"),
                                new Node(1, "0T Processor")
                            },
                        },
                        new Node(1) {
                            new Node(1) {
                                new Node(1, "L Processor"),
                                new Node(2, "C Processor")
                            },
                            new Node(3) {
                                new Node(1, "L Processor"),
                                new Node(4, "T Processor")
                            }
                        }
                    }
                },
                // PkgType 2
                new Node(2) {
                    // Table 18
                    new Node(1) {
                        new Node(0) {
                            new Node(0) {
                                new Node(0, "AMD Sempron(tm) M1"),
                                new Node(1, "AMD V")
                            },
                            new Node(1) {
                                new Node(0, "AMD Turion(tm) II Ultra Dual-Core Mobile M6"),
                                new Node(1, "AMD Turion(tm) II Dual-Core Mobile M5"),
                                new Node(2, "AMD Athlon(tm) II Dual-Core M3"),
                                new Node(3, "AMD Turion(tm) II P"),
                                new Node(4, "AMD Athlon(tm) II P"),
                                new Node(5, "AMD Phenom(tm) II X"),
                                new Node(6, "AMD Phenom(tm) II N"),
                                new Node(7, "AMD Turion(tm) II N"),
                                new Node(8, "AMD Athlon(tm) II N"),
                                new Node(9, "AMD Phenom(tm) II P"),
                            },
                            new Node(2) {
                                new Node(2, "AMD Phenom(tm) II P"),
                                new Node(3, "AMD Phenom(tm) II N"),
                                new Node(4, "AMD Phenom(tm) II X")
                            },
                            new Node(3) {
                                new Node(1, "AMD Phenom(tm) II P"),
                                new Node(2, "AMD Phenom(tm) II X"),
                                new Node(3, "AMD Phenom(tm) II N")
                            }
                        }
                    },
                    // Table 19
                    new Node(2) {
                        new Node(0) {
                            new Node(0) {
                                new Node(1, "0 Processor")
                            },
                            new Node(1) {
                                new Node(2, "0 Dual-Core Processor")
                            },
                            new Node(2) {
                                new Node(2, "0 Triple-Core Processor"),
                            },
                            new Node(3) {
                                new Node(1, "0 Quad-Core Processor")
                            }
                        }
                    }
                },
                // PkgType 3
                new Node(3) {
                    // Table 20
                    new Node(1) {
                        new Node(0) {
                            new Node(7) {
                                new Node(0, "AMD Opteron(tm) Processor 61")
                            },
                            new Node(11) {
                                new Node(0, "AMD Opteron(tm) Processor 61")
                            },
                        },
                        new Node(1) {
                            new Node(7) {
                                new Node(1, "Embedded AMD Opteron(tm) Processor")
                            }
                        }
                    },
                    // Table 21
                    new Node(2) {
                        new Node(0) {
                            new Node(7) {
                                new Node(0, " HE"),
                                new Node(1, " SE")
                            },
                            new Node(11) {
                                new Node(0, " HE"),
                                new Node(1, " SE")
                            }
                        },
                        new Node(1) {
                            new Node(7) {
                                new Node(1, "QS"),
                                new Node(2, "KS")
                            }
                        }
                    }
                },
                // PkgType 4
                new Node(4) {
                    // Table 22
                    new Node(1) {
                        new Node(0) {
                            new Node(0) {
                                new Node(1, "AMD Athlon(tm) II Neo K"),
                                new Node(2, "AMD V"),
                                new Node(3, "AMD Athlon(tm) II Neo R")
                            },
                            new Node(1) {
                                new Node(1, "AMD Turion(tm) II Neo K"),
                                new Node(2, "AMD Athlon(tm) II Neo K"),
                                new Node(3, "AMD V"),
                                new Node(4, "AMD Turion(tm) II Neo N"),
                                new Node(5, "AMD Athlon(tm) II Neo N"),
                            }
                        }
                    },
                    // Table 23
                    new Node(2) {
                        new Node(0) {
                            new Node(0) {
                                new Node(1, "5 Processor"),
                                new Node(2, "L Processor")
                            },
                            new Node(1) {
                                new Node(1, "5 Dual-Core Processor"),
                                new Node(2, "L Dual-Core Processor"),
                                new Node(4, "H Dual-Core Processor")
                            }
                        }
                    }
                },
                // PkgType 5
                new Node(5) {
                    // Table 24
                    new Node(1) {
                        new Node(0) {
                            new Node(3) {
                                new Node(0, "AMD Opteron(tm) Processor 41")
                            },
                            new Node(5) {
                                new Node(0, "AMD Opteron(tm) Processor 41")
                            }
                        },
                        new Node(1) {
                            new Node(3) {
                                new Node(1, "Embedded AMD Opteron(tm) Processor")
                            },
                            new Node(5) {
                                new Node(1, "Embedded AMD Opteron(tm) Processor")
                            }
                        }
                    },
                    // Table 25
                    new Node(2) {
                        new Node(0) {
                            new Node(3) {
                                new Node(0, " HE"),
                                new Node(1, " EE")
                            },
                            new Node(5) {
                                new Node(0, " HE"),
                                new Node(1, " EE")
                            }
                        },
                        new Node(1) {
                            new Node(3) {
                                new Node(1, "QS HE"),
                                new Node(2, "LE HE"),
                                new Node(3, "CL EE")
                            },
                            new Node(5) {
                                new Node(1, "KX HE"),
                                new Node(2, "GL EE")
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

                CpuIdRegister pkgReg = cpu.Registers.GetCpuId(GenericIntelCpuBase.ExtendedInformationFunction, 0);
                int pkgType = (pkgReg.Result[1] >> 28) & 0xF;

                CpuIdRegister ncReg = cpu.Registers.GetCpuId(GenericIntelCpuBase.ExtendedLmApicId, 0);
                int nc = ncReg.Result[2] & 0xFF;

                if (pkgType >= 2) pm -= 1;
                string pma = string.Format("{0:D02}", pm);
                string s1 = Family10hTree[pkgType][1][pg][nc][str1].Value;
                if (!string.IsNullOrEmpty(s1)) {
                    string s2 = Family10hTree[pkgType][2][pg][nc][str2].Value;
                    if (string.IsNullOrEmpty(s2)) return string.Format("{0}{1}", s1, pma);
                    return string.Format("{0}{1}{2}", s1, pma, s2);
                }
                return "AMD Processor Model Unknown";
            }
        }
    }
}
