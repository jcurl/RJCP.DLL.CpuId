namespace RJCP.Diagnostics.CpuIdWin.Controls
{
    using System;
    using System.Text;
    using System.Windows.Forms;
    using CpuId;
    using CpuId.Intel;

    public partial class CpuDetailsControl : UserControl
    {
        private const int HypervisorFunction = 0x40000000;

        public CpuDetailsControl(ICpuId cpuId)
        {
            ThrowHelper.ThrowIfNull(cpuId);

            InitializeComponent();

            lblCpuVendor.Text = cpuId.CpuVendor.ToString();
            lblVendorId.Text = cpuId.VendorId;
            lblDescription.Text = cpuId.Description;

            if (cpuId is ICpuIdX86 x86Cpu) {
                lblBrand.Text = x86Cpu.BrandString;
                lblProcessorSignature.Text = $"{x86Cpu.ProcessorSignature:X}h";
                lblFamily.Text = $"{x86Cpu.Family:X}h";
                lblModel.Text = $"{x86Cpu.Model:X}h";
                lblStepping.Text = $"{x86Cpu.Stepping}";
                lblProcessorType.Text = $"{x86Cpu.ProcessorType}";
                lblFeatureLevel.Text = $"{x86Cpu.FeatureLevel}";

                lblHypervisor.Text = GetHypervisor(x86Cpu) ?? "-";
            }
        }

        private static string GetHypervisor(ICpuIdX86 x86Cpu)
        {
            if (!x86Cpu.Features["HYPERVISOR"].Value) return null;

            CpuIdRegister hyper = x86Cpu.Registers.GetCpuId(HypervisorFunction, 0);
            return GetHypervisor(hyper);
        }

        private static string GetHypervisor(CpuIdRegister register)
        {
            if (register == null) return null;

            StringBuilder description = new();
            int ebx = register.Result[1];
            int ecx = register.Result[2];
            int edx = register.Result[3];

            Append(description, ebx & 0xFF);
            Append(description, (ebx >> 8) & 0xFF);
            Append(description, (ebx >> 16) & 0xFF);
            Append(description, (ebx >> 24) & 0xFF);
            Append(description, ecx & 0xFF);
            Append(description, (ecx >> 8) & 0xFF);
            Append(description, (ecx >> 16) & 0xFF);
            Append(description, (ecx >> 24) & 0xFF);
            Append(description, edx & 0xFF);
            Append(description, (edx >> 8) & 0xFF);
            Append(description, (edx >> 16) & 0xFF);
            Append(description, (edx >> 24) & 0xFF);
            return description.ToString();
        }

        private static void Append(StringBuilder brand, int value)
        {
            if (value == 0) return;
            brand.Append((char)value);
        }
    }
}
