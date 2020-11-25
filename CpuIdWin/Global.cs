namespace RJCP.Diagnostics.CpuIdWin
{
    public static class Global
    {
        public readonly static ICpuIdFactory CpuFactory = new CpuIdFactory();

        public readonly static CpuIdXmlFactory CpuXmlFactory = new CpuIdXmlFactory();
    }
}
