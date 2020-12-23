namespace RJCP.Diagnostics.CpuIdWin.Infrastructure
{
    using System.Collections;
    using System.Text;

    public static class Text
    {
        public static string ConvertToBitString(long value, int minLength)
        {
            const int space = 4;

            BitArray bstring = new BitArray(64);
            int upperBit = -1;
            for (int i = 0; i < 64; i++) {
                if (value != 0) {
                    bstring[i] = (value & 0x01) != 0;
                    value >>= 1;
                } else {
                    upperBit = i - 1;
                    if (upperBit < minLength - 1) upperBit = minLength - 1;
                    break;
                }
            }
            if (upperBit == -1) upperBit = 63;

            StringBuilder result = new StringBuilder(upperBit);
            for (int i = upperBit; i >= 0; --i) {
                result.Append(bstring[i] ? '1' : '0');
                if (i != 0 && i % space == 0) result.Append(' ');
            }
            return result.ToString();
        }
    }
}
