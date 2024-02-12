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
        internal CpuFeature(string feature, bool value)
            : this(feature, value, FeatureGroup.Unknown, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CpuFeature"/> class.
        /// </summary>
        /// <param name="feature">The feature mnemonic.</param>
        /// <param name="value">Is <see langword="true"/> if the feature is present, otherwise <see langword="false"/>.</param>
        /// <param name="featureGroup">The standard feature group.</param>
        /// <param name="bitGroup">The name of the bit group. This string is language independent.</param>
        internal CpuFeature(string feature, bool value, FeatureGroup featureGroup, string bitGroup)
        {
            ThrowHelper.ThrowIfNull(feature);
            Feature = feature;
            Value = value;
            Group = featureGroup;
            BitGroup = bitGroup;
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

        /// <summary>
        /// Sets the description key by reading the resource for the feature.
        /// </summary>
        /// <param name="key">The feature, which is used as the key.</param>
        /// <param name="prefix">The prefix to the key.</param>
        /// <remarks>
        /// Gets the description for <paramref name="key"/> from <see cref="Resources.CpuFeatures"/>. If the
        /// <paramref name="prefix"/> is not <see langword="null"/> or whitespace, this the key with this prefix is
        /// checked first.
        /// </remarks>
        internal void SetDescriptionKey(string key, string prefix)
        {
            string descriptionKey = key.ToUpper(CultureInfo.InvariantCulture);
            string description;
            if (!string.IsNullOrEmpty(prefix)) {
                description = Resources.CpuFeatures.ResourceManager.GetString($"{prefix}_{descriptionKey}");
                if (description != null) {
                    Description = description;
                    return;
                }
            }
            description = Resources.CpuFeatures.ResourceManager.GetString(descriptionKey);
            if (description != null) Description = description;
        }

        /// <summary>
        /// DProvides a more detailed description of the given feature.
        /// </summary>
        /// <returns>The name of the feature.</returns>
        public string Description { get; private set; }
    }
}
