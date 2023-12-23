#ifndef RJCP_DIAGNOSTICS_CPUID_ICPUIDNATIVEFACTORY_H
#define RJCP_DIAGNOSTICS_CPUID_ICPUIDNATIVEFACTORY_H

#include "cpuid/CpuIdNativeConfig.h"
#include "cpuid/ICpuIdFactory.h"

#include <memory>

namespace rjcp {
namespace diagnostics {
namespace cpuid {

/// <summary>
/// A factory for creating <see cref="CpuIdNative"/> objects.
/// </summary>
class CpuIdNativeFactory : public ICpuIdFactory {
 public:
  /// <summary>
  /// Makes the <see cref="ICpuId"/> object.
  /// </summary>
  /// <returns>An object that knows how to get CPUID information for the CPU which is being called.</returns>
  auto MakeCpuId() noexcept -> std::unique_ptr<ICpuId> override;

  /// <summary>
  /// Makes the <see cref="ICpuId"/> object.
  /// </summary>
  /// <param name="cpunum">The CPU number from the Operating System to create for.</param>
  /// <returns>An object that knows how to get CPUID information for the <paramref name="cpunum"/>.</returns>
  auto MakeCpuId(unsigned int cpunum) noexcept -> std::unique_ptr<ICpuId> override;

  /// <summary>
  /// Returns the number of CPUs known.
  /// </summary>
  /// <returns>The number of CPUs known.</returns>
  auto CpuCount() const noexcept -> unsigned int override;
};

/// <summary>
/// Create a <see cref="CpuIdNativeFactory"/>.
/// </summary>
/// <param name="config">A default instantiation. There are no parameters at this time.</param>
/// <returns>An object derived from <see cref="ICpuIdFactory"/>.</returns>
auto MakeCpuIdFactory(const CpuIdNativeConfig& config) noexcept -> std::unique_ptr<ICpuIdFactory>;

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
#endif
