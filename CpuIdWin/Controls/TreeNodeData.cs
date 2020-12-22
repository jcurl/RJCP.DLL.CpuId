namespace RJCP.Diagnostics.CpuIdWin.Controls
{
    using System.Windows.Forms;
    using CpuId;

    public class TreeNodeData
    {
        public UserControl Control { get; set; }

        public ICpuId CpuId { get; set; }

        public NodeType NodeType { get; set; }
    }
}
