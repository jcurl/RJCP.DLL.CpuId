namespace RJCP.Diagnostics.CpuIdWin.Controls
{

    partial class CpuDumpControl
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Standard Functions", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Intel Xeon Phi Functions", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Hypervisor Functions", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Extended Functions", System.Windows.Forms.HorizontalAlignment.Left);
            this.lvwRegisters = new ThemeListView();
            this.hdrEAX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrECX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrPEAX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrPEBX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrPECX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrPEDX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvwRegisters
            // 
            this.lvwRegisters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwRegisters.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwRegisters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrEAX,
            this.hdrECX,
            this.hdrPEAX,
            this.hdrPEBX,
            this.hdrPECX,
            this.hdrPEDX});
            this.lvwRegisters.FullRowSelect = true;
            this.lvwRegisters.GridLines = true;
            listViewGroup1.Header = "Standard Functions";
            listViewGroup1.Name = "lvgStandardFunctions";
            listViewGroup2.Header = "Intel Xeon Phi Functions";
            listViewGroup2.Name = "lvgPhiFunctions";
            listViewGroup3.Header = "Hypervisor Functions";
            listViewGroup3.Name = "lvgHypervisorFunctions";
            listViewGroup4.Header = "Extended Functions";
            listViewGroup4.Name = "lvgExtendedFunctions";
            this.lvwRegisters.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4});
            this.lvwRegisters.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwRegisters.HideSelection = false;
            this.lvwRegisters.LabelWrap = false;
            this.lvwRegisters.Location = new System.Drawing.Point(3, 3);
            this.lvwRegisters.MultiSelect = false;
            this.lvwRegisters.Name = "lvwRegisters";
            this.lvwRegisters.Size = new System.Drawing.Size(474, 314);
            this.lvwRegisters.TabIndex = 0;
            this.lvwRegisters.UseCompatibleStateImageBehavior = false;
            this.lvwRegisters.View = System.Windows.Forms.View.Details;
            this.lvwRegisters.SizeChanged += new System.EventHandler(this.lvwRegisters_SizeChanged);
            // 
            // hdrEAX
            // 
            this.hdrEAX.Text = "EAX";
            // 
            // hdrECX
            // 
            this.hdrECX.Text = "EXC";
            // 
            // hdrPEAX
            // 
            this.hdrPEAX.Text = "-> EAX";
            // 
            // hdrPEBX
            // 
            this.hdrPEBX.Text = "-> EBX";
            // 
            // hdrPECX
            // 
            this.hdrPECX.Text = "-> ECX";
            // 
            // hdrPEDX
            // 
            this.hdrPEDX.Text = "-> EDX";
            // 
            // CpuDumpControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvwRegisters);
            this.Name = "CpuDumpControl";
            this.Size = new System.Drawing.Size(480, 320);
            this.ResumeLayout(false);

        }

        #endregion

        private ThemeListView lvwRegisters;
        private System.Windows.Forms.ColumnHeader hdrEAX;
        private System.Windows.Forms.ColumnHeader hdrECX;
        private System.Windows.Forms.ColumnHeader hdrPEAX;
        private System.Windows.Forms.ColumnHeader hdrPEBX;
        private System.Windows.Forms.ColumnHeader hdrPECX;
        private System.Windows.Forms.ColumnHeader hdrPEDX;
    }
}
