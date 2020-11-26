using System;
using System.Windows.Forms;

namespace RJCP.Diagnostics.CpuIdWin
{
    public partial class Main : Form
    {
        private ICpuId m_CpuId;

        public Main()
        {
            InitializeComponent();
            m_CpuId = Global.CpuFactory.Create();

            int lvwWidth = (lstCpuIdRegisters.Width) / 6 - 2;
            lstCpuIdRegisters.Columns[0].Width = lvwWidth;
            lstCpuIdRegisters.Columns[1].Width = lvwWidth;
            lstCpuIdRegisters.Columns[2].Width = lvwWidth;
            lstCpuIdRegisters.Columns[3].Width = lvwWidth;
            lstCpuIdRegisters.Columns[4].Width = lvwWidth;
            lstCpuIdRegisters.Columns[5].Width = lvwWidth;

            UpdateForm();
        }

        private void mnuFileNew_Click(object sender, EventArgs e)
        {
            m_CpuId = null;
            UpdateForm();
        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog {
                CheckFileExists = true,
                AutoUpgradeEnabled = true,
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
                Title = "Open CPUID information"
            };
            dlg.ShowDialog();

            if (string.IsNullOrWhiteSpace(dlg.FileName)) return;
            try {
                m_CpuId = Global.CpuXmlFactory.Create(dlg.FileName);
            } catch (Exception ex) {
                string message = string.Format("Error opening file: {0}", ex.Message);
                MessageBox.Show(message, "Error opening File");
                return;
            }

            UpdateForm();
        }

        private void mnuFileOpenLocal_Click(object sender, EventArgs e)
        {
            m_CpuId = Global.CpuFactory.Create();
            UpdateForm();
        }

        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            if (m_CpuId is Intel.GenericIntelCpuBase x86cpu) {
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

        private void UpdateForm()
        {
            ShowCpuVendor();
            if (m_CpuId != null) {
                if (m_CpuId is Intel.GenuineIntelCpu intelCpu)
                    ShowCpuVendor(intelCpu);
                else if (m_CpuId is Intel.GenericIntelCpu genIntelCpu)
                    ShowCpuVendor(genIntelCpu);
                else if (m_CpuId is Intel.ICpuIdX86 x86Cpu)
                    ShowCpuVendor(x86Cpu);
                else
                    ShowCpuVendor(m_CpuId);
            }
        }

        private void ShowCpuVendor()
        {
            lblVendor.Text = "-";
            lblCpuName.Text = "-";
            HideTab(tbcCpuId, tabFeatures);
            HideTab(tbcCpuId, tabX86);
        }

        private void ShowCpuVendor(ICpuId cpu)
        {
            UpdateFeatures(cpu);
            ShowTab(tbcCpuId, tabMain, tabFeatures);
        }

        private void ShowCpuVendor(Intel.ICpuIdX86 cpu)
        {
            lblVendor.Text = cpu.VendorId ?? "-";
            lblCpuName.Text = cpu.Description ?? "-";

            lblFamily.Text = string.Format("{0:X}", cpu.Family);
            lblModel.Text = string.Format("{0:X}", cpu.Model);
            lblStepping.Text = string.Format("{0:X}", cpu.Stepping);
            lblType.Text = string.Format("{0:X}", cpu.ProcessorType);

            ShowCpuVendor((ICpuId)cpu);
            DumpRegisters(cpu);
            ShowTab(tbcCpuId, tabMain, tabX86);
        }

        private void ShowCpuVendor(Intel.GenericIntelCpu cpu)
        {
            ShowCpuVendor((Intel.ICpuIdX86)cpu);
        }

        private void ShowCpuVendor(Intel.GenuineIntelCpu cpu)
        {
            ShowCpuVendor((Intel.ICpuIdX86)cpu);
        }

        private void AddRegister(int eax, int ecx, int reax, int rebx, int recx, int redx, ListViewGroup lvg)
        {
            ListViewItem lvw = new ListViewItem(new string[] { eax.ToString("X8"), ecx.ToString("X8"),
                reax.ToString("X8"), rebx.ToString("X8"), recx.ToString("X8"), redx.ToString("X8")}, lvg);
            lstCpuIdRegisters.Items.Add(lvw);
        }

        private void DumpRegisters(Intel.ICpuIdX86 cpu)
        {
            lstCpuIdRegisters.Items.Clear();

            lblSignature.Text = cpu.ProcessorSignature.ToString("X8");
            lstCpuIdRegisters.View = View.Details;

            ListViewGroup lvg;
            ListViewGroup lvgs = lstCpuIdRegisters.Groups["lvgStandardFunctions"];
            ListViewGroup lvge = lstCpuIdRegisters.Groups["lvgExtendedFunctions"];
            foreach (Intel.CpuIdRegister reg in cpu.Registers) {
                if ((reg.Function & (1 << 31)) == 0) {
                    lvg = lvgs;
                } else {
                    lvg = lvge;
                }

                AddRegister(unchecked(reg.Function),
                    unchecked(reg.SubFunction),
                    unchecked(reg.Result[0]),
                    unchecked(reg.Result[1]),
                    unchecked(reg.Result[2]),
                    unchecked(reg.Result[3]), lvg);
            }
        }

        private void UpdateFeatures(ICpuId cpu)
        {
            lstCpuFeatures.Items.Clear();
            foreach (string feature in cpu.Features) {
                string text;
                string desc = cpu.Features.Description(feature);
                if (desc != null) {
                    text = string.Format("{0}: {1}", feature, desc);
                } else {
                    text = feature;
                }
                lstCpuFeatures.Items.Add(text, cpu.Features[feature]);
            }
        }

        private void ShowTab(TabControl tabControl, TabPage prev, TabPage page)
        {
            if (!tabControl.TabPages.Contains(page)) {
                if (prev == null) {
                    tabControl.TabPages.Insert(0, page);
                } else {
                    int pos = tabControl.TabPages.IndexOf(prev);

                    // https://stackoverflow.com/questions/1532301/visual-studio-tabcontrol-tabpages-insert-not-working
                    IntPtr h = tabControl.Handle;
                    if (!h.Equals(IntPtr.Zero)) {
                        tabControl.TabPages.Insert(pos + 1, page);
                    }
                }
            }
        }

        private void HideTab(TabControl tabControl, TabPage page)
        {
            if (tabControl.TabPages.Contains(page))
                tabControl.TabPages.Remove(page);
        }
    }
}
