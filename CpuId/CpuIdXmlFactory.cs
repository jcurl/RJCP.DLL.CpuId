namespace RJCP.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using CpuId;

    /// <summary>
    /// Factory for getting a class with information about the CPU recorded to an XML file.
    /// </summary>
    public class CpuIdXmlFactory : ICpuIdFactory
    {
        /// <summary>
        /// The default constructor, with no file name defined.
        /// </summary>
        /// <remarks>
        /// This is the default constructor. No file name is defined. One must set the file name property
        /// <see cref="FileName"/>, or use <see cref="Create(string)"/> to load from a file.
        /// </remarks>
        public CpuIdXmlFactory()
        {
            m_FileName = string.Empty;
        }

        /// <summary>
        /// The constructor which uses the file name given on <see cref="Create()"/>
        /// </summary>
        /// <param name="fileName">Name of the file that should be loaded when <see cref="Create()"/> is called.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> may not be an empty string.</exception>
        public CpuIdXmlFactory(string fileName)
        {
            ThrowHelper.ThrowIfNull(fileName);
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("FileName may not be an empty string", nameof(fileName));

            m_FileName = fileName;
        }

        private string m_FileName;

        /// <summary>
        /// Gets or sets the name of the file thta should be opened when <see cref="Create()"/> is called.
        /// </summary>
        /// <value>The name of the file.</value>
        /// <exception cref="ArgumentNullException">The value may not be <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The value may not be an empty string.</exception>
        public string FileName
        {
            get { return m_FileName; }
            set
            {
                ThrowHelper.ThrowIfNull(value);
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("FileName may not be an empty string", nameof(value));
                m_FileName = value;
            }
        }

        /// <summary>
        /// Retrieve information about a CPU using the file name in the property <see cref="FileName"/>.
        /// </summary>
        /// <returns>CPU information.</returns>
        /// <exception cref="InvalidOperationException">
        /// <see cref="FileName"/> is <see langword="null"/>.
        /// <para>- or -</para>
        /// <see cref="FileName"/> is empty.
        /// </exception>
        public ICpuId Create()
        {
            if (FileName is null) throw new InvalidOperationException("FileName is null");
            return string.IsNullOrEmpty(FileName) ? throw new InvalidOperationException("File name is empty") : Create(FileName);
        }

        /// <summary>
        /// Retrieves information about a CPU using the file name given as the parameter.
        /// </summary>
        /// <param name="fileName">Name of the file to load.</param>
        /// <returns>CPU information.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> may not be an empty string.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Factory Method")]
        public ICpuId Create(string fileName)
        {
            XmlNode cpuIdNode = GetCpuIdNode(fileName);
            if (cpuIdNode is null) return null;
            string processor = cpuIdNode.Attributes["type"]?.Value ?? "x86";

            switch (processor) {
            case "x86":
                CpuId.Intel.X86CpuIdFactoryXml x86Factory = new(cpuIdNode);
                return x86Factory.Create();
            default:
                // This processor type is unknown.
                return null;
            }
        }

        private static XmlNode GetCpuIdNode(string fileName)
        {
            ThrowHelper.ThrowIfNull(fileName);
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("File name is empty", nameof(fileName));

            XmlDocument xmlDoc = new() {
                XmlResolver = null
            };
            xmlDoc.Load(fileName);

            return xmlDoc.SelectSingleNode("/cpuid");
        }

        /// <summary>
        /// Retrieves information about all CPUs using the file name in the property <see cref="FileName"/>.
        /// </summary>
        /// <returns>An enumerable collection of all CPUs.</returns>
        /// <exception cref="InvalidOperationException">
        /// <see cref="FileName"/> is <see langword="null"/>
        /// <para>- or -</para>
        /// <see cref="FileName"/> is empty.
        /// </exception>
        public IEnumerable<ICpuId> CreateAll()
        {
            if (FileName is null) throw new InvalidOperationException("FileName is null");
            return string.IsNullOrEmpty(FileName) ? throw new InvalidOperationException("File name is empty") : CreateAll(FileName);
        }

        /// <summary>
        /// Retrieves information about CPUs using the file name given as the parameter.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>An enumerable collection of all CPUs.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Factory method")]
        public IEnumerable<ICpuId> CreateAll(string fileName)
        {
            XmlNode cpuIdNode = GetCpuIdNode(fileName);
            if (cpuIdNode is null)
#if NET40
                return new ICpuId[0];
#else
                return Array.Empty<ICpuId>();
#endif
            string processor = cpuIdNode.Attributes["type"]?.Value ?? "x86";

            switch (processor) {
            case "x86":
                CpuId.Intel.X86CpuIdFactoryXml x86Factory = new(cpuIdNode);
                return x86Factory.CreateAll();
            default:
                // This processor type is unknown.
#if NET40
                return new ICpuId[0];
#else
                return Array.Empty<ICpuId>();
#endif
            }
        }

        /// <summary>
        /// Writes the cached CPUID information to an XML file.
        /// </summary>
        /// <param name="fileName">Name of the file to write to.</param>
        /// <param name="cpus">The collection of CPUs that should be written to the XML writer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fileName" /> may not be <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName" /> may not be an empty string.</exception>
        public static void Save(string fileName, IEnumerable<ICpuId> cpus)
        {
            ThrowHelper.ThrowIfNull(fileName);
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("File name may not be empty", nameof(fileName));

            List<CpuId.Intel.GenericIntelCpuBase> x86cpus = new();
            foreach (ICpuId cpu in cpus) {
                if (cpu is CpuId.Intel.GenericIntelCpuBase x86cpu) {
                    x86cpus.Add(x86cpu);
                }
            }

            using (XmlWriter xmlWriter = XmlWriter.Create(fileName, SaveXmlSettings())) {
                if (x86cpus.Count > 0) CpuId.Intel.X86CpuIdFactoryXml.Save(xmlWriter, x86cpus);
            }
        }

        private static XmlWriterSettings SaveXmlSettings()
        {
            return new XmlWriterSettings {
                CloseOutput = true,
                ConformanceLevel = ConformanceLevel.Document,
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = false
            };
        }
    }
}
