#ifndef RJCP_DIAGNOSTICS_NATIVE_WIN32_KERNEL32_GETCURRENTPROCESSORNUMBERCPUID_H
#define RJCP_DIAGNOSTICS_NATIVE_WIN32_KERNEL32_GETCURRENTPROCESSORNUMBERCPUID_H

#include "stdafx.h"

#include <array>

namespace rjcp {
namespace native {
namespace win32 {
namespace kernel32 {

/// <summary>
/// Emulate GetCurrentProcessorNumber for Windows XP.
/// </summary>
class GetCurrentProcessorNumberCpuId {
 public:
  /// <summary>
  /// Initializes a new instance of the <see cref="GetCurrentProcessorNumberCpuId"/> class.
  /// </summary>
  GetCurrentProcessorNumberCpuId();

  /// <summary>
  /// Gets the current processor number.
  /// </summary>
  /// <returns>Returns the current processor number.</returns>
  auto GetCurrentProcessorNumber() -> DWORD;

 private:
  unsigned int cpus_ = 0;
  std::array<uint8_t, 256> acpi_{};
};

}  // namespace kernel32
}  // namespace win32
}  // namespace native
}  // namespace rjcp
#endif
