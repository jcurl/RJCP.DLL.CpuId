namespace RJCP.Diagnostics.CpuIdWin.Controls
{
    using System;
    using System.Windows.Forms;
    using CpuId.Intel;

    public partial class CpuTopologyControl : UserControl
    {
        public CpuTopologyControl(ICpuIdX86 cpuId)
        {
            ThrowHelper.ThrowIfNull(cpuId);

            // The CoreMask is used to know how many bits to show for the cache mask.
            int coreMask = 0;
            foreach (CpuTopo cpuLevel in cpuId.Topology.CoreTopology) {
                if (cpuLevel.TopoType == CpuTopoType.Package) {
                    if (cpuLevel.Mask == 0) {
                        coreMask = 8;
                    } else {
                        coreMask = GetBitSize(~cpuLevel.Mask);
                    }
                }
            }
            if (coreMask < 8) coreMask = 8;

            InitializeComponent();

            lblApicId.Text = cpuId.Topology.ApicId.ToString("X8");

            if (cpuId.Topology.BigLittle is BigLittleIntel perfCore) {
                switch (perfCore.CoreType) {
                case BigLittleIntelCoreType.IntelAtom:
                    lblBigLittle.Text = $"Intel ATOM (model {perfCore.ModelId:x06}h)";
                    break;
                case BigLittleIntelCoreType.IntelCore:
                    lblBigLittle.Text = $"Intel Core (model {perfCore.ModelId:x06}h)";
                    break;
                default:
                    lblBigLittle.Text = $"Intel {perfCore.CoreType:x02} (model {perfCore.ModelId:x06}h)";
                    break;
                }
            }

            foreach (CpuTopo cpuTopo in cpuId.Topology.CoreTopology) {
                ListViewItem lvi = new ListViewItem {
                    Text = cpuTopo.TopoType.ToString(),
                    SubItems = {
                        cpuTopo.Id.ToString(),
                        ""
                    }
                };

                if (cpuTopo.TopoType != CpuTopoType.Package) {
                    lvi.SubItems[2].Text = Infrastructure.Text.ConvertToBitString(cpuTopo.Mask, coreMask);
                }
                lvwCpuTopo.Items.Add(lvi);
            }
            hdrPackageLevel.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            hdrIdentifier.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            hdrMask.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
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
    }
}
