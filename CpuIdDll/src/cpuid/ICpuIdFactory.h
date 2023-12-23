#ifndef RJCP_DIAGNOSTICS_CPUID_ICPUIDFACTORY_H
#define RJCP_DIAGNOSTICS_CPUID_ICPUIDFACTORY_H

#include "cpuid/ICpuId.h"

#include <memory>

namespace rjcp {
namespace diagnostics {
namespace cpuid {

/// <summary>
/// Knows how to create a <see cref="ICpuId"/> object.
/// </summary>
class ICpuIdFactory {
 protected:
  ICpuIdFactory() = default;
  ICpuIdFactory(const ICpuIdFactory&) = default;
  ICpuIdFactory(ICpuIdFactory&&) = default;
  auto operator=(const ICpuIdFactory&) -> ICpuIdFactory& = default;
  auto operator=(ICpuIdFactory&&) -> ICpuIdFactory& = default;

 public:
  /// <summary>
  /// Makes the <see cref="ICpuId"/> object.
  /// </summary>
  /// <returns>An object that knows how to get CPUID information for the CPU which is being called.</returns>
  virtual auto MakeCpuId() noexcept -> std::unique_ptr<ICpuId> = 0;

  /// <summary>
  /// Makes the <see cref="ICpuId"/> object.
  /// </summary>
  /// <param name="cpunum">The CPU number from the Operating System to create for.</param>
  /// <returns>An object that knows how to get CPUID information for the <paramref name="cpunum"/>.</returns>
  virtual auto MakeCpuId(unsigned int cpunum) noexcept -> std::unique_ptr<ICpuId> = 0;

  /// <summary>
  /// Returns the number of CPUs known.
  /// </summary>
  /// <returns>The number of CPUs known.</returns>
  virtual auto CpuCount() const noexcept -> unsigned int = 0;

  /// <summary>
  /// Finalizes an instance of the <see cref="ICpuIdFactory"/> class.
  /// </summary>
  virtual ~ICpuIdFactory() noexcept = default;
};

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
#endif
