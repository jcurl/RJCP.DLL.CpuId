namespace RJCP.Diagnostics.CpuIdWin.Controls
{
    using System;
    using System.Windows.Forms;
    using CpuId.Intel;

    public partial class CpuCacheControl : UserControl
    {
        private readonly int m_CoreMask;

        public CpuCacheControl(ICpuIdX86 cpuId)
        {
            ThrowHelper.ThrowIfNull(cpuId);

            // The CoreMask is used to know how many bits to show for the cache mask.
            foreach (CpuTopo cpuLevel in cpuId.Topology.CoreTopology) {
                if (cpuLevel.TopoType == CpuTopoType.Package) {
                    if (cpuLevel.Mask == 0) {
                        m_CoreMask = 8;
                    } else {
                        m_CoreMask = GetBitSize(~cpuLevel.Mask);
                    }
                }
            }
            if (m_CoreMask < 8) m_CoreMask = 8;

            InitializeComponent();

            lblApicId.Text = cpuId.Topology.ApicId.ToString("X8");

            foreach (CacheTopo cacheTopo in cpuId.Topology.CacheTopology) {
                if (cacheTopo is CacheTopoCpu cacheTopoCpu) {
                    tvwCache.Nodes.Add(CacheCpuNode(cacheTopoCpu));
                } else if (cacheTopo is CacheTopoTlb cacheTopoTlb) {
                    tvwCache.Nodes.Add(CacheTlbNode(cacheTopoTlb));
                } else if (cacheTopo is CacheTopoTrace cacheTopoTrace) {
                    tvwCache.Nodes.Add(CacheTraceNode(cacheTopoTrace));
                } else if (cacheTopo is CacheTopoPrefetch cacheTopoPrefetch) {
                    tvwCache.Nodes.Add(CachePrefetchNode(cacheTopoPrefetch));
                }
            }
        }

        private static int GetBitSize(long value)
        {
            if (value < 0x100) return 8;
            if (value < 0x1000) return 12;
            if (value < 0x10000) return 16;
            if (value < 0x100000) return 20;
            if (value < 0x1000000) return 24;
            if (value < 0x10000000) return 28;
            return 32;
        }

        private TreeNode CacheCpuNode(CacheTopoCpu cacheTopoCpu)
        {
            TreeNode node = new TreeNode(cacheTopoCpu.ToString());
            node.Nodes.Add(string.Format("Level: {0}", cacheTopoCpu.Level));
            node.Nodes.Add(string.Format("Size: {0} kB", cacheTopoCpu.Size / 1024));
            node.Nodes.Add(string.Format("Associativity: {0}", GetAssociativity(cacheTopoCpu.Associativity, cacheTopoCpu.Sets)));
            node.Nodes.Add(string.Format("Line Size: {0} bytes", cacheTopoCpu.LineSize));
            if (cacheTopoCpu.Sets != 1) node.Nodes.Add(string.Format("Sets: {0}", cacheTopoCpu.Sets));
            node.Nodes.Add(string.Format("Partitions: {0}", cacheTopoCpu.Partitions));
            if (cacheTopoCpu.Mask != -1)
                node.Nodes.Add(string.Format("Mask: {0}",
                    Infrastructure.Text.ConvertToBitString(cacheTopoCpu.Mask, m_CoreMask)));
            return node;
        }

        private TreeNode CacheTlbNode(CacheTopoTlb cacheTopoTlb)
        {
            TreeNode node = new TreeNode(cacheTopoTlb.ToString());
            node.Nodes.Add(string.Format("Level: {0}", cacheTopoTlb.Level));
            node.Nodes.Add(string.Format("Entries: {0}", cacheTopoTlb.Entries));
            node.Nodes.Add(string.Format("Associativity: {0}", GetAssociativity(cacheTopoTlb.Associativity, cacheTopoTlb.Sets)));
            if (cacheTopoTlb.Sets != 1) node.Nodes.Add(string.Format("Sets: {0}", cacheTopoTlb.Sets));
            if (cacheTopoTlb.Mask != -1)
                node.Nodes.Add(string.Format("Mask: {0}",
                    Infrastructure.Text.ConvertToBitString(cacheTopoTlb.Mask, m_CoreMask)));
            return node;
        }

        private static TreeNode CachePrefetchNode(CacheTopoPrefetch cacheTopoPrefetch)
        {
            TreeNode node = new TreeNode(cacheTopoPrefetch.ToString());
            node.Nodes.Add(string.Format("Level: {0}", cacheTopoPrefetch.Level));
            node.Nodes.Add(string.Format("Prefetch Size: {0} bytes", cacheTopoPrefetch.LineSize));
            return node;
        }

        private static TreeNode CacheTraceNode(CacheTopoTrace cacheTopoTrace)
        {
            TreeNode node = new TreeNode(cacheTopoTrace.ToString());
            node.Nodes.Add(string.Format("Level: {0}", cacheTopoTrace.Level));
            node.Nodes.Add(string.Format("Associativity: {0}", GetAssociativity(cacheTopoTrace.Associativity, 0)));
            node.Nodes.Add(string.Format("Size: {0} kB μ-ops", cacheTopoTrace.Size / 1024));
            return node;
        }

        private static string GetAssociativity(int associativity, int sets)
        {
            return sets == 1 ? "Full" : string.Format("{0}-way", associativity);
        }
    }
}
