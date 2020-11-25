namespace RJCP.Diagnostics.Intel
{
    using System.Xml;
    using System.Globalization;

    internal class CpuXmlRegisters : CpuRegistersBase
    {
        public CpuXmlRegisters(XmlNode node)
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

            CpuIdRegister result = new CpuIdRegister(function, subfunction, registers);
            AddRegister(result);
        }

        private bool TryGetHexValue(string value, out int result)
        {
            return int.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
        }
    }
}
