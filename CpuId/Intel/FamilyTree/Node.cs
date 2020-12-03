namespace RJCP.Diagnostics.Intel.FamilyTree
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// A Graph node, where the key is an integer, and the value is a string.
    /// </summary>
    [DebuggerDisplay("Node: {Key}; {Value}")]
    internal class Node : INode
    {
        private readonly Dictionary<int, INode> m_Nodes = new Dictionary<int, INode>();

        public Node(int key) : this(key, string.Empty) { }

        public Node(int key, string value)
        {
            Key = key;
            Value = value;
        }

        public Node(int key, IEnumerable<Node> nodes)
        {
            Key = key;
            foreach (Node node in nodes) {
                m_Nodes.Add(node.Key, node);
            }
        }

        public int Key { get; private set; }

        public string Value { get; set; }

        public INode this[int key]
        {
            get
            {
                if (!m_Nodes.ContainsKey(key)) return new NodeEmpty(Key);
                return m_Nodes[key];
            }
        }

        public int Count { get { return m_Nodes.Count; } }

        public bool IsReadOnly { get { return false; } }

        public void Add(INode item)
        {
            if (m_Nodes.ContainsKey(item.Key)) return;
            m_Nodes.Add(item.Key, item);
        }

        public void Clear() { m_Nodes.Clear(); }

        public bool Contains(INode item)
        {
            return m_Nodes.ContainsKey(item.Key);
        }

        public void CopyTo(INode[] array, int arrayIndex)
        {
            m_Nodes.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<INode> GetEnumerator()
        {
            return m_Nodes.Values.GetEnumerator();
        }

        public bool Remove(INode item)
        {
            return m_Nodes.Remove(item.Key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
