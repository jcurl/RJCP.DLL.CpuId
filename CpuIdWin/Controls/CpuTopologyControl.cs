namespace RJCP.Diagnostics.CpuIdWin.Controls
{
    using System.Windows.Forms;
    using CpuId.Intel;

    public partial class CpuTopologyControl : UserControl
    {
        public CpuTopologyControl(ICpuIdX86 cpuId)
        {
            InitializeComponent();

            lblApicId.Text = cpuId.Topology.ApicId.ToString("X8");

            foreach (CpuTopo cpuTopo in cpuId.Topology.CoreTopology) {
                ListViewItem lvi = new ListViewItem {
                    Text = cpuTopo.TopoType.ToString(),
                    SubItems = {
                        cpuTopo.Id.ToString()
                    }
                };

                lvwCpuTopo.Items.Add(lvi);
            }
            hdrPackageLevel.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            hdrIdentifier.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
    }
}
