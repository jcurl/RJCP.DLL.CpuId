#ifndef RJCP_DIAGNOSTICS_CPUID_CPUIDNATIVE_H
#define RJCP_DIAGNOSTICS_CPUID_CPUIDNATIVE_H

#include "cpuid/ICpuId.h"
#include "native/win32/Thread.h"

#include <memory>

namespace rjcp {
namespace diagnostics {
namespace cpuid {

/// <summary>
/// Obtain CPUID information by executing the instruction on an Intel platform.
/// </summary>
/// <remarks>
/// Instantiating this class will pin the current thread to a specific CPU core. Ensure to destroy the instance before
/// creating a new instance on the same thread. i.e. don't copy at all, or move to another thread. The reason for doing
/// this at construction and not when obtaining the CPUID information is for performance (pin the thread first, then get
/// all the values needed).
/// </remarks>
class CpuIdNative : public ICpuId {
 public:
  /// <summary>
  /// Initializes a new instance of the <see cref="CpuIdNative"/> class.
  /// </summary>
  /// <remarks>Gets the CPUID information for the current core.</remarks>
  CpuIdNative() noexcept;

  /// <summary>
  /// Initializes a new instance of the <see cref="CpuIdNative"/> class.
  /// </summary>
  /// <param name="cpunum">The number to get the information for.</param>
  /// <remarks>Gets the CPUID information for the core given by <paramref name="cpunum"/>.</remarks>
  explicit CpuIdNative(unsigned int cpunum) noexcept;

  /// <summary>
  /// Gets the CPU identifier for a specific EAX, ECX.
  /// </summary>
  /// <param name="eax">The value of EAX to query for.</param>
  /// <param name="ecx">The value of ECX to query for.</param>
  /// <returns>The result of the CPUID instruction.</returns>
  auto GetCpuId(std::uint32_t eax, std::uint32_t ecx) const noexcept -> const std::optional<CpuIdRegister> override;

 private:
  native::win32::Thread thread_;
};

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
#endif
