#ifndef RJCP_DIAGNOSTICS_CPUID_ICPUIDDUMP_H
#define RJCP_DIAGNOSTICS_CPUID_ICPUIDDUMP_H

#include "cpuid/CpuIdRegister.h"
#include "cpuid/ICpuId.h"

#include <vector>

namespace rjcp {
namespace diagnostics {
namespace cpuid {

/// <summary>
/// Interface to get a complete CPUID dump for different CPU manufacturers.
/// </summary>
class ICpuIdDump {
 protected:
  ICpuIdDump() = default;
  ICpuIdDump(const ICpuIdDump&) = default;
  ICpuIdDump(ICpuIdDump&&) = default;
  auto operator=(const ICpuIdDump&) -> ICpuIdDump& = default;
  auto operator=(ICpuIdDump&&) -> ICpuIdDump& = default;

 public:
  /// <summary>
  /// Captures a complete CPUID for a specific CPU manufacturer.
  /// </summary>
  /// <param name="cpuid">The CPUID object to get the dumps.</param>
  /// <param name="info">An array of <see name="cpuidinfo"/> objects to write to.</param>
  /// <param name="bytes">The size of the complete buffer given by <paramref name="info"/> in bytes.</param>
  /// <returns>The number of elements written to <paramref name="info"/>.</returns>
  virtual auto CpuIdDump(ICpuId& cpuid) -> std::vector<CpuIdRegister> = 0;

  /// <summary>
  /// Finalizes an instance of the <see cref="ICpuIdDump"/> class.
  /// </summary>
  virtual ~ICpuIdDump() noexcept = default;
};

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
#endif
