namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class X86CpuIdFactoryXml : X86CpuIdFactoryBase
    {
        public X86CpuIdFactoryXml() { }

        public X86CpuIdFactoryXml(XmlNode node)
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
                ThrowHelper.ThrowIfNull(value);
                m_Node = value;
            }
        }

        public override ICpuId Create()
        {
            if (Node == null) throw new InvalidOperationException("Node is not defined");

            XmlNode cpuNode = Node.SelectSingleNode("./processor");
            return Create(new BasicCpu(cpuNode));
        }

        public override IEnumerable<ICpuId> CreateAll()
        {
            if (Node == null) throw new InvalidOperationException("Node is not defined");

            XmlNodeList cpuNodes = Node.SelectNodes("./processor");
            IEnumerable<BasicCpu> cpus = GetCpuNodes(cpuNodes);
            List<ICpuId> ids = new();
            foreach (BasicCpu cpu in cpus) {
                ids.Add(Create(cpu));
            }
            return ids;
        }

        private static List<BasicCpu> GetCpuNodes(XmlNodeList xmlNodes)
        {
            List<BasicCpu> cpus = new();
            foreach (XmlNode cpuNode in xmlNodes) {
                BasicCpu cpu = new(cpuNode);
                cpus.Add(cpu);
            }

            return cpus;
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
