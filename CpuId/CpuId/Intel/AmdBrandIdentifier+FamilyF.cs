namespace RJCP.Diagnostics.CpuId.Intel
{
    internal static partial class AmdBrandIdentifier
    {
        private static class FamilyF
        {
            public static string GetAthlonOpteronType(AuthenticAmdCpu cpu)
            {
                int brand = GetBrand(cpu);
                if (brand == 0) return "AMD Engineering Sample";

                int bti, nn;
                if ((brand & 0xFF) == 0) {
                    bti = (brand & 0x0FC000) >> 14;
                    nn = (brand & 0x3F00) >> 8;
                } else {
                    bti = (brand & 0xE0) >> 3;
                    nn = brand & 0x1F;
                }

                switch (bti) {
                case 0x00: return "AMD Engineering Sample";
                case 0x04: return string.Format("AMD Athlon(tm) 64 Processor {0:D02}00+", 22 + nn);
                case 0x05: return string.Format("AMD Athlon(tm) 64 X2 Dual Core Processor {0:D02}00+", 22 + nn);
                case 0x06: return string.Format("AMD Athlon(tm) 64 FX-{0:D02} Dual Core Processor", 24 + nn);
                case 0x08:
                case 0x09: return string.Format("Mobile AMD Athlon(tm) 64 Processor {0:D02}00+", 22 + nn);
                case 0x0A: return string.Format("AMD Turion(tm) 64 Mobile Technology ML-{0:D02}", 22 + nn);
                case 0x0B: return string.Format("AMD Turion(tm) 64 Mobile Technology MT-{0:D02}", 22 + nn);
                case 0x0C:
                case 0x0D: return string.Format("AMD Opteron(tm) Processor 1{0:D02}", 38 + 2 * nn);
                case 0x0E: return string.Format("AMD Opteron(tm) Processor 1{0:D02} HE", 38 + 2 * nn);
                case 0x0F: return string.Format("AMD Opteron(tm) Processor 1{0:D02} EE", 38 + 2 * nn);
                case 0x10:
                case 0x11: return string.Format("AMD Opteron(tm) Processor 2{0:D02}", 38 + 2 * nn);
                case 0x12: return string.Format("AMD Opteron(tm) Processor 2{0:D02} HE", 38 + 2 * nn);
                case 0x13: return string.Format("AMD Opteron(tm) Processor 2{0:D02} EE", 38 + 2 * nn);
                case 0x14:
                case 0x15: return string.Format("AMD Opteron(tm) Processor 8{0:D02}", 38 + 2 * nn);
                case 0x16: return string.Format("AMD Opteron(tm) Processor 8{0:D02} HE", 38 + 2 * nn);
                case 0x17: return string.Format("AMD Opteron(tm) Processor 8{0:D02} EE", 38 + 2 * nn);
                case 0x18: return string.Format("AMD Athlon(tm) 64 Processor {0:D02}00+", 9 + nn);
                case 0x1D:
                case 0x1E: return string.Format("Mobile AMD Athlon(tm) XP-M Processor {0:D02}00+", 22 + nn);
                case 0x20: return string.Format("AMD Athlon(tm) XP Processor {0:D02}00+", 22 + nn);
                case 0x21:
                case 0x23: return string.Format("Mobile AMD Sempron(tm) Processor {0:D02}00+", 24 + nn);
                case 0x22:
                case 0x26: return string.Format("AMD Sempron(tm) Processor {0:D02}00+", 24 + nn);
                case 0x24: return string.Format("AMD Athlon(tm) 64 FX-{0:D02} Processor", 24 + nn);
                case 0x29: return string.Format("Dual Core AMD Opteron(tm) Processor 1{0:D02} SE", 45 + 5 * nn);
                case 0x2A: return string.Format("Dual Core AMD Opteron(tm) Processor 2{0:D02} SE", 45 + 5 * nn);
                case 0x2B: return string.Format("Dual Core AMD Opteron(tm) Processor 8{0:D02} SE", 45 + 5 * nn);
                case 0x2C:
                case 0x2D:
                case 0x38:
                case 0x3B: return string.Format("Dual Core AMD Opteron(tm) Processor 1{0:D02}", 45 + 5 * nn);
                case 0x2E: return string.Format("Dual Core AMD Opteron(tm) Processor 1{0:D02} HE", 45 + 5 * nn);
                case 0x2F: return string.Format("Dual Core AMD Opteron(tm) Processor 1{0:D02} EE", 45 + 5 * nn);
                case 0x30:
                case 0x31:
                case 0x39:
                case 0x3C: return string.Format("Dual Core AMD Opteron(tm) Processor 2{0:D02}", 45 + 5 * nn);
                case 0x32: return string.Format("Dual Core AMD Opteron(tm) Processor 2{0:D02} HE", 45 + 5 * nn);
                case 0x33: return string.Format("Dual Core AMD Opteron(tm) Processor 2{0:D02} EE", 45 + 5 * nn);
                case 0x34:
                case 0x35:
                case 0x3A:
                case 0x3D: return string.Format("Dual Core AMD Opteron(tm) Processor 8{0:D02}", 45 + 5 * nn);
                case 0x36: return string.Format("Dual Core AMD Opteron(tm) Processor 8{0:D02} HE", 45 + 5 * nn);
                case 0x37: return string.Format("Dual Core AMD Opteron(tm) Processor 8{0:D02} EE", 45 + 5 * nn);
                default: return "AMD Processor model unknown";
                }
            }

            public static string GetNptType(AuthenticAmdCpu cpu)
            {
                int brand = GetBrand(cpu);
                int pwrLim = ((brand & 0x1C000) >> 13) | ((brand & 0x400000) >> 22);
                int bti = (brand & 0x3E0000) >> 17;
                int nn = ((brand & 0x800000) >> 17) | ((brand & 0x3F00) >> 8);

                CpuIdRegister pkgReg = cpu.Registers.GetCpuId(GenericIntelCpuBase.ExtendedInformationFunction, 0);
                int pkgType = (pkgReg.Result[0] & 0x30) >> 4;

                CpuIdRegister cmpReg = cpu.Registers.GetCpuId(GenericIntelCpuBase.ExtendedLmApicId, 0);
                int cmpCap = (cmpReg.Result[2] & 0xFF) == 0 ? 0 : 1;

                switch (pkgType) {
                case 0x00: return GetNptTypeS1g1(pwrLim, bti, nn, cmpCap); // Table 9
                case 0x01: return GetNptTypeFr3F(pwrLim, bti, nn, cmpCap); // Table 7
                case 0x03: return GetNptTypeAm2Asb1(pwrLim, bti, nn, cmpCap); // Table 8
                }
                return "AMD Processor model unknown";
            }

            private static string GetNptTypeFr3F(int pwrLim, int bti, int nn, int cmpCap)
            {
                if (cmpCap == 0) {
                    switch (bti) {
                    case 0x01:
                        if (pwrLim == 2) return string.Format("AMD Opteron(tm) Processor 22{0:D02} EE", nn - 1);
                        break;
                    }
                    return "AMD Processor model unknown";
                }

                switch (bti) {
                case 0x00:
                    if (pwrLim == 2) return string.Format("Dual-Core AMD Opteron(tm) Processor 12{0:D02} EE", nn - 1);
                    if (pwrLim == 6) return string.Format("Dual-Core AMD Opteron(tm) Processor 12{0:D02} HE", nn - 1);
                    break;
                case 0x01:
                    if (pwrLim == 2) return string.Format("Dual-Core AMD Opteron(tm) Processor 22{0:D02} EE", nn - 1);
                    if (pwrLim == 6) return string.Format("Dual-Core AMD Opteron(tm) Processor 22{0:D02} HE", nn - 1);
                    if (pwrLim == 10) return string.Format("Dual-Core AMD Opteron(tm) Processor 22{0:D02}", nn - 1);
                    if (pwrLim == 12) return string.Format("Dual-Core AMD Opteron(tm) Processor 22{0:D02} SE", nn - 1);
                    break;
                case 0x04:
                    if (pwrLim == 2) return string.Format("Dual-Core AMD Opteron(tm) Processor 82{0:D02} EE", nn - 1);
                    if (pwrLim == 6) return string.Format("Dual-Core AMD Opteron(tm) Processor 82{0:D02} HE", nn - 1);
                    if (pwrLim == 10) return string.Format("Dual-Core AMD Opteron(tm) Processor 82{0:D02}", nn - 1);
                    if (pwrLim == 12) return string.Format("Dual-Core AMD Opteron(tm) Processor 82{0:D02} SE", nn - 1);
                    break;
                case 0x06:
                    if (pwrLim == 14) return string.Format("AMD Athlon(tm) 64 FX-{0:D02} Processor", 57 + nn);
                    break;
                }
                return "AMD Processor model unknown";
            }

            private static string GetNptTypeAm2Asb1(int pwrLim, int bti, int nn, int cmpCap)
            {
                if (cmpCap == 0) {
                    switch (bti) {
                    case 0x01:
                        if (pwrLim == 5) return string.Format("AMD Sempron(tm) Processor LE-1{0:D02}0", nn - 1);
                        break;
                    case 0x02:
                        if (pwrLim == 6) return string.Format("AMD Athlon(tm) Processor LE-1{0:D02}0", 57 + nn);
                        break;
                    case 0x03:
                        if (pwrLim == 6) return string.Format("AMD Athlon(tm) Processor 1{0:D02}0B", 57 + nn);
                        break;
                    case 0x04:
                        switch (pwrLim) {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 8: return string.Format("AMD Athlon(tm) 64 Processor {0:D02}00+", 15 + nn);
                        }
                        break;
                    case 0x05:
                        if (pwrLim == 2) return string.Format("AMD Sempron(tm) Processor {0:D02}50p", nn - 1);
                        break;
                    case 0x06:
                        switch (pwrLim) {
                        case 4:
                        case 8: return string.Format("AMD Sempron(tm) Processor {0:D02}00+", 15 + nn);
                        }
                        break;
                    case 0x07:
                        switch (pwrLim) {
                        case 1:
                        case 2: return string.Format("AMD Sempron(tm) Processor {0:D02}0U", 15 + nn);
                        }
                        break;
                    case 0x08:
                        switch (pwrLim) {
                        case 2:
                        case 3: return string.Format("AMD Athlon(tm) Processor {0:D02}50e", 15 + nn);
                        }
                        break;
                    case 0x09:
                        if (pwrLim == 2) return string.Format("AMD Athlon(tm) Neo Processor MV-{0:D02}", 15 + nn);
                        break;
                    case 0x0C:
                        if (pwrLim == 2) return string.Format("AMD Sempron(tm) Processor 2{0:D02}U", nn - 1);
                        break;
                    }
                    return "AMD Processor model unknown";
                }

                switch (bti) {
                case 0x01:
                    if (pwrLim == 6) return string.Format("Dual-Core AMD Opteron(tm) Processor 12{0:D02} HE", nn - 1);
                    if (pwrLim == 10) return string.Format("Dual-Core AMD Opteron(tm) Processor 12{0:D02}", nn - 1);
                    if (pwrLim == 12) return string.Format("Dual-Core AMD Opteron(tm) Processor 12{0:D02} SE", nn - 1);
                    break;
                case 0x03:
                    if (pwrLim == 3) return string.Format("AMD Athlon(tm) X2 Dual Core Processor BE-2{0:D02}0", 25 + nn);
                    break;
                case 0x04:
                    switch (pwrLim) {
                    case 1:
                    case 2:
                    case 6:
                    case 8:
                    case 12: return string.Format("AMD Athlon(tm) 64 X2 Dual Core Processor {0:D02}00+", 25 + nn);
                    }
                    break;
                case 0x05:
                    if (pwrLim == 12) return string.Format("AMD Athlon(tm) 64 FX-{0:D02} Dual Core Processor", 57 + nn);
                    break;
                case 0x06:
                    if (pwrLim == 6) return string.Format("AMD Sempron(tm) Dual Core Processor {0:D02}00", nn - 1);
                    break;
                case 0x07:
                    switch (pwrLim) {
                    case 3: return string.Format("AMD Athlon(tm) Dual Core Processor {0:D02}50e", 25 + nn);
                    case 6:
                    case 7: return string.Format("AMD Athlon(tm) Dual Core Processor {0:D02}00B", 25 + nn);
                    }
                    break;
                case 0x08:
                    if (pwrLim == 3) return string.Format("AMD Athlon(tm) Dual Core Processor {0:D02}50B", 25 + nn);
                    break;
                case 0x09:
                    if (pwrLim == 1) return string.Format("AMD Athlon(tm) X2 Dual Core Processor {0:D02}50e", 25 + nn);
                    break;
                case 0x0A:
                    switch (pwrLim) {
                    case 1:
                    case 2: return string.Format("AMD Athlon(tm) Neo X2 Dual Core Processor {0:D02}50e", 25 + nn);
                    }
                    break;
                case 0x0B:
                    if (pwrLim == 0) return string.Format("AMD Turion(tm) Neo X2 Dual Core Processor L6{0:D02}", nn - 1);
                    break;
                case 0x0C:
                    if (pwrLim == 0) return string.Format("AMD Turion(tm) Neo X2 Dual Core Processor L3{0:D02}", nn - 1);
                    break;
                }
                return "AMD Processor model unknown";
            }

            private static string GetNptTypeS1g1(int pwrLim, int bti, int nn, int cmpCap)
            {
                if (cmpCap == 0) {
                    switch (bti) {
                    case 0x01:
                        if (pwrLim == 2) return string.Format("AMD Athlon(tm) 64 Processor {0:D02}00+", 15 + nn);
                        break;
                    case 0x02:
                        if (pwrLim == 12) return string.Format("AMD Turion(tm) 64 Mobile Technology MK-{0:D02}", 29 + nn);
                        break;
                    case 0x03:
                        switch (pwrLim) {
                        case 1: return string.Format("Mobile AMD Sempron(tm) Processor {0:D02}00+", 15 + nn);
                        case 6:
                        case 12: return string.Format("Mobile AMD Sempron(tm) Processor {0:D02}00+", 26 + nn);
                        }
                        break;
                    case 0x04:
                        if (pwrLim == 2) return string.Format("AMD Sempron(tm) Processor {0:D02}00+", 15 + nn);
                        break;
                    case 0x06:
                        switch (pwrLim) {
                        case 4:
                        case 6:
                        case 12: return string.Format("AMD Athlon(tm) Processor TF-{0:D02}", 15 + nn);
                        }
                        break;
                    case 0x07:
                        if (pwrLim == 3) return string.Format("AMD Athlon(tm) Processor L1{0:D02}", nn - 1);
                        break;
                    }
                    return "AMD Processor model unknown";
                }

                switch (bti) {
                case 0x01:
                    if (pwrLim == 12) return string.Format("AMD Sempron(tm) Dual Core Processor TJ-{0:D02}", 29 + nn);
                    break;
                case 0x02:
                    if (pwrLim == 12) return string.Format("AMD Turion(tm) 64 X2 Mobile Technology TL-{0:D02}", 29 + nn);
                    break;
                case 0x03:
                    switch (pwrLim) {
                    case 4:
                    case 12: return string.Format("AMD Athlon(tm) 64 X2 Dual-Core Processor TK-{0:D02}", 29 + nn);
                    }
                    break;
                case 0x05:
                    if (pwrLim == 4) return string.Format("AMD Athlon(tm) 64 X2 Dual Core Processor {0:D02}00+", 25 + nn);
                    break;
                case 0x06:
                    if (pwrLim == 2) return string.Format("AMD Athlon(tm) X2 Dual Core Processor L3{0:D02}", nn - 1);
                    break;
                case 0x07:
                    if (pwrLim == 4) return string.Format("AMD Turion(tm) X2 Dual Core Processor L5{0:D02}", nn - 1);
                    break;
                }
                return "AMD Processor model unknown";
            }
        }
    }
}
