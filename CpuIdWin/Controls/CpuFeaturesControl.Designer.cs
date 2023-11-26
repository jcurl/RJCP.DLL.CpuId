namespace RJCP.Diagnostics.CpuIdWin.Controls
{

    partial class CpuFeaturesControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lvwFeatures = new RJCP.Diagnostics.CpuIdWin.Controls.ThemeListView();
            this.hdrFeature = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrBits = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvwFeatures
            // 
            this.lvwFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwFeatures.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwFeatures.CheckBoxes = true;
            this.lvwFeatures.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrFeature,
            this.hdrBits,
            this.hdrDescription});
            this.lvwFeatures.FullRowSelect = true;
            this.lvwFeatures.GridLines = true;
            this.lvwFeatures.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwFeatures.HideSelection = false;
            this.lvwFeatures.LabelWrap = false;
            this.lvwFeatures.Location = new System.Drawing.Point(3, 3);
            this.lvwFeatures.MultiSelect = false;
            this.lvwFeatures.Name = "lvwFeatures";
            this.lvwFeatures.Size = new System.Drawing.Size(474, 314);
            this.lvwFeatures.TabIndex = 0;
            this.lvwFeatures.UseCompatibleStateImageBehavior = false;
            this.lvwFeatures.View = System.Windows.Forms.View.Details;
            this.lvwFeatures.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvwFeatures_ItemCheck);
            // 
            // hdrFeature
            // 
            this.hdrFeature.Text = "Feature";
            this.hdrFeature.Width = 105;
            // 
            // hdrBits
            // 
            this.hdrBits.Text = "Bit";
            // 
            // hdrDescription
            // 
            this.hdrDescription.Text = "Description";
            this.hdrDescription.Width = 176;
            // 
            // CpuFeaturesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvwFeatures);
            this.Name = "CpuFeaturesControl";
            this.Size = new System.Drawing.Size(480, 320);
            this.ResumeLayout(false);

        }

        #endregion

        private ThemeListView lvwFeatures;
        private System.Windows.Forms.ColumnHeader hdrFeature;
        private System.Windows.Forms.ColumnHeader hdrDescription;
        private System.Windows.Forms.ColumnHeader hdrBits;
    }
}
