namespace RJCP.Diagnostics.Intel
{
    using System.Collections.Generic;

    internal static class IntelLegacySignatures
    {
        private static readonly Dictionary<int, string> I486 = new Dictionary<int, string>() {
            [0x003] = "IntelDX2 OverDrive",
            [0x004] = "Intel486 SL",
            [0x005] = "IntelSX2",
            [0x007] = "IntexDX2 Write-Back Enhanced",
            [0x008] = "IntexDX4(TM)",
            [0x018] = "IntexDX4(TM) OverDrive"
        };

        private static readonly Dictionary<int, string> Pentium = new Dictionary<int, string>() {
            [0x001] = "Pentium (60, 66)",
            [0x002] = "Pentium (75, 90, 100, 120, 133, 150, 166, 200)",
            [0x011] = "Pentium (60, 66) OverDrive",
            [0x012] = "Pentium (75, 90, 100, 120, 133) OverDrive",
            [0x013] = "Pentium OverDrive for Intel486 Systems",
            [0x008] = "Pentium (166, 200) MMX(TM)",
            [0x018] = "Pentium (75, 90, 100, 120, 133) MMX(TM)"
        };

        private static readonly Dictionary<int, string> PentiumPro = new Dictionary<int, string>() {
            [0x001] = "Pentium Pro",
            [0x003] = "Pentium II, Model 03h",
            [0x005] = "Pentium II, Xeon, Celeron, Model 05h",
            [0x105] = "EP80579 Integrated Processor",
            [0x006] = "Celeron, Model 06h",
            [0x007] = "Pentium III, Xeon, Model 07h",
            [0x008] = "Pentium III, Xeon, Celeron, Model 08h",
            [0x009] = "Pentium M, Celeron M, Model 09h",
            [0x00A] = "Pentium III Xeon, Model 0Ah",
            [0x00B] = "Pentium III Xeon, Model 0Bh",
            [0x00D] = "Pentium M, Celeron M, Model 0Dh, 90nm",
            [0x00E] = "Intel Core(TM) Solo, Intel Core(TM) Duo, Model 0Eh, 65nm",
            [0x00F] = "Intel Core(TM)2 Duo, Core(TM)2 Duo Mobile, Core(TM)2 Quad, Core(TM)2 Quad Mobile, Core(TM)2 Extreme, Pentium Dual-Core, Xeon, Model 0Fh, 65nm",
            [0x106] = "Intel Celeron, Model 16h, 65nm",
            [0x107] = "Intel Core(TM)2 Extreme, Xeon, Model 17h, 45nm",
            [0x013] = "Intel Pentium II OverDrive",
            [0x10C] = "Intel Atom, 45nm",
            [0x10A] = "Intel Core i7, Xeon, 45nm",
            [0x10D] = "Intel Xeon MP, 45nm",
            [0x10E] = "Intel Core i5/i7, Core i5/i7 Mobile, Xeon, 45nm",
            [0x20E] = "Intel Xeon MP, 45nm",
            [0x20F] = "Intel Xeon MP, 32nm",
            [0x20C] = "Intel Core i7, Xeon, 32nm",
            [0x205] = "Intel Core i3, Intel Core i3/i5 Mobile, 32nm",
            [0x20A] = "Intel Core 2nd Gen Mobile and Desktop, Xeon E3-1200 Family, Sandy Bridge, 32nm",
            [0x20D] = "Intel Xeon E5 Family, Sandy Bridge, 32nm",
            [0x309] = "Intel Core 3rd Gen, Xeon E3-1200 v2, Sandy Bridge, 22nm"
        };

        private static readonly Dictionary<int, string> Pentium4 = new Dictionary<int, string>() {
            [0x000] = "Pentium 4, Xeon",
            [0x001] = "Pentium 4, Intel Xeon, Intel Xeon MP, Intel Celeron, Model 01h, 0.18um",
            [0x002] = "Pentium 4, Mobile Intel Pentium 4, Intel Xeon, Intel Xeon MP, Intel Celeron, Intel Mobile Celeron, 0.13um",
            [0x003] = "Pentium 4, Intel Xeon, Intel Celeron D, Model 03h, 90nm",
            [0x004] = "Pentium 4, Pentium 4 Extreme, Pentium D OverDrive, Intel Xeon, Intel Xeon MP, Intel Celeron D, Model 04h, 90nm",
            [0x006] = "Pentium 4, Pentium D, Pentium Extreme, Xeon, Xeon MP, Celeron D, Model 06h, 65nm",
        };

        public static string GetType(int extfamily, int extmodel, int type, int family, int model)
        {
            if (extfamily != 0) return string.Empty;
            switch (family) {
            case 3:
                return GetI386Type(type, model);
            case 4:
                return GetCalculatedType(I486, extmodel, type, model, "Intel486");
            case 5:
                return GetCalculatedType(Pentium, extmodel, type, model, "Pentium Family");
            case 6:
                return GetCalculatedType(PentiumPro, extmodel, type, model, "Pentium Pro Family");
            case 15:
                return GetCalculatedType(Pentium4, extmodel, type, model, "Pentium 4 Family");
            }
            return string.Empty;
        }

        private static string GetI386Type(int type, int model)
        {
            switch ((type << 4) | model) {
            case 0x00: return "Intel386(TM) DX processor";
            case 0x20: return "Intel386 SX/CS/EX processor";
            case 0x40:
            case 0x41: return "Intel386 SL processor";
            case 0x04: return "RapidCAD coprocessor";
            default: return "Intel386";
            }
        }

        private static string GetCalculatedType(Dictionary<int, string> mapping, int extmodel, int type, int model, string defaultName)
        {
            if (extmodel != 0) return defaultName;
            int index = (extmodel << 8) | (type << 4) | model;
            if (mapping.TryGetValue(index, out string description)) return description;
            return defaultName;
        }
    }
}
