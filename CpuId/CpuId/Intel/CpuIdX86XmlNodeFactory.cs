namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class CpuIdX86XmlNodeFactory : ICpuIdFactory
    {
        public CpuIdX86XmlNodeFactory() { }

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

        public ICpuId Create()
        {
            if (Node is null) throw new InvalidOperationException("Node is not defined");

            XmlNode cpuNode = Node.SelectSingleNode("./processor");
            return CpuIdX86.CreateCpuIdX86(new CpuIdX86XmlRegisters(cpuNode));
        }

        public IEnumerable<ICpuId> CreateAll()
        {
            if (Node is null) throw new InvalidOperationException("Node is not defined");

            XmlNodeList cpuNodes = Node.SelectNodes("./processor");

            List<ICpuId> ids = new();
            foreach (XmlNode cpuNode in cpuNodes) {
                ids.Add(CpuIdX86.CreateCpuIdX86(new CpuIdX86XmlRegisters(cpuNode)));
            }
            return ids;
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
