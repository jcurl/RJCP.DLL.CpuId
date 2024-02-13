namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    /// <summary>
    /// Results of a call to the processor's CPU identification instruction.
    /// </summary>
    [DebuggerDisplay("Function={Function}; SubFunction={SubFunction}")]
    public class CpuIdRegister
    {
        internal CpuIdRegister(int function, int subfunction, IEnumerable<int> result)
        {
            ThrowHelper.ThrowIfNull(result);

            Function = function;
            SubFunction = subfunction;
            List<int> results = new(result);
            Result = new ReadOnlyCollection<int>(results);
        }

        /// <summary>
        /// Gets the function number for the result.
        /// </summary>
        /// <value>The function number for the result.</value>
        public int Function { get; private set; }

        /// <summary>
        /// Gets the optional sub function for the result.
        /// </summary>
        /// <value>The optional sub function for the result.</value>
        public int SubFunction { get; private set; }

        /// <summary>
        /// A list of the results from the CPU identification instruction.
        /// </summary>
        /// <value>The result of the CPU identification instruction.</value>
        public IList<int> Result { get; private set; }
    }
}
