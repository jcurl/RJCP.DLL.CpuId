namespace RJCP.Diagnostics.CpuId
{
    using System;
    using System.Collections.Generic;
    using Intel;
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
                X86CpuIdFactory factory = new X86CpuIdFactory();
                ICpuId cpu = factory.Create();
                if (cpu is ICpuIdX86 x86cpu) {
                    x86cpu.Topology.CoreTopology.IsReadOnly = true;
                }
                return cpu;
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
                X86CpuIdFactory factory = new X86CpuIdFactory();
                IEnumerable<ICpuId> cpus = factory.CreateAll();
                foreach (ICpuId cpu in cpus) {
                    if (cpu is ICpuIdX86 x86cpu) {
                        x86cpu.Topology.CoreTopology.IsReadOnly = true;
                    }
                }
                return cpus;
            default:
                throw new PlatformNotSupportedException("Architecture is not supported");
            }
        }

        private static void LoadLibrary()
        {
            SafeLibraryHandle handle = Win32.LoadLibrary<WindowsCpuIdFactory>("cpuid.dll");
            if (handle.IsInvalid)
                throw new PlatformNotSupportedException("Cannot load platform specific libraries");
        }
    }
}
