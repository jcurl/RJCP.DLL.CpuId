namespace RJCP.Diagnostics.CpuIdWin.Controls
{

    partial class CpuDetailsControl
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
            this.lbltxtCpuVendor = new System.Windows.Forms.Label();
            this.lblCpuVendor = new System.Windows.Forms.Label();
            this.lblVendorId = new System.Windows.Forms.Label();
            this.lbltxtVendorId = new System.Windows.Forms.Label();
            this.lblBrand = new System.Windows.Forms.Label();
            this.lbltxtBrand = new System.Windows.Forms.Label();
            this.lbltxtDescription = new System.Windows.Forms.Label();
            this.lbltxtProcessorSignature = new System.Windows.Forms.Label();
            this.lblFamily = new System.Windows.Forms.Label();
            this.lbltxtFamily = new System.Windows.Forms.Label();
            this.lblModel = new System.Windows.Forms.Label();
            this.lbltxtModel = new System.Windows.Forms.Label();
            this.lblStepping = new System.Windows.Forms.Label();
            this.lbltxtStepping = new System.Windows.Forms.Label();
            this.lblProcessorType = new System.Windows.Forms.Label();
            this.lbltxtProcessorType = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.TextBox();
            this.lblProcessorSignature = new System.Windows.Forms.TextBox();
            this.lblHypervisor = new System.Windows.Forms.TextBox();
            this.lbltxtHypervisor = new System.Windows.Forms.Label();
            this.lbltxtFeatureLevel = new System.Windows.Forms.Label();
            this.lblFeatureLevel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbltxtCpuVendor
            // 
            this.lbltxtCpuVendor.Location = new System.Drawing.Point(3, 14);
            this.lbltxtCpuVendor.Name = "lbltxtCpuVendor";
            this.lbltxtCpuVendor.Size = new System.Drawing.Size(132, 14);
            this.lbltxtCpuVendor.TabIndex = 0;
            this.lbltxtCpuVendor.Text = "CPU Vendor (Identified):";
            this.lbltxtCpuVendor.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblCpuVendor
            // 
            this.lblCpuVendor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCpuVendor.AutoEllipsis = true;
            this.lblCpuVendor.Location = new System.Drawing.Point(141, 14);
            this.lblCpuVendor.Name = "lblCpuVendor";
            this.lblCpuVendor.Size = new System.Drawing.Size(310, 14);
            this.lblCpuVendor.TabIndex = 1;
            this.lblCpuVendor.Text = "-";
            // 
            // lblVendorId
            // 
            this.lblVendorId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVendorId.AutoEllipsis = true;
            this.lblVendorId.Location = new System.Drawing.Point(141, 38);
            this.lblVendorId.Name = "lblVendorId";
            this.lblVendorId.Size = new System.Drawing.Size(310, 14);
            this.lblVendorId.TabIndex = 3;
            this.lblVendorId.Text = "-";
            // 
            // lbltxtVendorId
            // 
            this.lbltxtVendorId.Location = new System.Drawing.Point(3, 38);
            this.lbltxtVendorId.Name = "lbltxtVendorId";
            this.lbltxtVendorId.Size = new System.Drawing.Size(132, 14);
            this.lbltxtVendorId.TabIndex = 2;
            this.lbltxtVendorId.Text = "CPU Vendor:";
            this.lbltxtVendorId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblBrand
            // 
            this.lblBrand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBrand.AutoEllipsis = true;
            this.lblBrand.Location = new System.Drawing.Point(141, 70);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(310, 14);
            this.lblBrand.TabIndex = 7;
            this.lblBrand.Text = "-";
            // 
            // lbltxtBrand
            // 
            this.lbltxtBrand.Location = new System.Drawing.Point(3, 70);
            this.lbltxtBrand.Name = "lbltxtBrand";
            this.lbltxtBrand.Size = new System.Drawing.Size(132, 14);
            this.lbltxtBrand.TabIndex = 6;
            this.lbltxtBrand.Text = "Brand Id:";
            this.lbltxtBrand.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbltxtDescription
            // 
            this.lbltxtDescription.Location = new System.Drawing.Point(3, 54);
            this.lbltxtDescription.Name = "lbltxtDescription";
            this.lbltxtDescription.Size = new System.Drawing.Size(132, 14);
            this.lbltxtDescription.TabIndex = 4;
            this.lbltxtDescription.Text = "Brand String:";
            this.lbltxtDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbltxtProcessorSignature
            // 
            this.lbltxtProcessorSignature.Location = new System.Drawing.Point(3, 94);
            this.lbltxtProcessorSignature.Name = "lbltxtProcessorSignature";
            this.lbltxtProcessorSignature.Size = new System.Drawing.Size(132, 14);
            this.lbltxtProcessorSignature.TabIndex = 8;
            this.lbltxtProcessorSignature.Text = "Processor Signature:";
            this.lbltxtProcessorSignature.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblFamily
            // 
            this.lblFamily.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFamily.Location = new System.Drawing.Point(141, 110);
            this.lblFamily.Name = "lblFamily";
            this.lblFamily.Size = new System.Drawing.Size(310, 14);
            this.lblFamily.TabIndex = 11;
            this.lblFamily.Text = "-";
            // 
            // lbltxtFamily
            // 
            this.lbltxtFamily.Location = new System.Drawing.Point(3, 110);
            this.lbltxtFamily.Name = "lbltxtFamily";
            this.lbltxtFamily.Size = new System.Drawing.Size(132, 14);
            this.lbltxtFamily.TabIndex = 10;
            this.lbltxtFamily.Text = "Family:";
            this.lbltxtFamily.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblModel
            // 
            this.lblModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblModel.Location = new System.Drawing.Point(141, 126);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(310, 14);
            this.lblModel.TabIndex = 13;
            this.lblModel.Text = "-";
            // 
            // lbltxtModel
            // 
            this.lbltxtModel.Location = new System.Drawing.Point(3, 126);
            this.lbltxtModel.Name = "lbltxtModel";
            this.lbltxtModel.Size = new System.Drawing.Size(132, 14);
            this.lbltxtModel.TabIndex = 12;
            this.lbltxtModel.Text = "Model:";
            this.lbltxtModel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblStepping
            // 
            this.lblStepping.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStepping.Location = new System.Drawing.Point(141, 142);
            this.lblStepping.Name = "lblStepping";
            this.lblStepping.Size = new System.Drawing.Size(310, 14);
            this.lblStepping.TabIndex = 15;
            this.lblStepping.Text = "-";
            // 
            // lbltxtStepping
            // 
            this.lbltxtStepping.Location = new System.Drawing.Point(3, 142);
            this.lbltxtStepping.Name = "lbltxtStepping";
            this.lbltxtStepping.Size = new System.Drawing.Size(132, 14);
            this.lbltxtStepping.TabIndex = 14;
            this.lbltxtStepping.Text = "Stepping:";
            this.lbltxtStepping.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblProcessorType
            // 
            this.lblProcessorType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProcessorType.Location = new System.Drawing.Point(141, 158);
            this.lblProcessorType.Name = "lblProcessorType";
            this.lblProcessorType.Size = new System.Drawing.Size(310, 14);
            this.lblProcessorType.TabIndex = 17;
            this.lblProcessorType.Text = "-";
            // 
            // lbltxtProcessorType
            // 
            this.lbltxtProcessorType.Location = new System.Drawing.Point(3, 158);
            this.lbltxtProcessorType.Name = "lbltxtProcessorType";
            this.lbltxtProcessorType.Size = new System.Drawing.Size(132, 14);
            this.lbltxtProcessorType.TabIndex = 16;
            this.lbltxtProcessorType.Text = "Processor Type:";
            this.lbltxtProcessorType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.BackColor = System.Drawing.SystemColors.Control;
            this.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblDescription.Location = new System.Drawing.Point(144, 54);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.ReadOnly = true;
            this.lblDescription.Size = new System.Drawing.Size(307, 13);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "-";
            // 
            // lblProcessorSignature
            // 
            this.lblProcessorSignature.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProcessorSignature.BackColor = System.Drawing.SystemColors.Control;
            this.lblProcessorSignature.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblProcessorSignature.Location = new System.Drawing.Point(144, 95);
            this.lblProcessorSignature.Name = "lblProcessorSignature";
            this.lblProcessorSignature.ReadOnly = true;
            this.lblProcessorSignature.Size = new System.Drawing.Size(307, 13);
            this.lblProcessorSignature.TabIndex = 9;
            this.lblProcessorSignature.Text = "-";
            // 
            // lblHypervisor
            // 
            this.lblHypervisor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHypervisor.BackColor = System.Drawing.SystemColors.Control;
            this.lblHypervisor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblHypervisor.Location = new System.Drawing.Point(144, 209);
            this.lblHypervisor.Name = "lblHypervisor";
            this.lblHypervisor.ReadOnly = true;
            this.lblHypervisor.Size = new System.Drawing.Size(307, 13);
            this.lblHypervisor.TabIndex = 19;
            this.lblHypervisor.Text = "-";
            // 
            // lbltxtHypervisor
            // 
            this.lbltxtHypervisor.Location = new System.Drawing.Point(3, 208);
            this.lbltxtHypervisor.Name = "lbltxtHypervisor";
            this.lbltxtHypervisor.Size = new System.Drawing.Size(132, 14);
            this.lbltxtHypervisor.TabIndex = 18;
            this.lbltxtHypervisor.Text = "Hypervisor:";
            this.lbltxtHypervisor.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbltxtFeatureLevel
            // 
            this.lbltxtFeatureLevel.Location = new System.Drawing.Point(3, 174);
            this.lbltxtFeatureLevel.Name = "lbltxtFeatureLevel";
            this.lbltxtFeatureLevel.Size = new System.Drawing.Size(132, 14);
            this.lbltxtFeatureLevel.TabIndex = 20;
            this.lbltxtFeatureLevel.Text = "AMD64 Feature Level:";
            this.lbltxtFeatureLevel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblFeatureLevel
            // 
            this.lblFeatureLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFeatureLevel.Location = new System.Drawing.Point(141, 174);
            this.lblFeatureLevel.Name = "lblFeatureLevel";
            this.lblFeatureLevel.Size = new System.Drawing.Size(310, 14);
            this.lblFeatureLevel.TabIndex = 21;
            this.lblFeatureLevel.Text = "-";
            // 
            // CpuDetailsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblFeatureLevel);
            this.Controls.Add(this.lbltxtFeatureLevel);
            this.Controls.Add(this.lblHypervisor);
            this.Controls.Add(this.lbltxtHypervisor);
            this.Controls.Add(this.lblProcessorSignature);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblProcessorType);
            this.Controls.Add(this.lbltxtProcessorType);
            this.Controls.Add(this.lblStepping);
            this.Controls.Add(this.lbltxtStepping);
            this.Controls.Add(this.lblModel);
            this.Controls.Add(this.lbltxtModel);
            this.Controls.Add(this.lblFamily);
            this.Controls.Add(this.lbltxtFamily);
            this.Controls.Add(this.lbltxtProcessorSignature);
            this.Controls.Add(this.lbltxtDescription);
            this.Controls.Add(this.lblBrand);
            this.Controls.Add(this.lbltxtBrand);
            this.Controls.Add(this.lblVendorId);
            this.Controls.Add(this.lbltxtVendorId);
            this.Controls.Add(this.lblCpuVendor);
            this.Controls.Add(this.lbltxtCpuVendor);
            this.Name = "CpuDetailsControl";
            this.Size = new System.Drawing.Size(480, 320);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbltxtCpuVendor;
        private System.Windows.Forms.Label lblCpuVendor;
        private System.Windows.Forms.Label lblVendorId;
        private System.Windows.Forms.Label lbltxtVendorId;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.Label lbltxtBrand;
        private System.Windows.Forms.Label lbltxtDescription;
        private System.Windows.Forms.Label lbltxtProcessorSignature;
        private System.Windows.Forms.Label lblFamily;
        private System.Windows.Forms.Label lbltxtFamily;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.Label lbltxtModel;
        private System.Windows.Forms.Label lblStepping;
        private System.Windows.Forms.Label lbltxtStepping;
        private System.Windows.Forms.Label lblProcessorType;
        private System.Windows.Forms.Label lbltxtProcessorType;
        private System.Windows.Forms.TextBox lblDescription;
        private System.Windows.Forms.TextBox lblProcessorSignature;
        private System.Windows.Forms.TextBox lblHypervisor;
        private System.Windows.Forms.Label lbltxtHypervisor;
        private System.Windows.Forms.Label lbltxtFeatureLevel;
        private System.Windows.Forms.Label lblFeatureLevel;
    }
}
