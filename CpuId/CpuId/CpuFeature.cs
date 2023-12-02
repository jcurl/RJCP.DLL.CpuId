namespace RJCP.Diagnostics.CpuId
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Describes a CPUID bit feature group.
    /// </summary>
    public class CpuFeature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CpuFeature"/> class.
        /// </summary>
        /// <param name="feature">The feature mnemonic.</param>
        /// <param name="value">Is <see langword="true"/> if the feature is present, otherwise <see langword="false"/>.</param>
        public CpuFeature(string feature, bool value)
            : this(feature, value, FeatureGroup.Unknown, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CpuFeature"/> class.
        /// </summary>
        /// <param name="feature">The feature mnemonic.</param>
        /// <param name="value">Is <see langword="true"/> if the feature is present, otherwise <see langword="false"/>.</param>
        /// <param name="featureGroup">The standard feature group.</param>
        /// <param name="bitGroup">The name of the bit group. This string is language independent.</param>
        public CpuFeature(string feature, bool value, FeatureGroup featureGroup, string bitGroup)
        {
            if (feature == null) throw new ArgumentNullException(nameof(feature));
            Feature = feature;
            Value = value;
            Group = featureGroup;
            BitGroup = bitGroup;
            SetDescriptionKey(Feature);
        }

        /// <summary>
        /// Gets the feature mnemonic.
        /// </summary>
        /// <value>The feature mnemonic.</value>
        public string Feature { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="CpuFeature"/> is set.
        /// </summary>
        /// <value><see langword="true"/> if the feature is set; otherwise, <see langword="false"/>.</value>
        public bool Value { get; internal set; }

        /// <summary>
        /// Gets the feature group, that can be used to identify what the feature is for.
        /// </summary>
        /// <value>The feature group.</value>
        public FeatureGroup Group { get; }

        /// <summary>
        /// Gets the bit group.
        /// </summary>
        /// <value>The language independent feature bit group. This would be similar to <c>CPUID.01h.ECX[bit].</c></value>
        public string BitGroup { get; }

        /// <summary>
        /// Tests if this feature is part of a reserved bit.
        /// </summary>
        /// <value><see langword="true"/> if this feature is reserved; otherwise, <see langword="false"/>.</value>
        public bool IsReserved { get; internal set; }

        private string m_DescriptionKey;

        internal void SetDescriptionKey(string name)
        {
            m_DescriptionKey = name.ToUpper(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// DProvides a more detailed description of the given feature.
        /// </summary>
        /// <returns>The name of the feature.</returns>
        public string Description
        {
            get
            {
                return Resources.CpuFeatures.ResourceManager.GetString(m_DescriptionKey);
            }
        }
    }
}
