namespace RJCP.Diagnostics
{
    using System;
    using System.Collections.Generic;
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
                LoadLibrary();
                Intel.X86CpuIdFactory factory = new Intel.X86CpuIdFactory();
                return factory.Create();
            default:
                throw new PlatformNotSupportedException("Architecture is not supported");
            }
        }

        public IEnumerable<ICpuId> CreateAll()
        {
            OSArchitecture architecture = Win32.GetArchitecture();

            switch (architecture) {
            case OSArchitecture.x64:
            case OSArchitecture.x86:
            case OSArchitecture.x86_x64:
                LoadLibrary();
                Intel.X86CpuIdFactory factory = new Intel.X86CpuIdFactory();
                return factory.CreateAll();
            default:
                throw new PlatformNotSupportedException("Architecture is not supported");
            }
        }

        private void LoadLibrary()
        {
            SafeLibraryHandle handle = Win32.LoadLibrary<WindowsCpuIdFactory>("cpuid.dll");
            if (handle.IsInvalid)
                throw new PlatformNotSupportedException("Cannot load platform specific libraries");
        }
    }
}
