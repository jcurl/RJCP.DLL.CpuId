namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using Native.Win32;

    [SupportedOSPlatform("windows")]
    internal class CpuIdLibRegisters : ICpuRegisters
    {
        internal const int MaxCpuLeaves = 256;

        private readonly List<CpuIdRegister> m_RegisterList = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CpuIdLibRegisters"/> class.
        /// </summary>
        /// <remarks>
        /// Queries the local machine for CPU data.
        /// </remarks>
        public CpuIdLibRegisters()
        {
            Initialise();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CpuIdLibRegisters"/> class.
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
        public CpuIdLibRegisters(CpuIdLib.CpuIdInfo[] data, int offset, int length)
        {
            ThrowHelper.ThrowIfArrayOutOfBounds(data, offset, length);
            Initialise(data, offset, length);
        }

        private unsafe void Initialise()
        {
            if (CpuIdLib.hascpuid() == 0)
                throw new PlatformNotSupportedException("CPUID instruction not supported");

            CpuIdLib.CpuIdInfo[] data = new CpuIdLib.CpuIdInfo[MaxCpuLeaves];
            int r;
            fixed (CpuIdLib.CpuIdInfo* cpuidptr = &data[0]) {
                r = CpuIdLib.iddump(cpuidptr, Marshal.SizeOf(data[0]) * data.Length);
            }
            Initialise(data, 0, r);
        }

        private void Initialise(CpuIdLib.CpuIdInfo[] data, int offset, int length)
        {
            for (int i = 0; i < length; i++) {
                int r = offset + i;
                CpuIdRegister result = new(data[r].veax, data[r].vecx,
                    new int[] { data[r].peax, data[r].pebx, data[r].pecx, data[r].pedx });
                m_RegisterList.Add(result);
            }
        }

        public CpuIdRegister GetCpuId(int function, int subfunction)
        {
            _ = CpuIdLib.cpuid(function, subfunction, out int eax, out int ebx, out int ecx, out int edx);
            return new CpuIdRegister(function, subfunction, new int[] { eax, ebx, ecx, edx });
        }

        public bool IsOnline { get { return true; } }

        public IEnumerator<CpuIdRegister> GetEnumerator()
        {
            return m_RegisterList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
