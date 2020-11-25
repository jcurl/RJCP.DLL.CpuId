namespace RJCP.Diagnostics.Intel
{
    using System;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// The base abstract class for which all 80x86 CPU information implementations should derive.
    /// </summary>
    public abstract class GenericIntelCpuBase : ICpuIdX86
    {
        private readonly BasicCpu m_Cpu;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3442:\"abstract\" classes should not have \"public\" constructors", Justification = "User code shouldn't instantiate this")]
        internal GenericIntelCpuBase(BasicCpu cpu)
        {
            m_Cpu = cpu;
            Features = new CpuFeatures();
        }

        /// <inheritdoc/>
        public virtual CpuVendor CpuVendor
        {
            get { return CpuVendor.Unknown; }
        }

        /// <inheritdoc/>
        public string VendorId
        {
            get { return m_Cpu.VendorId; }
        }

        /// <inheritdoc/>
        public ICpuRegisters Registers
        {
            get { return m_Cpu.CpuRegisters; }
        }

        /// <inheritdoc/>
        public string Description { get; protected set; }

        /// <inheritdoc/>
        public int ProcessorSignature { get; protected set; }

        /// <inheritdoc/>
        public int Family { get; protected set; }

        /// <inheritdoc/>
        public int Model { get; protected set; }

        /// <inheritdoc/>
        public int Stepping { get; protected set; }

        /// <inheritdoc/>
        public int ProcessorType { get; protected set; }

        /// <inheritdoc/>
        public CpuFeatures Features { get; private set; }

        /// <summary>
        /// Writes the cached CPUID registers (those found in <see cref="Registers" /> to an XML file.
        /// </summary>
        /// <param name="fileName">Name of the file to write to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> may not be <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> may not be an empty string.</exception>
        public void Save(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("File name may not be empty", nameof(fileName));
            using (XmlWriter xmlWriter = XmlWriter.Create(fileName, SaveXmlSettings())) {
                xmlWriter.WriteStartElement("cpuid");
                Save(xmlWriter);
                xmlWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Writes the cached CPUID registers (those found in <see cref="Registers"/> to an XML writer.
        /// </summary>
        /// <param name="xmlWriter">The XML writer to write to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="xmlWriter"/> may not be <see langword="null"/>.</exception>
        public void Save(XmlWriter xmlWriter)
        {
            if (xmlWriter == null) throw new ArgumentNullException(nameof(xmlWriter));
            xmlWriter.WriteStartElement("processor");
            xmlWriter.WriteAttributeString("type", "x86");
            WriteRegisters(xmlWriter);
            xmlWriter.WriteEndElement();
        }

        private XmlWriterSettings SaveXmlSettings()
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings {
                CloseOutput = true,
                ConformanceLevel = ConformanceLevel.Document,
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = false
            };
            return xmlSettings;
        }

        private void WriteRegisters(XmlWriter xmlWriter)
        {
            foreach (CpuIdRegister register in Registers.Registers) {
                xmlWriter.WriteStartElement("register");
                xmlWriter.WriteAttributeString("eax", register.Function.ToString("X8"));
                xmlWriter.WriteAttributeString("ecx", register.SubFunction.ToString("X8"));
                string value = string.Format("{0:X8},{1:X8},{2:X8},{3:X8}",
                    register.Result[0], register.Result[1], register.Result[2], register.Result[3]);
                xmlWriter.WriteString(value);
                xmlWriter.WriteEndElement();
            }
        }
    }
}
