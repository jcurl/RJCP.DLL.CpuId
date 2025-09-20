namespace RJCP.Diagnostics.Native.Win32
{
    using System;
    using System.Runtime.Versioning;
    using System.Threading;

    [SupportedOSPlatform("windows")]
    internal class PinCpuWin32 : ICpuPin
    {
        private sealed class Release : IDisposable
        {
            private readonly nuint m_Affinity;
            private readonly int m_ThreadId;

            public Release(nuint affinity)
            {
                m_ThreadId = Kernel32.GetCurrentThreadId();
                m_Affinity = affinity;
            }

            public void Dispose()
            {
                int threadid = Kernel32.GetCurrentThreadId();
                if (threadid != m_ThreadId) return;

                // We're restoring the affinity, so we don't care what the original value was.
                _ = Kernel32.SetThreadAffinityMask(Kernel32.GetCurrentThread(), m_Affinity);
                Thread.EndThreadAffinity();
            }
        }

        public IDisposable Pin(int core)
        {
            if (core < 0 || core >= IntPtr.Size * 8 || core >= Environment.ProcessorCount)
                throw new ArgumentOutOfRangeException(nameof(core), core, "Core outside of accessible number of CPUs");

            nuint pin = (nuint)1 << core;

            Thread.BeginThreadAffinity();
            nuint oldAffinity = Kernel32.SetThreadAffinityMask(Kernel32.GetCurrentThread(), pin);
            if (oldAffinity == 0) {
                Thread.EndThreadAffinity();
                throw new InvalidOperationException($"Couldn't pin CPU core {core}");
            }
            return new Release(oldAffinity);
        }
    }
}
