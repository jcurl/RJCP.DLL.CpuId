namespace RJCP.Diagnostics.Native
{
    using System;
    using System.IO;
    using System.Runtime.Versioning;
    using static Kernel32;

    [SupportedOSPlatform("windows")]
    internal static class Win32
    {
        public static SafeLibraryHandle LoadLibrary<T>(string fileName)
        {
            Uri assemblyLocation = new(typeof(T).Assembly.Location);
            string libraryPath = Path.GetDirectoryName(assemblyLocation.LocalPath);

            libraryPath = !Environment.Is64BitProcess ? Path.Combine(libraryPath, "x86", fileName) : Path.Combine(libraryPath, "x64", fileName);

            return LoadLibraryEx(libraryPath, IntPtr.Zero, LoadLibraryFlags.None);
        }

        public static OSArchitecture GetArchitecture()
        {
            OSArchitecture architecture;
            bool nativeSystemInfo;
            SYSTEM_INFO lpSystemInfo;

            // GetNativeSystemInfo is independent if we're 64-bit or not But it needs _WIN32_WINNT 0x0501
            try {
                GetNativeSystemInfo(out lpSystemInfo);
                architecture = lpSystemInfo.uProcessorInfo.wProcessorArchitecture;
                nativeSystemInfo = true;
            } catch {
                architecture = OSArchitecture.Unknown;
                nativeSystemInfo = false;
            }

            if (architecture == OSArchitecture.Unknown || !nativeSystemInfo) {
                try {
                    GetSystemInfo(out lpSystemInfo);
                    architecture = lpSystemInfo.uProcessorInfo.wProcessorArchitecture;
                } catch {
                    architecture = OSArchitecture.Unknown;
                }
            }

            // We try to determine if we're a WOW64 process if we don't know the architecture or if we're x86 and
            // NativeSystemInfo didn't work.
            switch (architecture) {
            case OSArchitecture.x64:
            case OSArchitecture.x86:
            case OSArchitecture.x86_x64:
                if (IsWow64())
                    architecture = OSArchitecture.x86_x64;
                break;
            }

            return architecture;
        }

        private static bool IsWow64()
        {
            try {
                return IsWow64Process(GetCurrentProcess(), out bool wow64) && wow64;
            } catch (EntryPointNotFoundException) {
                return false;
            }
        }
    }
}
