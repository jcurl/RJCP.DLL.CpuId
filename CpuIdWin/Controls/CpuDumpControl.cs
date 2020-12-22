namespace RJCP.Diagnostics.CpuIdWin.Controls
{
    using System.Windows.Forms;
    using CpuId.Intel;

    public partial class CpuDumpControl : UserControl
    {
        public CpuDumpControl(ICpuIdX86 cpuId)
        {
            InitializeComponent();

            ListViewGroup lvg;
            ListViewGroup lvgs = lvwRegisters.Groups["lvgStandardFunctions"];
            ListViewGroup lvgp = lvwRegisters.Groups["lvgPhiFunctions"];
            ListViewGroup lvgh = lvwRegisters.Groups["lvgHypervisorFunctions"];
            ListViewGroup lvge = lvwRegisters.Groups["lvgExtendedFunctions"];
            foreach (CpuIdRegister reg in cpuId.Registers) {
                if (((reg.Function >> 28) & 0xF) == 0) {
                    lvg = lvgs;
                } else if (((reg.Function >> 28) & 0xF) == 0x2) {
                    lvg = lvgp;
                } else if (((reg.Function >> 28) & 0xF) == 0x4) {
                    lvg = lvgh;
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

        private void AddRegister(int eax, int ecx, int reax, int rebx, int recx, int redx, ListViewGroup lvg)
        {
            ListViewItem lvw = new ListViewItem(
                new string[] {
                    eax.ToString("X8"),
                    ecx.ToString("X8"),
                    reax.ToString("X8"),
                    rebx.ToString("X8"),
                    recx.ToString("X8"),
                    redx.ToString("X8")
                }, lvg);
            lvwRegisters.Items.Add(lvw);
        }

        private void lvwRegisters_SizeChanged(object sender, System.EventArgs e)
        {
            int lvwWidth = (lvwRegisters.Width) / 6 - 3;
            SuspendLayout();
            foreach (ColumnHeader hdr in lvwRegisters.Columns) {
                hdr.Width = lvwWidth;
            }
            ResumeLayout();
        }
    }
}
