namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class CpuRegisters : ICpuRegisters
    {
        private readonly Dictionary<int, List<CpuIdRegister>> m_Registers = new();
        private readonly List<CpuIdRegister> m_RegisterList = new();

        private int m_CurrentFunction;
        private int m_CurrentSubfunction;
        private int m_NextRegisterIndex;
        private List<CpuIdRegister> m_CurrentRegisterList;

        private readonly ICpuRegisters m_CpuRegisters;

        public CpuRegisters(ICpuRegisters cpuRegisters)
        {
            ThrowHelper.ThrowIfNull(cpuRegisters);
            m_CpuRegisters = cpuRegisters;

            foreach (CpuIdRegister cpuRegister in cpuRegisters) {
                AddRegister(cpuRegister);
            }
        }

        public CpuIdRegister GetCpuId(int function, int subfunction)
        {
            CpuIdRegister cpuRegister = GetCachedCpuId(function, subfunction);
            if (cpuRegister is not null) return cpuRegister;

            // Not cached, so query the concrete implementation.
            cpuRegister = m_CpuRegisters.GetCpuId(function, subfunction);
            if (cpuRegister is not null) {
                AddRegister(cpuRegister);
                return cpuRegister;
            }

            return null;
        }

        private CpuIdRegister GetCachedCpuId(int function, int subfunction)
        {
            if (m_CurrentFunction == function && m_CurrentRegisterList is not null) {
                if (m_CurrentSubfunction == subfunction)
                    return GetNextSubfunction(subfunction);
            } else {
                m_CurrentRegisterList = null;
                if (!m_Registers.TryGetValue(function, out m_CurrentRegisterList)) {
                    return null;
                }
                m_CurrentFunction = function;
            }

            m_NextRegisterIndex = 0;
            m_CurrentSubfunction = subfunction;
            return GetNextSubfunction(subfunction);
        }

        private CpuIdRegister GetNextSubfunction(int subfunction)
        {
            CpuIdRegister result;
            int lastRegisterIndex = m_NextRegisterIndex;
            do {
                if (m_NextRegisterIndex == m_CurrentRegisterList.Count) {
                    if (lastRegisterIndex == 0) return null;
                    m_NextRegisterIndex = 0;
                }
                result = m_CurrentRegisterList[m_NextRegisterIndex];
                m_NextRegisterIndex++;

                if (result.SubFunction == subfunction) return result;
                if (lastRegisterIndex == m_NextRegisterIndex) return null;
            } while (true);
        }

        private void AddRegister(CpuIdRegister result)
        {
            ThrowHelper.ThrowIfNull(result);

            if (!m_Registers.TryGetValue(result.Function, out List<CpuIdRegister> registers)) {
                registers = new List<CpuIdRegister>();
                m_Registers.Add(result.Function, registers);
            }

            registers.Add(result);
            m_RegisterList.Add(result);
        }

        public bool IsOnline { get { return m_CpuRegisters.IsOnline; } }

        public IEnumerator<CpuIdRegister> GetEnumerator()
        {
            return m_RegisterList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
