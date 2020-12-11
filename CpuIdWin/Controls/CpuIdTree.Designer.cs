namespace RJCP.Diagnostics.CpuIdWin.Controls
{

    partial class CpuIdTree
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CpuIdTree));
            this.tvwCpuId = new ThemeTreeView();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // tvwCpuId
            // 
            this.tvwCpuId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvwCpuId.ImageIndex = 0;
            this.tvwCpuId.ImageList = this.imgList;
            this.tvwCpuId.Location = new System.Drawing.Point(3, 3);
            this.tvwCpuId.Name = "tvwCpuId";
            this.tvwCpuId.SelectedImageIndex = 0;
            this.tvwCpuId.Size = new System.Drawing.Size(139, 480);
            this.tvwCpuId.TabIndex = 0;
            this.tvwCpuId.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwCpuId_AfterSelect);
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "icoCpu");
            this.imgList.Images.SetKeyName(1, "icoDetails");
            this.imgList.Images.SetKeyName(2, "icoCache");
            this.imgList.Images.SetKeyName(3, "icoDump");
            this.imgList.Images.SetKeyName(4, "icoFeatures");
            this.imgList.Images.SetKeyName(5, "icoTopology");
            // 
            // pnlInfo
            // 
            this.pnlInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInfo.Location = new System.Drawing.Point(148, 3);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(640, 480);
            this.pnlInfo.TabIndex = 1;
            // 
            // CpuIdTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.tvwCpuId);
            this.Name = "CpuIdTree";
            this.Size = new System.Drawing.Size(791, 486);
            this.ResumeLayout(false);

        }

        #endregion

        private ThemeTreeView tvwCpuId;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.ImageList imgList;
    }
}
