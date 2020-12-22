namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.Collections.Generic;

    /// <summary>
    /// Access to the CPU's Identification Registers.
    /// </summary>
    public interface ICpuRegisters : IEnumerable<CpuIdRegister>
    {
        /// <summary>
        /// Call the CPU Identification Register function.
        /// </summary>
        /// <param name="function">The function number.</param>
        /// <param name="subfunction">The sub-function number.</param>
        /// <returns>The results of the function call.</returns>
        CpuIdRegister GetCpuId(int function, int subfunction);
    }
}
