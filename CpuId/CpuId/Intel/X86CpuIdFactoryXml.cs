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
            if (node == null) throw new ArgumentNullException(nameof(node));
            Node = node;
        }

        private XmlNode m_Node;

        public XmlNode Node
        {
            get { return m_Node; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
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
            List<ICpuId> ids = new List<ICpuId>();
            foreach (BasicCpu cpu in cpus) {
                ids.Add(Create(cpu));
            }
            return ids;
        }

        private IEnumerable<BasicCpu> GetCpuNodes(XmlNodeList xmlNodes)
        {
            List<BasicCpu> cpus = new List<BasicCpu>();
            foreach (XmlNode cpuNode in xmlNodes) {
                BasicCpu cpu = new BasicCpu(cpuNode);
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
            if (xmlWriter == null) throw new ArgumentNullException(nameof(xmlWriter));
            if (cpus == null) throw new ArgumentNullException(nameof(cpus));

            xmlWriter.WriteStartElement("cpuid");
            xmlWriter.WriteAttributeString("type", "x86");
            foreach (GenericIntelCpuBase cpu in cpus) {
                cpu.Save(xmlWriter);
            }
            xmlWriter.WriteEndElement();
        }
    }
}
