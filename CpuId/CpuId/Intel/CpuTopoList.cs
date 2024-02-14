namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A list of CPU topology information.
    /// </summary>
    public class CpuTopoList : IList<CpuTopo>
    {
        private readonly List<CpuTopo> m_CpuTopo = new();

        /// <summary>
        /// Gets or sets the <see cref="CpuTopo"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>An element describing a part of the CPU topology</returns>
        /// <exception cref="InvalidOperationException">List is read only.</exception>
        /// <exception cref="ArgumentNullException">Can't assign <see langword="null"/>.</exception>
        public CpuTopo this[int index]
        {
            get { return m_CpuTopo[index]; }
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException("List is read only");
                ThrowHelper.ThrowIfNull(value);
                m_CpuTopo[index] = value;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="CpuTopoList"/>.
        /// </summary>
        /// <value>The number of elements.</value>
        public int Count
        {
            get { return m_CpuTopo.Count; }
        }

        private bool m_IsReadOnly;

        /// <summary>
        /// Gets a value indicating whether the <see cref="CpuTopoList"/> is read-only.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if this instance is read only; otherwise, <see langword="false"/>.
        /// </value>
        /// <exception cref="InvalidOperationException">List is read only.</exception>
        /// <remarks>
        /// When the list is complete, you can set this property to <see langword="false"/> to make it immutable.
        /// </remarks>
        public bool IsReadOnly
        {
            get { return m_IsReadOnly; }
            set
            {
                if (m_IsReadOnly && !value)
                    throw new InvalidOperationException("List is read only");
                m_IsReadOnly = value;
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="CpuTopoList"/>.
        /// </summary>
        /// <param name="item">The object to add to <see cref="CpuTopoList"/>.</param>
        /// <exception cref="InvalidOperationException">List is read only.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is <see langword="null"/>.</exception>
        public void Add(CpuTopo item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("List is read only");
            ThrowHelper.ThrowIfNull(item);
            m_CpuTopo.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="CpuTopoList"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">List is read only.</exception>
        public void Clear()
        {
            if (IsReadOnly)
                throw new InvalidOperationException("List is read only");
            m_CpuTopo.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="CpuTopoList"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="CpuTopoList"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="item"/> is found in the <see cref="CpuTopoList"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool Contains(CpuTopo item)
        {
            return item is not null && m_CpuTopo.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="CpuTopoList"/> to an <see cref="Array"/>, starting at a particular
        /// <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the elements copied from
        /// <see cref="CpuTopoList"/>. The <see cref="Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(CpuTopo[] array, int arrayIndex)
        {
            m_CpuTopo.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<CpuTopo> GetEnumerator()
        {
            return m_CpuTopo.GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="CpuTopoList"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="CpuTopoList"/>.</param>
        /// <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1.</returns>
        public int IndexOf(CpuTopo item)
        {
            return item is null ? -1 : m_CpuTopo.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="CpuTopoList"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="CpuTopoList"/>.</param>
        /// <exception cref="InvalidOperationException">List is read only.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is <see langword="null"/>.</exception>
        public void Insert(int index, CpuTopo item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("List is read only");
            ThrowHelper.ThrowIfNull(item);
            m_CpuTopo.Insert(index, item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="CpuTopoList"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="CpuTopoList"/>.</param>
        /// <returns>
        /// Returns <see langword="true"/> if <paramref name="item"/> was successfully removed from the
        /// <see cref="CpuTopoList"/>; otherwise, <see langword="false"/>. This method also returns
        /// <see langword="false"/> if <paramref name="item"/> is not found in the original <see cref="CpuTopoList"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">List is read only.</exception>
        public bool Remove(CpuTopo item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("List is read only");
            return item is not null && m_CpuTopo.Remove(item);
        }

        /// <summary>
        /// Removes the <see cref="CpuTopoList"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="InvalidOperationException">List is read only.</exception>
        public void RemoveAt(int index)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("List is read only");
            m_CpuTopo.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
