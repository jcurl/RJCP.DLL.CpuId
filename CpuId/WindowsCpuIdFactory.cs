namespace RJCP.Diagnostics
{
    using System;
    using Native;

    internal class WindowsCpuIdFactory : ICpuIdFactory
    {
        public ICpuId Create()
        {
            OSArchitecture architecture = Win32.GetArchitecture();

            switch (architecture) {
            case OSArchitecture.x64:
            case OSArchitecture.x86:
            case OSArchitecture.x86_x64:
                return LoadLibraryAndCreate();
            default:
                throw new PlatformNotSupportedException("Architecture is not supported");
            }
        }

        private ICpuId LoadLibraryAndCreate()
        {
            SafeLibraryHandle handle = Win32.LoadLibrary<WindowsCpuIdFactory>("cpuid.dll");
            if (handle.IsInvalid)
                throw new PlatformNotSupportedException("Cannot load platform specific libraries");

            Intel.X86CpuIdFactory factory = new Intel.X86CpuIdFactory();
            return factory.Create();
        }
    }
}
