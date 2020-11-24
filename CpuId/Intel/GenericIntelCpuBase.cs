namespace RJCP.Diagnostics.Intel
{
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
    }
}
