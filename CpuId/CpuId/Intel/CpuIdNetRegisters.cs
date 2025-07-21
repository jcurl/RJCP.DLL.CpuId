namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Intrinsics.X86;
    using System.Runtime.Versioning;
    using Native.Win32;
#if NETCOREAPP
    using System.Runtime.InteropServices;
    using Native.Linux;
#endif

    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("Linux")]
    internal class CpuIdNetRegisters : ICpuRegisters
    {
        internal const int MaxCpuLeaves = 256;

        private readonly int m_Core = 0;
        private readonly List<CpuIdRegister> m_RegisterList = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CpuIdNetRegisters"/> class.
        /// </summary>
        /// <remarks>Queries the local machine for CPU data.</remarks>
        public CpuIdNetRegisters()
        {
            CheckPlatform();
            Initialise();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CpuIdNetRegisters"/> class.
        /// </summary>
        /// <param name="core">The core to query for the CPUID registers.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="core"/> is not a valid core number.</exception>
        /// <remarks>Queries the local machine for CPU data.</remarks>
        public CpuIdNetRegisters(int core)
        {
            CheckPlatform();
            if (core < 0 || core >= Environment.ProcessorCount)
                throw new ArgumentOutOfRangeException(nameof(core));

            m_Core = core;
            Initialise();
        }

        private static void CheckPlatform()
        {
            if (!X86Base.IsSupported)
                throw new PlatformNotSupportedException("X86Base not supported");
        }

        private void Initialise()
        {
            // TODO: Enumerate all values and pass to Initialise(registers). The enumeration should be done in the appropriate
            // class and add to m_RegisterList.
        }

        public CpuIdRegister GetCpuId(int function, int subfunction)
        {
            using (Pin(m_Core)) {
                var (Eax, Ebx, Ecx, Edx) = X86Base.CpuId(function, subfunction);
                return new CpuIdRegister(function, subfunction, new[] { Eax, Ebx, Ecx, Edx });
            }
        }

        private static IDisposable Pin(int core)
        {
            Native.ICpuPin pin;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                pin = new PinCpuWin32();
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                pin = new PinCpuLinux();
            } else {
                throw new PlatformNotSupportedException("Unknown platform");
            }
            return pin.Pin(core);
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
