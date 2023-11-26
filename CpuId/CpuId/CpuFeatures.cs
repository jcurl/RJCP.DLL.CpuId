namespace RJCP.Diagnostics.CpuId
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A collection of CPU features.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Public API")]
    public class CpuFeatures : IEnumerable<string>
    {
        private readonly Dictionary<string, CpuFeature> m_Features =
            new Dictionary<string, CpuFeature>(StringComparer.InvariantCultureIgnoreCase);

        private readonly Dictionary<string, CpuFeature> m_NoFeature =
            new Dictionary<string, CpuFeature>(StringComparer.InvariantCultureIgnoreCase);

        private CpuFeature GetNoFeature(string key)
        {
            if (m_NoFeature.TryGetValue(key, out CpuFeature value)) return value;
            value = new CpuFeature(key, false);
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
        /// Gets a value indicating whether this collection is read only.
        /// </summary>
        /// <value>This property always returns <see langword="true"/> indicating the collection is read only.</value>
        public bool IsReadOnly
        {
            get { return true; }
        }

        internal void Add(string key, CpuFeature value)
        {
            m_Features.Add(key, value);
        }

        internal void Clear()
        {
            m_Features.Clear();
        }

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
