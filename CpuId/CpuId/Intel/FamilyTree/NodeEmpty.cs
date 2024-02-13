namespace RJCP.Diagnostics.CpuId.Intel.FamilyTree
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("NodeEmpty: {Key}")]
    internal class NodeEmpty : INode
    {
        private static readonly List<INode> m_Empty = new();

        public NodeEmpty(int key)
        {
            Key = key;
        }

        public INode this[int key]
        {
            get { return new NodeEmpty(key); }
        }

        public int Key { get; private set; }

        public string Value
        {
            get { return string.Empty; }
            set { throw new InvalidOperationException("Object is read only"); }
        }

        public int Count { get { return 0; } }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public void Add(INode item)
        {
            throw new InvalidOperationException("Object is read only");
        }

        public void Clear()
        {
            throw new InvalidOperationException("Object is read only");
        }

        public bool Contains(INode item)
        {
            return false;
        }

        public void CopyTo(INode[] array, int arrayIndex) { /* Nothing to do */ }

        public IEnumerator<INode> GetEnumerator()
        {
            return m_Empty.GetEnumerator();
        }

        public bool Remove(INode item)
        {
            throw new InvalidOperationException("Object is read only");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
