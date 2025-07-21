namespace RJCP.Diagnostics.Native.Linux
{
    using System;
    using System.Runtime.Versioning;
    using System.Threading;

    [SupportedOSPlatform("Linux")]
    internal class PinCpuLinux : ICpuPin
    {
        private sealed class Release : IDisposable
        {
            private readonly Glibc.cpu_set_t m_Affinity;

            public Release(Glibc.cpu_set_t affinity)
            {
                m_Affinity = affinity;
            }

            public void Dispose()
            {
                // We're restoring the affinity, so we don't care what the original value was.
                _ = Glibc.SetThreadAffinity(m_Affinity);
                Thread.EndThreadAffinity();
            }
        }

        public IDisposable Pin(int core)
        {
            if (core < 0 || core >= IntPtr.Size * 8 || core >= Environment.ProcessorCount)
                throw new ArgumentOutOfRangeException(nameof(core), core, "Core outside of accessible number of CPUs");

            Glibc.GetThreadAffinity(out Glibc.cpu_set_t orig_set);
            Thread.BeginThreadAffinity();
            if (!Glibc.SetThreadAffinity(core)) {
                Thread.EndThreadAffinity();
                throw new InvalidOperationException($"Couldn't pin CPU core {core}");
            }

            return new Release(orig_set);
        }
    }
}
