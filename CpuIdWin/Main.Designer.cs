namespace RJCP.Diagnostics.CpuIdWin
{
    partial class Main
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

        #region Windows Form Designer generated code

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.tbcCpuId = new System.Windows.Forms.TabControl();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.lblCpuName = new System.Windows.Forms.Label();
            this.lbltxtCpuName = new System.Windows.Forms.Label();
            this.lblVendor = new System.Windows.Forms.Label();
            this.lbltxtVendor = new System.Windows.Forms.Label();
            this.tabX86 = new System.Windows.Forms.TabPage();
            this.lblType = new System.Windows.Forms.Label();
            this.lbltxtType = new System.Windows.Forms.Label();
            this.lblStepping = new System.Windows.Forms.Label();
            this.lbltxtStepping = new System.Windows.Forms.Label();
            this.lblModel = new System.Windows.Forms.Label();
            this.lbltxtModel = new System.Windows.Forms.Label();
            this.lblFamily = new System.Windows.Forms.Label();
            this.lbltxtFamily = new System.Windows.Forms.Label();
            this.lblSignature = new System.Windows.Forms.Label();
            this.lbltxtSignature = new System.Windows.Forms.Label();
            this.lbltxtCpuIdValues = new System.Windows.Forms.Label();
            this.lstCpuIdRegisters = new System.Windows.Forms.ListView();
            this.colInEax = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInEcx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOutEax = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOutEbx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOutEcx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOutEdx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabFeatures = new System.Windows.Forms.TabPage();
            this.lstCpuFeatures = new System.Windows.Forms.CheckedListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileOpenLocal = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEditSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpContents = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tbcCpuId.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabX86.SuspendLayout();
            this.tabFeatures.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbcCpuId
            // 
            this.tbcCpuId.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcCpuId.Controls.Add(this.tabMain);
            this.tbcCpuId.Controls.Add(this.tabX86);
            this.tbcCpuId.Controls.Add(this.tabFeatures);
            this.tbcCpuId.Location = new System.Drawing.Point(12, 27);
            this.tbcCpuId.Name = "tbcCpuId";
            this.tbcCpuId.SelectedIndex = 0;
            this.tbcCpuId.Size = new System.Drawing.Size(491, 445);
            this.tbcCpuId.TabIndex = 0;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.lblCpuName);
            this.tabMain.Controls.Add(this.lbltxtCpuName);
            this.tabMain.Controls.Add(this.lblVendor);
            this.tabMain.Controls.Add(this.lbltxtVendor);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(483, 419);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "Main";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // lblCpuName
            // 
            this.lblCpuName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCpuName.Location = new System.Drawing.Point(96, 32);
            this.lblCpuName.Name = "lblCpuName";
            this.lblCpuName.Size = new System.Drawing.Size(381, 16);
            this.lblCpuName.TabIndex = 5;
            this.lblCpuName.Text = "-";
            // 
            // lbltxtCpuName
            // 
            this.lbltxtCpuName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbltxtCpuName.Location = new System.Drawing.Point(6, 32);
            this.lbltxtCpuName.Name = "lbltxtCpuName";
            this.lbltxtCpuName.Size = new System.Drawing.Size(84, 16);
            this.lbltxtCpuName.TabIndex = 4;
            this.lbltxtCpuName.Text = "Brand Name: ";
            this.lbltxtCpuName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblVendor
            // 
            this.lblVendor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVendor.Location = new System.Drawing.Point(96, 16);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(381, 16);
            this.lblVendor.TabIndex = 1;
            this.lblVendor.Text = "-";
            // 
            // lbltxtVendor
            // 
            this.lbltxtVendor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbltxtVendor.Location = new System.Drawing.Point(6, 16);
            this.lbltxtVendor.Name = "lbltxtVendor";
            this.lbltxtVendor.Size = new System.Drawing.Size(84, 16);
            this.lbltxtVendor.TabIndex = 0;
            this.lbltxtVendor.Text = "Vendor: ";
            this.lbltxtVendor.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tabX86
            // 
            this.tabX86.Controls.Add(this.lblType);
            this.tabX86.Controls.Add(this.lbltxtType);
            this.tabX86.Controls.Add(this.lblStepping);
            this.tabX86.Controls.Add(this.lbltxtStepping);
            this.tabX86.Controls.Add(this.lblModel);
            this.tabX86.Controls.Add(this.lbltxtModel);
            this.tabX86.Controls.Add(this.lblFamily);
            this.tabX86.Controls.Add(this.lbltxtFamily);
            this.tabX86.Controls.Add(this.lblSignature);
            this.tabX86.Controls.Add(this.lbltxtSignature);
            this.tabX86.Controls.Add(this.lbltxtCpuIdValues);
            this.tabX86.Controls.Add(this.lstCpuIdRegisters);
            this.tabX86.Location = new System.Drawing.Point(4, 22);
            this.tabX86.Name = "tabX86";
            this.tabX86.Size = new System.Drawing.Size(483, 419);
            this.tabX86.TabIndex = 3;
            this.tabX86.Text = "80x86";
            this.tabX86.UseVisualStyleBackColor = true;
            // 
            // lblType
            // 
            this.lblType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblType.Location = new System.Drawing.Point(96, 88);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(381, 16);
            this.lblType.TabIndex = 29;
            this.lblType.Text = "-";
            // 
            // lbltxtType
            // 
            this.lbltxtType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbltxtType.Location = new System.Drawing.Point(6, 88);
            this.lbltxtType.Name = "lbltxtType";
            this.lbltxtType.Size = new System.Drawing.Size(84, 16);
            this.lbltxtType.TabIndex = 28;
            this.lbltxtType.Text = "Proc Type: ";
            this.lbltxtType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblStepping
            // 
            this.lblStepping.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStepping.Location = new System.Drawing.Point(96, 72);
            this.lblStepping.Name = "lblStepping";
            this.lblStepping.Size = new System.Drawing.Size(381, 16);
            this.lblStepping.TabIndex = 27;
            this.lblStepping.Text = "-";
            // 
            // lbltxtStepping
            // 
            this.lbltxtStepping.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbltxtStepping.Location = new System.Drawing.Point(6, 72);
            this.lbltxtStepping.Name = "lbltxtStepping";
            this.lbltxtStepping.Size = new System.Drawing.Size(84, 16);
            this.lbltxtStepping.TabIndex = 26;
            this.lbltxtStepping.Text = "Stepping: ";
            this.lbltxtStepping.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblModel
            // 
            this.lblModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblModel.Location = new System.Drawing.Point(96, 56);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(381, 16);
            this.lblModel.TabIndex = 25;
            this.lblModel.Text = "-";
            // 
            // lbltxtModel
            // 
            this.lbltxtModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbltxtModel.Location = new System.Drawing.Point(6, 56);
            this.lbltxtModel.Name = "lbltxtModel";
            this.lbltxtModel.Size = new System.Drawing.Size(84, 16);
            this.lbltxtModel.TabIndex = 24;
            this.lbltxtModel.Text = "Model: ";
            this.lbltxtModel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblFamily
            // 
            this.lblFamily.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFamily.Location = new System.Drawing.Point(96, 40);
            this.lblFamily.Name = "lblFamily";
            this.lblFamily.Size = new System.Drawing.Size(381, 16);
            this.lblFamily.TabIndex = 23;
            this.lblFamily.Text = "-";
            // 
            // lbltxtFamily
            // 
            this.lbltxtFamily.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbltxtFamily.Location = new System.Drawing.Point(6, 40);
            this.lbltxtFamily.Name = "lbltxtFamily";
            this.lbltxtFamily.Size = new System.Drawing.Size(84, 16);
            this.lbltxtFamily.TabIndex = 22;
            this.lbltxtFamily.Text = "Family: ";
            this.lbltxtFamily.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSignature
            // 
            this.lblSignature.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSignature.Location = new System.Drawing.Point(96, 16);
            this.lblSignature.Name = "lblSignature";
            this.lblSignature.Size = new System.Drawing.Size(381, 16);
            this.lblSignature.TabIndex = 21;
            this.lblSignature.Text = "-";
            // 
            // lbltxtSignature
            // 
            this.lbltxtSignature.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbltxtSignature.Location = new System.Drawing.Point(6, 16);
            this.lbltxtSignature.Name = "lbltxtSignature";
            this.lbltxtSignature.Size = new System.Drawing.Size(84, 16);
            this.lbltxtSignature.TabIndex = 20;
            this.lbltxtSignature.Text = "Signature: ";
            this.lbltxtSignature.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbltxtCpuIdValues
            // 
            this.lbltxtCpuIdValues.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbltxtCpuIdValues.Location = new System.Drawing.Point(3, 114);
            this.lbltxtCpuIdValues.Name = "lbltxtCpuIdValues";
            this.lbltxtCpuIdValues.Size = new System.Drawing.Size(84, 13);
            this.lbltxtCpuIdValues.TabIndex = 19;
            this.lbltxtCpuIdValues.Text = "CPUID Values";
            // 
            // lstCpuIdRegisters
            // 
            this.lstCpuIdRegisters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstCpuIdRegisters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstCpuIdRegisters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colInEax,
            this.colInEcx,
            this.colOutEax,
            this.colOutEbx,
            this.colOutEcx,
            this.colOutEdx});
            this.lstCpuIdRegisters.Cursor = System.Windows.Forms.Cursors.Default;
            this.lstCpuIdRegisters.FullRowSelect = true;
            this.lstCpuIdRegisters.GridLines = true;
            listViewGroup1.Header = "Standard Functions";
            listViewGroup1.Name = "lvgStandardFunctions";
            listViewGroup2.Header = "Intel Xeon Phi Functions";
            listViewGroup2.Name = "lvgPhiFunctions";
            listViewGroup3.Header = "Hypervisor Functions";
            listViewGroup3.Name = "lvgHypervisorFunctions";
            listViewGroup4.Header = "Extended Functions";
            listViewGroup4.Name = "lvgExtendedFunctions";
            this.lstCpuIdRegisters.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4});
            this.lstCpuIdRegisters.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstCpuIdRegisters.HideSelection = false;
            this.lstCpuIdRegisters.Location = new System.Drawing.Point(0, 130);
            this.lstCpuIdRegisters.MultiSelect = false;
            this.lstCpuIdRegisters.Name = "lstCpuIdRegisters";
            this.lstCpuIdRegisters.Size = new System.Drawing.Size(480, 289);
            this.lstCpuIdRegisters.TabIndex = 18;
            this.lstCpuIdRegisters.UseCompatibleStateImageBehavior = false;
            this.lstCpuIdRegisters.View = System.Windows.Forms.View.Details;
            // 
            // colInEax
            // 
            this.colInEax.Text = "EAX";
            // 
            // colInEcx
            // 
            this.colInEcx.Text = "ECX";
            // 
            // colOutEax
            // 
            this.colOutEax.Text = "-> EAX";
            // 
            // colOutEbx
            // 
            this.colOutEbx.Text = "-> EBX";
            // 
            // colOutEcx
            // 
            this.colOutEcx.Text = "-> ECX";
            // 
            // colOutEdx
            // 
            this.colOutEdx.Text = "-> EDX";
            // 
            // tabFeatures
            // 
            this.tabFeatures.Controls.Add(this.lstCpuFeatures);
            this.tabFeatures.Location = new System.Drawing.Point(4, 22);
            this.tabFeatures.Name = "tabFeatures";
            this.tabFeatures.Padding = new System.Windows.Forms.Padding(3);
            this.tabFeatures.Size = new System.Drawing.Size(483, 419);
            this.tabFeatures.TabIndex = 2;
            this.tabFeatures.Text = "Features";
            this.tabFeatures.UseVisualStyleBackColor = true;
            // 
            // lstCpuFeatures
            // 
            this.lstCpuFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstCpuFeatures.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstCpuFeatures.FormattingEnabled = true;
            this.lstCpuFeatures.IntegralHeight = false;
            this.lstCpuFeatures.Location = new System.Drawing.Point(0, 0);
            this.lstCpuFeatures.Name = "lstCpuFeatures";
            this.lstCpuFeatures.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstCpuFeatures.Size = new System.Drawing.Size(483, 419);
            this.lstCpuFeatures.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.mnuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(515, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileNew,
            this.mnuFileOpen,
            this.mnuFileOpenLocal,
            this.toolStripSeparator,
            this.mnuFileSave,
            this.toolStripSeparator1,
            this.mnuFileExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuFileNew
            // 
            this.mnuFileNew.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileNew.Image")));
            this.mnuFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFileNew.Name = "mnuFileNew";
            this.mnuFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mnuFileNew.Size = new System.Drawing.Size(154, 22);
            this.mnuFileNew.Text = "&New";
            this.mnuFileNew.Click += new System.EventHandler(this.mnuFileNew_Click);
            // 
            // mnuFileOpen
            // 
            this.mnuFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileOpen.Image")));
            this.mnuFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFileOpen.Name = "mnuFileOpen";
            this.mnuFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnuFileOpen.Size = new System.Drawing.Size(154, 22);
            this.mnuFileOpen.Text = "&Open";
            this.mnuFileOpen.Click += new System.EventHandler(this.mnuFileOpen_Click);
            // 
            // mnuFileOpenLocal
            // 
            this.mnuFileOpenLocal.Name = "mnuFileOpenLocal";
            this.mnuFileOpenLocal.Size = new System.Drawing.Size(154, 22);
            this.mnuFileOpenLocal.Text = "Open Local";
            this.mnuFileOpenLocal.Click += new System.EventHandler(this.mnuFileOpenLocal_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(151, 6);
            // 
            // mnuFileSave
            // 
            this.mnuFileSave.Image = ((System.Drawing.Image)(resources.GetObject("mnuFileSave.Image")));
            this.mnuFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFileSave.Name = "mnuFileSave";
            this.mnuFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuFileSave.Size = new System.Drawing.Size(154, 22);
            this.mnuFileSave.Text = "&Save As";
            this.mnuFileSave.Click += new System.EventHandler(this.mnuFileSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(151, 6);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Size = new System.Drawing.Size(154, 22);
            this.mnuFileExit.Text = "E&xit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEditUndo,
            this.mnuEditRedo,
            this.toolStripSeparator3,
            this.mnuEditCut,
            this.mnuEditCopy,
            this.mnuEditPaste,
            this.toolStripSeparator4,
            this.mnuEditSelectAll});
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(39, 20);
            this.mnuEdit.Text = "&Edit";
            // 
            // mnuEditUndo
            // 
            this.mnuEditUndo.Enabled = false;
            this.mnuEditUndo.Name = "mnuEditUndo";
            this.mnuEditUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.mnuEditUndo.Size = new System.Drawing.Size(144, 22);
            this.mnuEditUndo.Text = "&Undo";
            // 
            // mnuEditRedo
            // 
            this.mnuEditRedo.Enabled = false;
            this.mnuEditRedo.Name = "mnuEditRedo";
            this.mnuEditRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.mnuEditRedo.Size = new System.Drawing.Size(144, 22);
            this.mnuEditRedo.Text = "&Redo";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
            // 
            // mnuEditCut
            // 
            this.mnuEditCut.Enabled = false;
            this.mnuEditCut.Image = ((System.Drawing.Image)(resources.GetObject("mnuEditCut.Image")));
            this.mnuEditCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuEditCut.Name = "mnuEditCut";
            this.mnuEditCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.mnuEditCut.Size = new System.Drawing.Size(144, 22);
            this.mnuEditCut.Text = "Cu&t";
            // 
            // mnuEditCopy
            // 
            this.mnuEditCopy.Enabled = false;
            this.mnuEditCopy.Image = ((System.Drawing.Image)(resources.GetObject("mnuEditCopy.Image")));
            this.mnuEditCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuEditCopy.Name = "mnuEditCopy";
            this.mnuEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mnuEditCopy.Size = new System.Drawing.Size(144, 22);
            this.mnuEditCopy.Text = "&Copy";
            // 
            // mnuEditPaste
            // 
            this.mnuEditPaste.Enabled = false;
            this.mnuEditPaste.Image = ((System.Drawing.Image)(resources.GetObject("mnuEditPaste.Image")));
            this.mnuEditPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuEditPaste.Name = "mnuEditPaste";
            this.mnuEditPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.mnuEditPaste.Size = new System.Drawing.Size(144, 22);
            this.mnuEditPaste.Text = "&Paste";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(141, 6);
            // 
            // mnuEditSelectAll
            // 
            this.mnuEditSelectAll.Enabled = false;
            this.mnuEditSelectAll.Name = "mnuEditSelectAll";
            this.mnuEditSelectAll.Size = new System.Drawing.Size(144, 22);
            this.mnuEditSelectAll.Text = "Select &All";
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelpContents,
            this.mnuHelpIndex,
            this.mnuHelpSearch,
            this.toolStripSeparator5,
            this.mnuHelpAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "&Help";
            // 
            // mnuHelpContents
            // 
            this.mnuHelpContents.Enabled = false;
            this.mnuHelpContents.Name = "mnuHelpContents";
            this.mnuHelpContents.Size = new System.Drawing.Size(122, 22);
            this.mnuHelpContents.Text = "&Contents";
            // 
            // mnuHelpIndex
            // 
            this.mnuHelpIndex.Enabled = false;
            this.mnuHelpIndex.Name = "mnuHelpIndex";
            this.mnuHelpIndex.Size = new System.Drawing.Size(122, 22);
            this.mnuHelpIndex.Text = "&Index";
            // 
            // mnuHelpSearch
            // 
            this.mnuHelpSearch.Enabled = false;
            this.mnuHelpSearch.Name = "mnuHelpSearch";
            this.mnuHelpSearch.Size = new System.Drawing.Size(122, 22);
            this.mnuHelpSearch.Text = "&Search";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
            // 
            // mnuHelpAbout
            // 
            this.mnuHelpAbout.Enabled = false;
            this.mnuHelpAbout.Name = "mnuHelpAbout";
            this.mnuHelpAbout.Size = new System.Drawing.Size(122, 22);
            this.mnuHelpAbout.Text = "&About...";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 484);
            this.Controls.Add(this.tbcCpuId);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Main";
            this.Text = "CpuID";
            this.tbcCpuId.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabX86.ResumeLayout(false);
            this.tabFeatures.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tbcCpuId;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.Label lblVendor;
        private System.Windows.Forms.Label lbltxtVendor;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuFileNew;
        private System.Windows.Forms.ToolStripMenuItem mnuFileOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem mnuFileSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuEditUndo;
        private System.Windows.Forms.ToolStripMenuItem mnuEditRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnuEditCut;
        private System.Windows.Forms.ToolStripMenuItem mnuEditCopy;
        private System.Windows.Forms.ToolStripMenuItem mnuEditPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mnuEditSelectAll;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuHelpContents;
        private System.Windows.Forms.ToolStripMenuItem mnuHelpIndex;
        private System.Windows.Forms.ToolStripMenuItem mnuHelpSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mnuHelpAbout;
        private System.Windows.Forms.Label lblCpuName;
        private System.Windows.Forms.Label lbltxtCpuName;
        private System.Windows.Forms.ToolStripMenuItem mnuFileOpenLocal;
        private System.Windows.Forms.TabPage tabFeatures;
        private System.Windows.Forms.CheckedListBox lstCpuFeatures;
        private System.Windows.Forms.TabPage tabX86;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lbltxtType;
        private System.Windows.Forms.Label lblStepping;
        private System.Windows.Forms.Label lbltxtStepping;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.Label lbltxtModel;
        private System.Windows.Forms.Label lblFamily;
        private System.Windows.Forms.Label lbltxtFamily;
        private System.Windows.Forms.Label lblSignature;
        private System.Windows.Forms.Label lbltxtSignature;
        private System.Windows.Forms.Label lbltxtCpuIdValues;
        private System.Windows.Forms.ListView lstCpuIdRegisters;
        private System.Windows.Forms.ColumnHeader colInEax;
        private System.Windows.Forms.ColumnHeader colInEcx;
        private System.Windows.Forms.ColumnHeader colOutEax;
        private System.Windows.Forms.ColumnHeader colOutEbx;
        private System.Windows.Forms.ColumnHeader colOutEcx;
        private System.Windows.Forms.ColumnHeader colOutEdx;
    }
}

