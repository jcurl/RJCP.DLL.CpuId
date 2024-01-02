namespace RJCP.Diagnostics.CpuId.Intel
{
    /// <summary>
    /// Information about the Intel Big/Little architecture.
    /// </summary>
    public class BigLittleIntel : IBigLittle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BigLittleIntel"/> class.
        /// </summary>
        /// <param name="isPerformance">Set to <see langword="true"/> if this is a performance core.</param>
        /// <param name="coreType">Type of the microarchtiectural core.</param>
        /// <param name="modelId">The model identifier for the core.</param>
        public BigLittleIntel(bool isPerformance, BigLittleIntelCoreType coreType, int modelId)
        {
            IsPerformanceCore = isPerformance;
            CoreType = coreType;
            ModelId = modelId;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a performance core.
        /// </summary>
        /// <value><see langword="true" /> if this instance is a performance core; otherwise, <see langword="false" />.</value>
        public bool IsPerformanceCore { get; }

        /// <summary>
        /// Gets the microarchitectural type of the core.
        /// </summary>
        /// <value>The microarchitectural type of the core.</value>
        public BigLittleIntelCoreType CoreType { get; }

        /// <summary>
        /// Gets the model identifier for the core.
        /// </summary>
        /// <value>The model identifier for the core.</value>
        public int ModelId { get; }
    }
}
