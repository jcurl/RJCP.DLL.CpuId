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
        /// <remarks>
        /// If <see cref="IsOnline"/>, querying a register that hasn't been cached will cause the register to be queried
        /// directly from the CPU from the current thread. If not <see cref="IsOnline"/>, this function can return
        /// <see langword="null"/>.
        /// </remarks>
        CpuIdRegister GetCpuId(int function, int subfunction);

        /// <summary>
        /// Gets a value indicating whether this instance is online.
        /// </summary>
        /// <value>Returns <see langword="true"/> if this instance is online; otherwise, <see langword="false"/>.</value>
        bool IsOnline { get; }
    }
}
