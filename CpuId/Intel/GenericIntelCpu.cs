namespace RJCP.Diagnostics.Intel
{
    /// <summary>
    /// CPU information for a generic Intel Clone.
    /// </summary>
    public class GenericIntelCpu : ICpuIdX86
    {
        private readonly BasicCpu m_Cpu;

        internal GenericIntelCpu(BasicCpu cpu)
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
        public int ApicId { get; protected set; }

        /// <inheritdoc/>
        public int ApicMaxThreads { get; protected set; }

        /// <inheritdoc/>
        public CpuFeatures Features { get; private set; }
    }
}
