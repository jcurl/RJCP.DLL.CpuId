namespace RJCP.Diagnostics.CpuId.Intel.InternalCheck
{
    using System.Collections.Generic;

    internal sealed class SubFunction
    {
        private readonly Dictionary<int, BitMask> m_SubFunction = new Dictionary<int, BitMask>();

        public bool Set(int subFunction, int register, int mask)
        {
            if (!m_SubFunction.TryGetValue(subFunction, out BitMask bitMask)) {
                bitMask = new BitMask();
                m_SubFunction.Add(subFunction, bitMask);
            }
            return bitMask.Set(register, mask);
        }
    }
}
