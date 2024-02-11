namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Runtime.Versioning;
    using Native;

    [SupportedOSPlatform("windows")]
    internal class CpuRegisters : CpuRegistersBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CpuRegisters"/> class.
        /// </summary>
        /// <param name="data">The CPUID data.</param>
        /// <param name="offset">The offset into <paramref name="data"/> for the node in question.</param>
        /// <param name="length">The length of the cpu data <paramref name="data"/> for the node in question.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative</exception>
        /// <exception cref="ArgumentException">
        /// The <paramref name="length"/> and <paramref name="offset"/> would exceed the boundaries of the array/buffer
        /// <paramref name="data"/>.
        /// </exception>
        /// <remarks>
        /// Creates CPU data based on the native CPU data read.
        /// </remarks>
        public CpuRegisters(CpuIdLib.CpuIdInfo[] data, int offset, int length)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "offset is negative");
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), "length is negative");
            if (offset > data.Length - length) throw new ArgumentException("The length and offset would exceed the boundaries of the array/buffer");

            for (int i = 0; i < length; i++) {
                int r = offset + i;
                CpuIdRegister result = new CpuIdRegister(data[r].veax, data[r].vecx,
                    new int[] { data[r].peax, data[r].pebx, data[r].pecx, data[r].pedx });
                AddRegister(result);
            }
        }

        public override CpuIdRegister GetCpuId(int function, int subfunction)
        {
            CpuIdRegister result = base.GetCpuId(function, subfunction);
            if (result == null) {
                // It's not cached, so query the CPU for it. Note, we assume that each EAX/ECX pair always returns the
                // same result for the same CPU core/thread.
                _ = CpuIdLib.cpuid(function, subfunction, out int eax, out int ebx, out int ecx, out int edx);
                result = new CpuIdRegister(function, subfunction, new int[] { eax, ebx, ecx, edx });
                AddRegister(result);
            }

            return result;
        }

        public override bool IsOnline { get { return true; } }
    }
}
