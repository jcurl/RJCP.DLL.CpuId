namespace RJCP.Diagnostics.CpuId.Intel.InternalCheck
{
    using System.Collections.Generic;

    internal sealed class BitMask
    {
        private readonly Dictionary<int, int> m_RegisterMask = new();

        public bool Set(int register, int mask)
        {
            if (!m_RegisterMask.TryGetValue(register, out int currentMask)) {
                m_RegisterMask.Add(register, currentMask);
            }
            if ((currentMask & mask) != 0) return false;
            currentMask |= mask;
            m_RegisterMask[register] = currentMask;
            return true;
        }
    }
}
