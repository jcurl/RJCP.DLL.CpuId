namespace RJCP.Diagnostics
{
    using System;
    using System.Xml;

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
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("FileName may not be an empty string", nameof(fileName));
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
                if (value == null) throw new ArgumentNullException(nameof(value));
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
            if (FileName == null) throw new InvalidOperationException("FileName is null");
            if (string.IsNullOrEmpty(FileName)) throw new InvalidOperationException("File name is empty");

            return Create(FileName);
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
        public ICpuId Create(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("File name is empty", nameof(fileName));

            XmlDocument xmlDoc = new XmlDocument {
                XmlResolver = null
            };
            xmlDoc.Load(fileName);

            XmlNode node = xmlDoc.SelectSingleNode("/cpuid/processor");
            if (node == null) return null;

            string processor = node.Attributes["type"]?.Value;
            if (processor == null) processor = "x86";

            switch (processor) {
            case "x86":
                Intel.X86CpuIdFactory x86Factory = new Intel.X86CpuIdFactory();
                return x86Factory.Create(node);
            default:
                // This processor type is unknown.
                return null;
            }
        }
    }
}
