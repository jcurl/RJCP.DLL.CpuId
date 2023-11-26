namespace RJCP.Diagnostics.CpuIdWin.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using CpuId;

    public partial class CpuFeaturesControl : UserControl
    {
        public CpuFeaturesControl(ICpuId cpuId)
        {
            if (cpuId == null) throw new ArgumentNullException(nameof(cpuId));

            InitializeComponent();

            lvwFeatures.SuspendLayout();
            Dictionary<FeatureGroup, ListViewGroup> groups = new Dictionary<FeatureGroup, ListViewGroup>();
            foreach (string feature in cpuId.Features) {
                CpuFeature cpuFeature = cpuId.Features[feature];
                if (!groups.TryGetValue(cpuFeature.Group, out ListViewGroup lvg)) {
                    string groupName = Resources.UserInterface.ResourceManager.GetString($"fg{cpuFeature.Group}")
                        ?? $"fg{cpuFeature.Group}";

                    lvg = new ListViewGroup($"fg{cpuFeature.Group}", HorizontalAlignment.Left) {
                        Header = groupName,
                        Name = $"lvg{cpuFeature.Group}"
                    };
                    groups.Add(cpuFeature.Group, lvg);
                    lvwFeatures.Groups.Add(lvg);
                }

                ListViewItem lvi = new ListViewItem() {
                    Checked = cpuId.Features[feature].Value,
                    Text = feature,
                    Group = lvg
                };

                string bitGrp = cpuFeature.BitGroup;
                string desc = cpuFeature.Description;
                lvi.SubItems.Add(bitGrp ?? string.Empty);
                lvi.SubItems.Add(desc ?? string.Empty);
                lvwFeatures.Items.Add(lvi);
            }
            lvwFeatures.ResumeLayout();

            hdrFeature.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            hdrBits.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            hdrDescription.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void lvwFeatures_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // The check boxes are read only. If the handle isn't created, then we're in the construction phase and we
            // should be able to set the value.
            if (IsHandleCreated) e.NewValue = e.CurrentValue;
        }
    }
}
