namespace RJCP.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;
    using CpuId;

    /// <summary>
    /// Factory for getting a class with information about the CPU recorded to an XML file.
    /// </summary>
    public class CpuIdXmlFactory : ICpuIdFactory
    {
        // This class is responsible for reading the XML file and passing all core nodes to the factory for the specific
        // CPU. Currently implemented is only 'CpuIdX86XmlNodeFactory'.

        /// <summary>
        /// The default constructor, with no file name defined.
        /// </summary>
        /// <remarks>
        /// This is the default constructor. No file name is defined. One must set the file name property
        /// <see cref="FileName"/>.
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
            ThrowHelper.ThrowIfNullOrEmpty(fileName);
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
                ThrowHelper.ThrowIfNullOrEmpty(value, nameof(FileName));
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
            if (string.IsNullOrEmpty(FileName)) throw new InvalidOperationException("FileName is empty");

            XmlNode cpuIdNode = GetCpuIdNode(FileName);
            if (cpuIdNode is null) return null;
            string processor = cpuIdNode.Attributes["type"]?.Value ?? "x86";

            switch (processor) {
            case "x86":
                CpuId.Intel.CpuIdX86XmlNodeFactory x86Factory = new(cpuIdNode);
                return x86Factory.Create();
            default:
                // This processor type is unknown.
                return null;
            }
        }

        /// <summary>
        /// Retrieve information about the CPU for a specific core.
        /// </summary>
        /// <param name="core">The core.</param>
        /// <returns>CPU information.</returns>
        /// <exception cref="InvalidOperationException">
        /// <see cref="FileName"/> is <see langword="null"/>.
        /// <para>- or -</para>
        /// <see cref="FileName"/> is empty.
        /// </exception>
        public ICpuId Create(int core)
        {
            if (FileName is null) throw new InvalidOperationException("FileName is null");
            if (string.IsNullOrEmpty(FileName)) throw new InvalidOperationException("FileName is empty");

            XmlNode cpuIdNode = GetCpuIdNode(FileName);
            if (cpuIdNode is null) return null;
            string processor = cpuIdNode.Attributes["type"]?.Value ?? "x86";

            switch (processor) {
            case "x86":
                CpuId.Intel.CpuIdX86XmlNodeFactory x86Factory = new(cpuIdNode);
                return x86Factory.Create(core);
            default:
                // This processor type is unknown.
                return null;
            }
        }

        private static XmlNode GetCpuIdNode(string fileName)
        {
            ThrowHelper.ThrowIfNullOrEmpty(fileName);

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
        public static IEnumerable<ICpuId> CreateAll(string fileName)
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
                CpuId.Intel.CpuIdX86XmlNodeFactory x86Factory = new(cpuIdNode);
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
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> may not be an empty string.</exception>
        /// <exception cref="NotSupportedException">
        /// .NET Framework and .NET Core versions older than 2.1: <paramref name="fileName"/> is an empty string (""),
        /// contains only white space, or contains one or more invalid characters.
        /// <para>- or -</para>
        /// <paramref name="fileName"/> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in an NTFS
        /// environment.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="fileName"/> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a
        /// non-NTFS environment.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        /// <paramref name="fileName"/> is invalid, such as being on an unmapped drive.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">The file or directory is set for read-only access.</exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, <paramref name="fileName"/>, or both exceed the system-defined maximum length.
        /// </exception>
        public static void Save(string fileName, IEnumerable<ICpuId> cpus)
        {
            ThrowHelper.ThrowIfNullOrEmpty(fileName);

            List<CpuId.Intel.GenericIntelCpuBase> x86cpus = new();
            foreach (ICpuId cpu in cpus) {
                if (cpu is CpuId.Intel.GenericIntelCpuBase x86cpu) {
                    x86cpus.Add(x86cpu);
                }
            }

            using (XmlWriter xmlWriter = XmlWriter.Create(fileName, SaveXmlSettings())) {
                if (x86cpus.Count > 0) CpuId.Intel.CpuIdX86XmlNodeFactory.Save(xmlWriter, x86cpus);
            }
        }

        /// <summary>
        /// Writes the cached CPUID information to an XML file.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="cpus">The collection of CPUs that should be written to the XML writer.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> may not be <see langword="null"/>.
        /// </exception>
        public static void Save(Stream stream, IEnumerable<ICpuId> cpus)
        {
            ThrowHelper.ThrowIfNull(stream);

            List<CpuId.Intel.GenericIntelCpuBase> x86cpus = new();
            foreach (ICpuId cpu in cpus) {
                if (cpu is CpuId.Intel.GenericIntelCpuBase x86cpu) {
                    x86cpus.Add(x86cpu);
                }
            }

            using (XmlWriter xmlWriter = XmlWriter.Create(stream, SaveXmlSettings())) {
                if (x86cpus.Count > 0) CpuId.Intel.CpuIdX86XmlNodeFactory.Save(xmlWriter, x86cpus);
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
