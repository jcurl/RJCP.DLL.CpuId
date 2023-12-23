#ifndef RJCP_DIAGNOSTICS_CPUID_ICPUID_H
#define RJCP_DIAGNOSTICS_CPUID_ICPUID_H

#include "cpuid/CpuIdRegister.h"

#include <cstdint>
#include <optional>

namespace rjcp {
namespace diagnostics {
namespace cpuid {

/// <summary>
/// Interface for getting CPUID instruction data.
/// </summary>
class ICpuId {
 protected:
  ICpuId() = default;
  ICpuId(const ICpuId&) = default;
  ICpuId(ICpuId&&) = default;
  auto operator=(const ICpuId&) -> ICpuId& = default;
  auto operator=(ICpuId&&) -> ICpuId& = default;

 public:
  /// <summary>
  /// Gets the CPU identifier for a specific EAX, ECX.
  /// </summary>
  /// <param name="eax">The value of EAX to query for.</param>
  /// <param name="ecx">The value of ECX to query for.</param>
  /// <returns>The result of the CPUID instruction.</returns>
  virtual auto GetCpuId(std::uint32_t eax, std::uint32_t ecx) const noexcept -> const std::optional<CpuIdRegister> = 0;

  /// <summary>
  /// Finalizes an instance of the <see cref="ICpuId"/> class.
  /// </summary>
  virtual ~ICpuId() noexcept = default;
};

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
#endif
