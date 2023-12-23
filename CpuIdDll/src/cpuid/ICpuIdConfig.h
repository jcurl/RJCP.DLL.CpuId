#ifndef RJCP_DIAGNOSTICS_CPUID_ICPUIDCONFIG_H
#define RJCP_DIAGNOSTICS_CPUID_ICPUIDCONFIG_H

namespace rjcp {
namespace diagnostics {
namespace cpuid {

/// <summary>
/// A configuration type for a <see cref="ICpuIdFactory"/>.
/// </summary>
/// <remarks>
/// When instantiating an <see cref="ICpuIdFactory"/>, use <c>MakeCpuIdFactory&lt;ICpuIdConfig&gt;(const
/// ICpuIdConfig&amp; config)</c> where <c>config</c> the template parameter and the type is a concrete implementation
/// (it won't work giving it an abstract class). This class is for posterity in C++17 (but may be useful in future
/// standards).
/// </remarks>
class ICpuIdConfig {
 protected:
  ICpuIdConfig() = default;
  ICpuIdConfig(const ICpuIdConfig&) = default;
  ICpuIdConfig(ICpuIdConfig&&) = default;
  auto operator=(const ICpuIdConfig&) -> ICpuIdConfig& = default;
  auto operator=(ICpuIdConfig&&) -> ICpuIdConfig& = default;

 public:
  /// <summary>
  /// Finalizes an instance of the <see cref="ICpuIdConfig"/> class.
  /// </summary>
  ~ICpuIdConfig() noexcept = default;
};

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
#endif
