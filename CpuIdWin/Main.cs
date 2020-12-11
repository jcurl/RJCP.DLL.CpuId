using System;
using System.Windows.Forms;

namespace RJCP.Diagnostics.CpuIdWin
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            GetLocal();
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

            ICpuId cpuId;
            try {
                cpuId = Global.CpuXmlFactory.Create(dlg.FileName);
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
            cpuIdTree1.Cores.Add(cpuId);
        }

        private void mnuFileOpenLocal_Click(object sender, EventArgs e)
        {
            GetLocal();
        }

        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            if (cpuIdTree1.Cores.Count == 0) return;
            ICpuId cpuId = cpuIdTree1.Cores[0];

            if (cpuId is Intel.GenericIntelCpuBase x86cpu) {
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
                    x86cpu.Save(dlg.FileName);
                } catch (Exception ex) {
                    string message = string.Format("Error saving file: {0}", ex.Message);
                    MessageBox.Show(message, "Error Saving File");
                }
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
                ICpuId cpuId = Global.CpuFactory.Create();
                cpuIdTree1.Cores.Add(cpuId);
            } catch (Exception ex) {
                string message = string.Format("An error occurred getting CPU information:\n{0}", ex.Message);
                MessageBox.Show(message, "CpuIdWin", MessageBoxButtons.OK);
                cpuIdTree1.Cores.Clear();
            }
        }
    }
}
