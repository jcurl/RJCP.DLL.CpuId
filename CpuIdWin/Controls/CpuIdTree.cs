namespace RJCP.Diagnostics.CpuIdWin.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Windows.Forms;
    using CpuId;
    using CpuId.Intel;

    public partial class CpuIdTree : UserControl
    {
        private readonly ObservableCollection<ICpuId> m_Cores = new ObservableCollection<ICpuId>();
        private readonly Dictionary<TreeNode, TreeNodeData> m_NodeControls = new Dictionary<TreeNode, TreeNodeData>();

        private int m_NodeId;

        public CpuIdTree()
        {
            InitializeComponent();
            m_Cores.CollectionChanged += Cores_CollectionChanged;
        }

        private void Cores_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Each entry in m_Cores maps to a part of the tree view.
            switch (e.Action) {
            case NotifyCollectionChangedAction.Add:
                for (int i = 0; i < e.NewItems.Count; i++) {
                    tvwCpuId.Nodes.Insert(e.NewStartingIndex + i, BuildTreeNode(e.NewItems[i] as ICpuId, m_NodeId));
                    m_NodeId++;
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                for (int i = 0; i < e.OldItems.Count; i++) {
                    RemoveTreeNode(tvwCpuId.Nodes[e.OldStartingIndex]);
                }
                break;
            case NotifyCollectionChangedAction.Reset:
                pnlInfo.Controls.Clear();
                tvwCpuId.Nodes.Clear();
                foreach (TreeNodeData nodeData in m_NodeControls.Values) {
                    if (nodeData.Control != null) nodeData.Control.Dispose();
                }
                m_NodeControls.Clear();
                m_NodeId = 0;
                break;
            case NotifyCollectionChangedAction.Move:
                TreeNode node = tvwCpuId.Nodes[e.OldStartingIndex];
                tvwCpuId.Nodes.RemoveAt(e.OldStartingIndex);
                tvwCpuId.Nodes.Insert(e.NewStartingIndex, node);
                break;
            case NotifyCollectionChangedAction.Replace:
                if (e.NewStartingIndex != e.OldStartingIndex)
                    throw new NotSupportedException("Replacing multiple indices is not supported");

                RemoveTreeNode(tvwCpuId.Nodes[e.NewStartingIndex]);
                m_NodeId++;
                tvwCpuId.Nodes[e.NewStartingIndex] = BuildTreeNode(e.NewItems[0] as ICpuId, m_NodeId);
                break;
            }
        }

        private TreeNode GetSelectedCpuNode()
        {
            TreeNode selectedNode = tvwCpuId.SelectedNode;
            while (selectedNode != null) {
                if (m_NodeControls[selectedNode].NodeType == NodeType.CpuRootNode)
                    return selectedNode;
                selectedNode = selectedNode.Parent;
            }
            return selectedNode;
        }

        private TreeNode BuildTreeNode(ICpuId cpuId, int nodeNumber)
        {
            string nodeName;
            if (cpuId is ICpuIdX86 x86cpuId && x86cpuId.Topology.ApicId != -1) {
                nodeName = string.Format("Node: APIC {0:X8}", x86cpuId.Topology.ApicId);
            } else {
                nodeName = string.Format("Node: {0}", nodeNumber);
            }

            TreeNode node = new TreeNode(nodeName) {
                ImageKey = "icoCpu",
                SelectedImageKey = "icoCpu"
            };
            m_NodeControls.Add(node, new TreeNodeData() {
                NodeType = NodeType.CpuRootNode,
                CpuId = cpuId,
            });

            TreeNode nodeDetails = new TreeNode("Details") {
                ImageKey = "icoDetails",
                SelectedImageKey = "icoDetails"
            };
            m_NodeControls.Add(nodeDetails, new TreeNodeData() {
                NodeType = NodeType.CpuDetails,
                CpuId = cpuId,
            });
            node.Nodes.Add(nodeDetails);

            if (IsIntelOrAmd(cpuId)) {
                TreeNode nodeFeatures = new TreeNode("Features") {
                    ImageKey = "icoFeatures",
                    SelectedImageKey = "icoFeatures"
                };
                m_NodeControls.Add(nodeFeatures, new TreeNodeData() {
                    NodeType = NodeType.CpuFeatures,
                    CpuId = cpuId,
                });
                node.Nodes.Add(nodeFeatures);

                TreeNode nodeTopology = new TreeNode("Topology") {
                    ImageKey = "icoTopology",
                    SelectedImageKey = "icoTopology"
                };
                m_NodeControls.Add(nodeTopology, new TreeNodeData() {
                    NodeType = NodeType.CpuTopology,
                    CpuId = cpuId,
                });
                node.Nodes.Add(nodeTopology);

                TreeNode nodeCache = new TreeNode("Cache") {
                    ImageKey = "icoCache",
                    SelectedImageKey = "icoCache"
                };
                m_NodeControls.Add(nodeCache, new TreeNodeData() {
                    NodeType = NodeType.CpuCache,
                    CpuId = cpuId,
                });
                node.Nodes.Add(nodeCache);
            }

            if (IsX86Cpu(cpuId)) {
                TreeNode nodeDump = new TreeNode("Dump") {
                    ImageKey = "icoDump",
                    SelectedImageKey = "icoDump"
                };
                m_NodeControls.Add(nodeDump, new TreeNodeData() {
                    NodeType = NodeType.CpuDump,
                    CpuId = cpuId,
                });
                node.Nodes.Add(nodeDump);
            }

            return node;
        }

        private bool IsIntelOrAmd(ICpuId cpuId)
        {
            return cpuId is GenuineIntelCpu || cpuId is AuthenticAmdCpu;
        }

        private bool IsX86Cpu(ICpuId cpuId)
        {
            return cpuId is ICpuIdX86;
        }

        private void RemoveTreeNode(TreeNode node)
        {
            TreeNode selectedNode = GetSelectedCpuNode();

            tvwCpuId.Nodes.Remove(node);
            foreach (TreeNode child in node.Nodes) {
                if (m_NodeControls.TryGetValue(child, out TreeNodeData nodeData)) {
                    if (child == selectedNode) {
                        pnlInfo.Controls.Clear();
                    }
                    if (nodeData.Control != null) nodeData.Control.Dispose();
                    m_NodeControls.Remove(child);
                }
            }
        }

        public ObservableCollection<ICpuId> Cores { get { return m_Cores; } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "False positive")]
        private void tvwCpuId_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!m_NodeControls.TryGetValue(e.Node, out TreeNodeData data)) {
                // The function BuildTreeNode didn't define an action for this node.
                pnlInfo.Controls.Clear();
                return;
            }

            if (data.Control == null) {
                ICpuIdX86 x86cpu = data.CpuId as ICpuIdX86;

                switch (data.NodeType) {
                case NodeType.CpuDetails:
                    data.Control = new CpuDetailsControl(data.CpuId);
                    break;
                case NodeType.CpuFeatures:
                    data.Control = new CpuFeaturesControl(data.CpuId);
                    break;
                case NodeType.CpuDump:
                    if (x86cpu == null) return;
                    data.Control = new CpuDumpControl(x86cpu);
                    break;
                case NodeType.CpuTopology:
                    if (x86cpu == null) return;
                    data.Control = new CpuTopologyControl(x86cpu);
                    break;
                case NodeType.CpuCache:
                    if (x86cpu == null) return;
                    data.Control = new CpuCacheControl(x86cpu);
                    break;
                default:
                    // We don't have a user control for this action.
                    pnlInfo.Controls.Clear();
                    return;
                }
                data.Control.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }

            // Adding the control at runtime after everything is scaled requires scaling of the panel to the same
            // dimensions of the user control at the time it was designed.
            SizeF scale = GetRuntimeScaleFactor(data.Control);
            int width = (int)((pnlInfo.ClientSize.Width - 6) / scale.Width);
            int height = (int)((pnlInfo.ClientSize.Height - 6) / scale.Height);
            data.Control.Location = new Point(3, 3);
            data.Control.Size = new Size(width, height);

            pnlInfo.SuspendLayout();
            pnlInfo.Controls.Clear();
            pnlInfo.Controls.Add(data.Control);
            pnlInfo.ResumeLayout();
        }

        private SizeF GetRuntimeScaleFactor(ContainerControl subControl)
        {
            // Controls that are added to a form at design time, must have AutoScaleMode.Inherit for it to scale
            // properly for high resolution DPI screens (specifically, where the designer is a different resolution than
            // the screen resolution that the software will run). Otherwise the UserControl is created and scaled
            // properly, but when the Form.InitializeComponent() is run, the components within the user control are not
            // scaled properly.
            //
            // But the ContainerControl does not have it's own AutoScaleDimensions set, where we need to look through
            // the parents looking on how to do the scaling.
            //
            // But here, we're given a subControl that isn't initialized in the InitializeComponent(), but added at
            // runtime, where we do the scaling manually before adding it.

            Control control = this;
            while (control != null) {
                if (control is ContainerControl containerControl) {
                    switch (containerControl.AutoScaleMode) {
                    case AutoScaleMode.Inherit:
                        control = control.Parent;
                        break;
                    case AutoScaleMode.Font:
                        return new SizeF(
                            containerControl.AutoScaleDimensions.Width / subControl.AutoScaleDimensions.Width,
                            containerControl.AutoScaleDimensions.Height / subControl.AutoScaleDimensions.Height);
                    default:
                        return new SizeF(1F, 1F);
                    }
                } else {
                    control = control.Parent;
                }
            }
            return new SizeF(1F, 1F);
        }
    }
}
