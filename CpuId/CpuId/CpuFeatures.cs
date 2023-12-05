namespace RJCP.Diagnostics.CpuId
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A collection of CPU features.
    /// </summary>
    public class CpuFeatures : IEnumerable<string>
    {
        private readonly Dictionary<string, CpuFeature> m_Features =
            new Dictionary<string, CpuFeature>(StringComparer.InvariantCultureIgnoreCase);

        private readonly Dictionary<string, CpuFeature> m_NoFeature =
            new Dictionary<string, CpuFeature>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="CpuFeatures"/> class.
        /// </summary>
        internal CpuFeatures() { }

        private CpuFeature GetNoFeature(string key)
        {
            if (m_NoFeature.TryGetValue(key, out CpuFeature value)) return value;
            value = new CpuFeature(key, false) {
                // Because this feature is unknown, consider it reserved.
                IsReserved = true
            };
            // We still search for the feature, in case it isn't defined.
            value.SetDescriptionKey(key, DescriptionPrefix);
            m_NoFeature.Add(key, value);
            return value;
        }

        /// <summary>
        /// Determines the presence of a feature.
        /// </summary>
        /// <param name="key">The feature identifier as a string.</param>
        /// <returns>
        /// Returns a <see cref="CpuFeature"/> that provides information about the feature and if it is set. If the
        /// feature is unknown, a default feature in an unknown function group is returned and the value is <see
        /// langword="false"/>.
        /// </returns>
        public CpuFeature this[string key]
        {
            get
            {
                if (m_Features.TryGetValue(key, out CpuFeature value)) return value;
                return GetNoFeature(key);
            }
            internal set
            {
                if (m_Features.ContainsKey(key)) return;
                m_Features[key] = value;
            }
        }

        /// <summary>
        /// Gets the number of features known by this class.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return m_Features.Count; }
        }

        /// <summary>
        /// Gets or sets the prefix for the key to get the description.
        /// </summary>
        /// <value>The prefix applied to the key used to get the description.</value>
        /// <remarks>
        /// Allows different CPU vendors to have different descriptions for the same feature name, by prefixing the key
        /// to the resource with this value.
        /// </remarks>
        internal string DescriptionPrefix { get; set; }

        /// <summary>
        /// Adds the specified feature to this collection.
        /// </summary>
        /// <param name="feature">The name of the feature as designated by the CPU vendor.</param>
        /// <param name="value">Sets the feature enabled if <see langword="true"/>, disabled otherwise..</param>
        /// <param name="featureGroup">The feature group.</param>
        /// <param name="bitGroup">The bit group, describing where the feature originated.</param>
        /// <returns>A <see cref="CpuFeature"/> that was just added.</returns>
        internal CpuFeature Add(string feature, bool value, FeatureGroup featureGroup, string bitGroup)
        {
            if (m_Features.TryGetValue(feature, out CpuFeature cpuFeature))
                return cpuFeature;

            cpuFeature = new CpuFeature(feature, value, featureGroup, bitGroup);
            cpuFeature.SetDescriptionKey(feature, DescriptionPrefix);
            m_Features[feature] = cpuFeature;
            return cpuFeature;
        }

        /// <summary>
        /// Adds the feature as the key.
        /// </summary>
        /// <param name="key">The name of the feature which to alias to.</param>
        /// <param name="value">The <see cref="CpuFeature"/> to alias.</param>
        /// <remarks>
        /// This function adds an existing <see cref="CpuFeature"/> to the collection. Normally, use
        /// <see cref="Add(string, bool, FeatureGroup, string)"/> which creates the <see cref="CpuFeature"/> and adds
        /// it, such that the key and the feature are the same. In this case, the <paramref name="key"/> may be
        /// different to the <see cref="CpuFeature.Feature"/>, and is thus an alias.
        /// </remarks>
        internal void Add(string key, CpuFeature value)
        {
            m_Features.Add(key, value);
            if (string.IsNullOrEmpty(value.Description))
                value.SetDescriptionKey(key, null);
        }

        /// <summary>
        /// Clears this instance of all features.
        /// </summary>
        internal void Clear()
        {
            m_Features.Clear();
        }

        /// <summary>
        /// Removes the specified key from the collection.
        /// </summary>
        /// <param name="key">The key to remove from the collection.</param>
        /// <returns>
        /// <see langword="true"/> if the feature exists and was removed, <see langword="false"/> otherwise.
        /// </returns>
        internal bool Remove(string key)
        {
            return m_Features.Remove(key);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the known features of the CPU.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<string> GetEnumerator()
        {
            return m_Features.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
