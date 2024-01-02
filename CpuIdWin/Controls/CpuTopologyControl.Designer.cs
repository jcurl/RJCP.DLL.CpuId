﻿namespace RJCP.Diagnostics.CpuIdWin.Controls
{

    partial class CpuTopologyControl
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
            this.lblApicId = new System.Windows.Forms.Label();
            this.lbltxtApicId = new System.Windows.Forms.Label();
            this.lvwCpuTopo = new RJCP.Diagnostics.CpuIdWin.Controls.ThemeListView();
            this.hdrPackageLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrIdentifier = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrMask = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lbltxtBigLittle = new System.Windows.Forms.Label();
            this.lblBigLittle = new System.Windows.Forms.Label();
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
            // lvwCpuTopo
            // 
            this.lvwCpuTopo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwCpuTopo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrPackageLevel,
            this.hdrIdentifier,
            this.hdrMask});
            this.lvwCpuTopo.FullRowSelect = true;
            this.lvwCpuTopo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwCpuTopo.HideSelection = false;
            this.lvwCpuTopo.Location = new System.Drawing.Point(144, 58);
            this.lvwCpuTopo.Name = "lvwCpuTopo";
            this.lvwCpuTopo.Size = new System.Drawing.Size(333, 259);
            this.lvwCpuTopo.TabIndex = 2;
            this.lvwCpuTopo.UseCompatibleStateImageBehavior = false;
            this.lvwCpuTopo.View = System.Windows.Forms.View.Details;
            // 
            // hdrPackageLevel
            // 
            this.hdrPackageLevel.Text = "Package Level";
            // 
            // hdrIdentifier
            // 
            this.hdrIdentifier.Text = "Identifier";
            // 
            // hdrMask
            // 
            this.hdrMask.Text = "Mask";
            // 
            // lbltxtBigLittle
            // 
            this.lbltxtBigLittle.Location = new System.Drawing.Point(3, 31);
            this.lbltxtBigLittle.Name = "lbltxtBigLittle";
            this.lbltxtBigLittle.Size = new System.Drawing.Size(132, 14);
            this.lbltxtBigLittle.TabIndex = 3;
            this.lbltxtBigLittle.Text = "Big/Little Core:";
            this.lbltxtBigLittle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblBigLittle
            // 
            this.lblBigLittle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBigLittle.AutoEllipsis = true;
            this.lblBigLittle.Location = new System.Drawing.Point(141, 31);
            this.lblBigLittle.Name = "lblBigLittle";
            this.lblBigLittle.Size = new System.Drawing.Size(310, 14);
            this.lblBigLittle.TabIndex = 4;
            this.lblBigLittle.Text = "-";
            // 
            // CpuTopologyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblBigLittle);
            this.Controls.Add(this.lbltxtBigLittle);
            this.Controls.Add(this.lvwCpuTopo);
            this.Controls.Add(this.lblApicId);
            this.Controls.Add(this.lbltxtApicId);
            this.Name = "CpuTopologyControl";
            this.Size = new System.Drawing.Size(480, 320);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblApicId;
        private System.Windows.Forms.Label lbltxtApicId;
        private ThemeListView lvwCpuTopo;
        private System.Windows.Forms.ColumnHeader hdrPackageLevel;
        private System.Windows.Forms.ColumnHeader hdrIdentifier;
        private System.Windows.Forms.ColumnHeader hdrMask;
        private System.Windows.Forms.Label lbltxtBigLittle;
        private System.Windows.Forms.Label lblBigLittle;
    }
}
