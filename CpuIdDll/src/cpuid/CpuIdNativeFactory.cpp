#include "cpuid/CpuIdNativeFactory.h"

#include "cpuid/CpuIdFactory.h"
#include "cpuid/CpuIdNative.h"
#include "cpuid/CpuIdNativeConfig.h"

#include <memory>
#include <thread>

namespace rjcp {
namespace diagnostics {
namespace cpuid {

auto CpuIdNativeFactory::MakeCpuId() noexcept -> std::unique_ptr<ICpuId> {
  return std::make_unique<CpuIdNative>();
}

auto CpuIdNativeFactory::MakeCpuId(unsigned int cpunum) noexcept -> std::unique_ptr<ICpuId> {
  return std::make_unique<CpuIdNative>(cpunum);
}

auto CpuIdNativeFactory::CpuCount() const noexcept -> unsigned int {
  return std::thread::hardware_concurrency();
}

auto MakeCpuIdFactory([[maybe_unused]] const CpuIdNativeConfig& config) noexcept -> std::unique_ptr<ICpuIdFactory> {
  // There are no configuration elements needed from `config`.
  return std::make_unique<CpuIdNativeFactory>();
}

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
