#include "globalstate.h"

namespace rjcp {
namespace diagnostics {

GlobalState::GlobalState(std::unique_ptr<cpuid::ICpuIdFactory> factory) : factory_{std::move(factory)} { }

auto GlobalState::GetCpuId() const noexcept -> std::unique_ptr<cpuid::ICpuId> const {
  if (factory_) return factory_->MakeCpuId();
  return nullptr;
}

auto GlobalState::GetCpuId(unsigned int cpunum) const noexcept -> std::unique_ptr<cpuid::ICpuId> {
  if (factory_ ) return factory_->MakeCpuId(cpunum);
  return nullptr;
}

auto GlobalState::GetCpuCount() const noexcept -> unsigned int {
  if (factory_) return factory_->CpuCount();
  return 0;
}

// NOLINTNEXTLINE(cppcoreguidelines-avoid-non-const-global-variables)
std::unique_ptr<GlobalState> global = nullptr;

}  // namespace diagnostics
}  // namespace rjcp
