namespace RJCP.Diagnostics.CpuId
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// A collection of CPU features.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Public API")]
    public class CpuFeatures : IEnumerable<string>
    {
        private readonly Dictionary<string, bool> m_Features =
            new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Determines the presence of a feature.
        /// </summary>
        /// <param name="key">The feature identifier as a string.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the feature is present and active, <see langword="false"/> if the feature
        /// is not available.
        /// </returns>
        /// <exception cref="InvalidOperationException">CPU Features is Read Only, you may only read it.</exception>
        public bool this[string key]
        {
            get
            {
                if (m_Features.TryGetValue(key, out bool value)) return value;
                return false;
            }
            internal set { m_Features[key] = value; }
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

        internal void Add(string key, bool value)
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
        /// DProvides a more detailed description of the given feature.
        /// </summary>
        /// <param name="key">The name of the feature.</param>
        /// <returns>The name of the feature.</returns>
        public string Description(string key)
        {
            return Resources.CpuFeatures.ResourceManager.GetString(key.ToUpper(CultureInfo.InvariantCulture));
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
