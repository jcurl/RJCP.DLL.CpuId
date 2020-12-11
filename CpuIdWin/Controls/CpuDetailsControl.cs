namespace RJCP.Diagnostics.CpuIdWin.Controls
{
    using System.Windows.Forms;
    using Intel;

    public partial class CpuDetailsControl : UserControl
    {
        public CpuDetailsControl(ICpuId cpuId)
        {
            InitializeComponent();

            lblCpuVendor.Text = cpuId.CpuVendor.ToString();
            lblVendorId.Text = cpuId.VendorId;
            lblDescription.Text = cpuId.Description;

            if (cpuId is ICpuIdX86 x86Cpu) {
                lblBrand.Text = x86Cpu.BrandString;
                lblProcessorSignature.Text = string.Format("{0:X}h", x86Cpu.ProcessorSignature);
                lblFamily.Text = string.Format("{0:X}h", x86Cpu.Family);
                lblModel.Text = string.Format("{0:X}h", x86Cpu.Model);
                lblStepping.Text = string.Format("{0}", x86Cpu.Stepping);
                lblProcessorType.Text = string.Format("{0}", x86Cpu.ProcessorType);
            }
        }
    }
}
