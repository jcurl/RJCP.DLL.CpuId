namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Runtime.Intrinsics.X86;
    using System.Runtime.Versioning;
    using Native.Win32;
#if NETCOREAPP
    using System.Runtime.InteropServices;
    using Native.Linux;
#endif

    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("Linux")]
    internal class CpuIdNetRegisters : ICpuRegisters//, IEnumerable<CpuIdRegister>
    {
        internal const int MaxCpuLeaves = 256;
        private readonly int m_Core = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="CpuIdNetRegisters"/> class.
        /// </summary>
        /// <remarks>
        /// Allows capturing register data for the current core. Be sure to pin to the correct thread first.
        /// </remarks>
        public CpuIdNetRegisters()
        {
            if (!X86Base.IsSupported)
                throw new PlatformNotSupportedException("X86Base not supported");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CpuIdNetRegisters"/> class.
        /// </summary>
        /// <param name="core">The core to pin to.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="core"/> is not a valid core number.</exception>
        /// <remarks>Queries the local machine for CPU data.</remarks>
        public CpuIdNetRegisters(int core)
        {
            if (!X86Base.IsSupported)
                throw new PlatformNotSupportedException("X86Base not supported");

            if (core < 0 || core >= Environment.ProcessorCount)
                throw new ArgumentOutOfRangeException(nameof(core));

            m_Core = core;
        }

        public CpuIdRegister GetCpuId(int function, int subfunction)
        {
            using (Pin(m_Core)) {
                var (Eax, Ebx, Ecx, Edx) = X86Base.CpuId(function, subfunction);
                return new CpuIdRegister(function, subfunction, new[] { Eax, Ebx, Ecx, Edx });
            }
        }

        internal static IDisposable Pin(int core)
        {
            // Special case that we don't pin a core.
            if (core < 0) return null;

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
    }
}
