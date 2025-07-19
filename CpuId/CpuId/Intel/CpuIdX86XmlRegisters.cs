namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml;

    internal class CpuIdX86XmlRegisters : ICpuRegisters
    {
        private readonly List<CpuIdRegister> m_RegisterList = new();

        public CpuIdX86XmlRegisters(XmlNode node)
        {
            XmlNodeList registers = node.SelectNodes("./register");
            foreach (XmlNode register in registers) {
                AddCpuRegister(register);
            }
        }

        private void AddCpuRegister(XmlNode registerNode)
        {
            if (!TryGetHexValue(registerNode.Attributes["eax"]?.Value, out int function)) return;
            if (!TryGetHexValue(registerNode.Attributes["ecx"]?.Value, out int subfunction)) return;
            string registerOutput = registerNode.InnerText;
            if (string.IsNullOrWhiteSpace(registerOutput)) return;
            string[] registerValues = registerOutput.Split(',');
            if (registerValues.Length != 4) return;

            int[] registers = new int[4];
            int i = 0;
            foreach (string registerValue in registerValues) {
                if (!TryGetHexValue(registerValue, out registers[i])) return;
                i++;
            }

            CpuIdRegister result = new(function, subfunction, registers);
            m_RegisterList.Add(result);
        }

        private static bool TryGetHexValue(string value, out int result)
        {
            return int.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
        }

        public CpuIdRegister GetCpuId(int function, int subfunction)
        {
            // Wrap this class around a CpuIdRegisters for proper function.
            return null;
        }

        public bool IsOnline { get { return false; } }

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
