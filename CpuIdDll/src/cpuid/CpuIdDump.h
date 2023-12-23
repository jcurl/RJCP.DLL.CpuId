#ifndef RJCP_DIAGNOSTICS_CPUID_CPUIDDUMP_H
#define RJCP_DIAGNOSTICS_CPUID_CPUIDDUMP_H

#include "cpuid/CpuIdRegister.h"
#include "cpuid/ICpuId.h"

#include <vector>

namespace rjcp {
namespace diagnostics {
namespace cpuid {

/// <summary>
/// Dump all relevant CPUID fields into the structure provided.
/// </summary>
/// <param name="cpuid">The object to obtain the CPUID information for.</param>
/// <returns>A vector of all the elements queried.</returns>
auto CpuIdDump(ICpuId& cpuid) -> std::vector<CpuIdRegister>;

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
#endif
