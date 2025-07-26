namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml;

    internal class CpuIdX86XmlNodeFactory : ICpuIdFactory
    {
        public CpuIdX86XmlNodeFactory(XmlNode node)
        {
            ThrowHelper.ThrowIfNull(node);
            Node = node;
        }

        private XmlNode m_Node;

        public XmlNode Node
        {
            get { return m_Node; }
            set
            {
                ThrowHelper.ThrowIfNull(value, nameof(Node));
                m_Node = value;
            }
        }

        /// <summary>
        /// Retrieve information about the first CPU.
        /// </summary>
        /// <returns>CPU information for the first entry in the XML file.</returns>
        /// <exception cref="InvalidOperationException">Node is not defined.</exception>
        /// <remarks>
        /// Because this factory is given an <see cref="XmlNode"/> during construction, and this should only be a single
        /// core, this is considered the first core. If this is really the first core of a multiprocessor system or not,
        /// is defined by the factory that creates this factory.
        /// </remarks>
        public ICpuId Create()
        {
            return Create(0);
        }

        /// <summary>
        /// Retrieve information about the CPU for a specific core.
        /// </summary>
        /// <param name="core">The core.</param>
        /// <returns>CPU information only for <paramref name="core"/>, defined by the position in the XML file.</returns>
        /// <remarks>
        /// This method can only return the same information as <see cref="Create()"/>. The constructor of this class
        /// receives the precise core information. The factory that constructs this class must handle the creation for a
        /// specific core.
        /// </remarks>
        public ICpuId Create(int core)
        {
            if (Node is null) throw new InvalidOperationException("Node is not defined");

            XmlNode cpuNode = Node.SelectSingleNode($"./processor[{core + 1}]");
            if (cpuNode is null) return null;
            return Create(cpuNode);
        }

        /// <summary>
        /// Retrieves information about all CPUs detected by the Operating System.
        /// </summary>
        /// <returns>An enumerable collection of all CPUs.</returns>
        /// <exception cref="InvalidOperationException">Node is not defined</exception>
        public IEnumerable<ICpuId> CreateAll()
        {
            if (Node is null) throw new InvalidOperationException("Node is not defined");

            XmlNodeList cpuNodes = Node.SelectNodes("./processor");

            List<ICpuId> ids = new();
            foreach (XmlNode cpuNode in cpuNodes) {
                ids.Add(Create(cpuNode));
            }
            return ids;
        }

        private static ICpuId Create(XmlNode cpuNode)
        {
            List<CpuIdRegister> registerList = new();
            XmlNodeList registers = cpuNode.SelectNodes("./register");
            foreach (XmlNode register in registers) {
                AddCpuRegister(registerList, register);
            }
            return CpuIdX86.CreateCpuIdX86(registerList);
        }

        private static void AddCpuRegister(ICollection<CpuIdRegister> list, XmlNode registerNode)
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
            list.Add(result);
        }

        private static bool TryGetHexValue(string value, out int result)
        {
            return int.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Writes the cached CPUID registers (those found in <see cref="ICpuIdX86.Registers" /> to an XML file.
        /// </summary>
        /// <param name="xmlWriter">The XML writer to write the output to.</param>
        /// <param name="cpus">The collection of CPUs that should be written to the XML writer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="xmlWriter"/> may not be <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="cpus"/> may not be <see langword="null"/>.</exception>
        public static void Save(XmlWriter xmlWriter, IEnumerable<GenericIntelCpuBase> cpus)
        {
            ThrowHelper.ThrowIfNull(xmlWriter);
            ThrowHelper.ThrowIfNull(cpus);

            xmlWriter.WriteStartElement("cpuid");
            xmlWriter.WriteAttributeString("type", "x86");
            foreach (GenericIntelCpuBase cpu in cpus) {
                cpu.Save(xmlWriter);
            }
            xmlWriter.WriteEndElement();
        }
    }
}
