namespace RJCP.Diagnostics.Native
{
    using System;

    internal interface ICpuPin
    {
        IDisposable Pin(int core);
    }
}
