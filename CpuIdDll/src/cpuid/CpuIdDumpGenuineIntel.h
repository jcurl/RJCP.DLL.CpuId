#ifndef RJCP_DIAGNOSTICS_CPUID_CPUIDDUMPGENUINEINTEL_H
#define RJCP_DIAGNOSTICS_CPUID_CPUIDDUMPGENUINEINTEL_H

#include "cpuid/ICpuIdDump.h"

namespace rjcp {
namespace diagnostics {
namespace cpuid {

class CpuIdDumpGenuineIntel : public ICpuIdDump {
 public:
  auto CpuIdDump(ICpuId& cpuid) -> std::vector<CpuIdRegister> override;
};

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
#endif
