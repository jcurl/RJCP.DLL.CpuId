namespace RJCP.Diagnostics.CpuIdWin.Controls
{
    using System.Windows.Forms;

    public partial class CpuFeaturesControl : UserControl
    {
        public CpuFeaturesControl(ICpuId cpuId)
        {
            InitializeComponent();

            foreach (string feature in cpuId.Features) {
                string desc = cpuId.Features.Description(feature);

                ListViewItem lvi = new ListViewItem() {
                    Checked = cpuId.Features[feature],
                    Text = feature,
                };
                if (desc != null) lvi.SubItems.Add(desc);
                lvwFeatures.Items.Add(lvi);
            }
            hdrFeature.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            hdrDescription.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
    }
}
