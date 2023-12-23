#ifndef RJCP_DIAGNOSTICS_GLOBALSTATE_H
#define RJCP_DIAGNOSTICS_GLOBALSTATE_H

#include "cpuid/ICpuId.h"
#include "cpuid/ICpuIdFactory.h"

#include <memory>

using namespace rjcp::diagnostics;

namespace rjcp {
namespace diagnostics {

/// <summary>
/// Global State for the DLL.
/// </summary>
/// <remarks>
/// The global state class is required to initialise the correct classes before the user calls the library API. The
/// purpose is to allow other implementations to be provided for testing purposes.
/// </remarks>
class GlobalState {
 public:
  /// <summary>
  /// Initialise the global state with the factory provided.
  /// </summary>
  /// <param name="factory">The factory to use to get CPUID information.</param>
  /// <remarks>
  /// The global state is only used by `cpuiddll.cpp` and `dllmain.cpp` and no more. It is to maintain state for the DLL
  /// for performance only.
  /// </remarks>
  GlobalState(std::unique_ptr<cpuid::ICpuIdFactory> factory);

  // No copy or move.
  GlobalState(GlobalState&) = delete;
  GlobalState(GlobalState&&) = delete;
  auto operator=(const GlobalState&) -> GlobalState& = delete;
  auto operator=(GlobalState&&) -> GlobalState& = delete;

  /// <summary>
  /// Finalizes an instance of the <see cref="GlobalState"/> class.
  /// </summary>
  ~GlobalState() = default;

  /// <summary>
  /// Global pointer to an object that gets the CPUID information for the current thread.
  /// </summary>
  auto GetCpuId() const noexcept -> std::unique_ptr<cpuid::ICpuId> const;

  /// <summary>
  /// Gets an object to obtain the CPUID information from the core specified.
  /// </summary>
  /// <param name="cpunum">The CPU core number.</param>
  /// <returns>An instance of a <see ref="ICpuId"/>.</returns>
  auto GetCpuId(unsigned int cpunum) const noexcept -> std::unique_ptr<cpuid::ICpuId>;

  /// <summary>
  /// Gets the number of CPUs supported.
  /// </summary>
  /// <returns>The number of CPUs supported.</returns>
  auto GetCpuCount() const noexcept -> unsigned int;

 private:
  std::unique_ptr<cpuid::ICpuIdFactory> factory_;
};

/// <summary>
/// Global state. Must be initialised before the library is used.
/// </summary>
// NOLINTNEXTLINE(cppcoreguidelines-avoid-non-const-global-variables)
extern std::unique_ptr<GlobalState> global;

}  // namespace diagnostics
}  // namespace rjcp

#endif
