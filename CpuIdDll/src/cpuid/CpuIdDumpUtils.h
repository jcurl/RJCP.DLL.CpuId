#ifndef RJCP_DIAGNOSTICS_CPUID_CPUIDDUMPUTILS_H
#define RJCP_DIAGNOSTICS_CPUID_CPUIDDUMPUTILS_H

#include "cpuid/CpuIdRegister.h"
#include "cpuid/ICpuId.h"

#include <vector>

namespace rjcp {
namespace diagnostics {
namespace cpuid {
namespace utils {

/// <summary>
/// Dump all relevant CPUID fields into the vector provided.
/// </summary>
/// <param name="cpuid">The object to obtain the CPUID information for.</param>
/// <param name="registers">The vector to write the results to.</param>
/// <param name="base">The base CPUID which in EAX returns the number of leaves.</param>
template<std::uint32_t B>
auto CpuIdDump(ICpuId& cpuid, std::vector<CpuIdRegister>& registers) -> void {
  std::uint32_t leaf{B};
  std::uint32_t maxleaf{B};

  while (true) {
    auto cpuidreg = cpuid.GetCpuId(leaf, 0);
    if (cpuidreg) {
      registers.push_back(*cpuidreg);
      switch (leaf) {
        case B: {
          // Maximum number of leaves and branding.
          maxleaf = cpuidreg->Eax();
          if ((maxleaf & 0xFF000000) != (leaf & 0xFF000000)) return;
          break;
        }
        default:
          break;
      }
    }

    if (leaf >= maxleaf) return;
    leaf++;
  }
}

}  // namespace utils
}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
#endif
