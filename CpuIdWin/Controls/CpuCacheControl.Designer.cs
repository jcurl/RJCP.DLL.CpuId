namespace RJCP.Diagnostics.CpuIdWin.Controls
{

    partial class CpuCacheControl
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
            if (disposing && (components is not null)) {
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
            this.lblApicId = new System.Windows.Forms.Label();
            this.lbltxtApicId = new System.Windows.Forms.Label();
            this.tvwCache = new RJCP.Diagnostics.CpuIdWin.Controls.ThemeTreeView();
            this.SuspendLayout();
            // 
            // lblApicId
            // 
            this.lblApicId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblApicId.AutoEllipsis = true;
            this.lblApicId.Location = new System.Drawing.Point(141, 14);
            this.lblApicId.Name = "lblApicId";
            this.lblApicId.Size = new System.Drawing.Size(310, 14);
            this.lblApicId.TabIndex = 1;
            this.lblApicId.Text = "-";
            // 
            // lbltxtApicId
            // 
            this.lbltxtApicId.Location = new System.Drawing.Point(3, 14);
            this.lbltxtApicId.Name = "lbltxtApicId";
            this.lbltxtApicId.Size = new System.Drawing.Size(132, 14);
            this.lbltxtApicId.TabIndex = 0;
            this.lbltxtApicId.Text = "APIC Identifier:";
            this.lbltxtApicId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tvwCache
            // 
            this.tvwCache.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvwCache.Location = new System.Drawing.Point(144, 48);
            this.tvwCache.Name = "tvwCache";
            this.tvwCache.Size = new System.Drawing.Size(333, 269);
            this.tvwCache.TabIndex = 2;
            // 
            // CpuCacheControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvwCache);
            this.Controls.Add(this.lblApicId);
            this.Controls.Add(this.lbltxtApicId);
            this.Name = "CpuCacheControl";
            this.Size = new System.Drawing.Size(480, 320);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblApicId;
        private System.Windows.Forms.Label lbltxtApicId;
        private ThemeTreeView tvwCache;
    }
}
