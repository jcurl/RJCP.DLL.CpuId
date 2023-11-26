namespace RJCP.Diagnostics.CpuId.Intel.InternalCheck
{
    using System.Collections.Generic;

    internal sealed class MainFunction
    {
        private readonly Dictionary<int, SubFunction> m_Function = new Dictionary<int, SubFunction>();

        public bool Set(int function, int subFunction, int register, int mask)
        {
            if (!m_Function.TryGetValue(function, out SubFunction subFunctionMap)) {
                subFunctionMap = new SubFunction();
                m_Function.Add(function, subFunctionMap);
            }
            return subFunctionMap.Set(subFunction, register, mask);
        }
    }
}
