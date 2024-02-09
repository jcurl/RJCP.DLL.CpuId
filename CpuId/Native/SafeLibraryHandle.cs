namespace RJCP.Diagnostics.Native
{
    using Microsoft.Win32.SafeHandles;
    using static Kernel32;
#if NETFRAMEWORK
    using System.Runtime.ConstrainedExecution;
#endif

    internal class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        protected SafeLibraryHandle() : base(true) { }

#if NETFRAMEWORK
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        protected override bool ReleaseHandle()
        {
            return FreeLibrary(handle);
        }
    }
}
