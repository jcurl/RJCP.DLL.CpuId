namespace RJCP.Diagnostics.CpuIdWin
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using CpuId;
    using Microsoft.Win32;

    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Font = SystemFonts.IconTitleFont;
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            GetLocal();
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.Window) {
                Font = SystemFonts.IconTitleFont;
            }
        }

        private void mnuFileNew_Click(object sender, EventArgs e)
        {
            cpuIdTree1.Cores.Clear();
        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog {
                CheckFileExists = true,
                AutoUpgradeEnabled = true,
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
                Title = "Open CPUID information"
            };
            DialogResult result = dlg.ShowDialog();
            if (result != DialogResult.OK) return;
            if (string.IsNullOrWhiteSpace(dlg.FileName)) return;

            IEnumerable<ICpuId> cpus;
            try {
                cpus = Global.CpuXmlFactory.CreateAll(dlg.FileName);
            } catch (Exception ex) {
#if DEBUG
                string message = string.Format("Error opening file: {0}", ex.ToString());
#else
                string message = string.Format("Error opening file: {0}", ex.Message);
#endif
                MessageBox.Show(message, "Error opening File");
                return;
            }

            cpuIdTree1.Cores.Clear();
            foreach (ICpuId cpu in cpus) {
                cpuIdTree1.Cores.Add(cpu);
            }
        }

        private void mnuFileOpenLocal_Click(object sender, EventArgs e)
        {
            GetLocal();
        }

        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            if (cpuIdTree1.Cores.Count == 0) return;

            SaveFileDialog dlg = new SaveFileDialog {
                AddExtension = true,
                AutoUpgradeEnabled = true,
                CheckPathExists = true,
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
                DefaultExt = "xml",
                OverwritePrompt = true,
                Title = "Save CPUID information",
                FileName = Environment.MachineName.ToLowerInvariant(),
                ValidateNames = true
            };
            DialogResult result = dlg.ShowDialog();
            if (result != DialogResult.OK) return;
            if (string.IsNullOrWhiteSpace(dlg.FileName)) return;

            try {
                CpuIdXmlFactory.Save(dlg.FileName, cpuIdTree1.Cores);
            } catch (Exception ex) {
                string message = string.Format("Error saving file: {0}", ex.Message);
                MessageBox.Show(message, "Error Saving File");
            }
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GetLocal()
        {
            cpuIdTree1.Cores.Clear();
            try {
                IEnumerable<ICpuId> cpus = Global.CpuFactory.CreateAll();
                foreach (ICpuId cpu in cpus) {
                    cpuIdTree1.Cores.Add(cpu);
                }
            } catch (Exception ex) {
                string message = string.Format("An error occurred getting CPU information:\n{0}", ex.Message);
                MessageBox.Show(message, "CpuIdWin", MessageBoxButtons.OK);
                cpuIdTree1.Cores.Clear();
            }
        }
    }
}
