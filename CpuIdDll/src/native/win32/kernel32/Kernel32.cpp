#include "native/win32/kernel32/Kernel32.h"

#include "native/win32/LibraryHandle.h"
#include "native/win32/kernel32/GetCurrentProcessorNumberCpuId.h"

#include <memory>
#include <mutex>

namespace rjcp {
namespace native {
namespace win32 {
namespace kernel32 {

namespace {
/// <summary>
/// Implementation of dynamic, runtime, Kernel32 API calls.
/// </summary>
/// <remarks>
/// This singleton is useful for providing implementations for functions that are not found on Windows XP (the minimum
/// library that is supported). Implementations therefore call this singleton to get either the current version
/// implemented by the Operating System (e.g. on Windows 7), or a workaround implementatio if on Windows XP.
/// </remarks>
class Kernel32 {
 public:
  static auto instance() -> Kernel32& {
    return instance_;
  }

  // Note: This is not made `const`, as when we need to workaround it, we'd do it lazily via a std::once, which would
  // change the state of this class.
  auto GetCurrentProcessorNumber() -> DWORD {
    if (GetCurrentProcessorNumber_) return GetCurrentProcessorNumber_();
    if (NtGetCurrentProcessorNumber_) return NtGetCurrentProcessorNumber_();

    // Windows XP doesn't offer this function. We enuemrate over all threads in the system, query the 8-bit APICID, and
    // then to get the thread number, reverse map the APICID back to the original thread number.
    std::call_once(GetCurrentProcessorNumberCpuIdFlag_, [this] {
      GetCurrentProcessorNumberCpuId_ = std::make_unique<GetCurrentProcessorNumberCpuId>();
    });
    if (GetCurrentProcessorNumberCpuId_) return GetCurrentProcessorNumberCpuId_->GetCurrentProcessorNumber();
    return 0;
  }

 private:
  Kernel32() = default;
  ~Kernel32() = default;
  Kernel32(const Kernel32&) = delete;
  Kernel32(Kernel32&&) = delete;
  auto operator=(const Kernel32&) -> Kernel32& = delete;
  auto operator=(Kernel32&&) -> Kernel32& = delete;
  static Kernel32 instance_;  // NOLINT(cppcoreguidelines-avoid-non-const-global-variables)

 private:
  // Must be before the public fields use this property.
  ModuleGet khandle_{TEXT("kernel32.dll")};
  LibraryLoad nhandle_{TEXT("ntdll.dll")};

  using GetCurrentProcessorNumber_t = DWORD(WINAPI*)();
  GetCurrentProcessorNumber_t GetCurrentProcessorNumber_ = khandle_["GetCurrentProcessorNumber"];
  GetCurrentProcessorNumber_t NtGetCurrentProcessorNumber_ = nhandle_["NtGetCurrentProcessorNumber"];

  std::once_flag GetCurrentProcessorNumberCpuIdFlag_{};
  std::unique_ptr<GetCurrentProcessorNumberCpuId> GetCurrentProcessorNumberCpuId_{};
};

Kernel32 Kernel32::instance_;  // NOLINT(cppcoreguidelines-avoid-non-const-global-variables)
}  // namespace

auto GetCurrentProcessorNumber() -> DWORD {
  return Kernel32::instance().GetCurrentProcessorNumber();
}

}  // namespace kernel32
}  // namespace win32
}  // namespace native
}  // namespace rjcp
