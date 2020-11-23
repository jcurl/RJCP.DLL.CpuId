namespace RJCP.Diagnostics.Native
{
    using Microsoft.Win32.SafeHandles;
    using static Kernel32;

    internal class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeLibraryHandle() : base(true) { }

        protected override bool ReleaseHandle()
        {
            return FreeLibrary(handle);
        }
    }
}
