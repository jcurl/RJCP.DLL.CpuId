#ifndef RJCP_DIAGNOSTICS_CPUID_CPUIDDUMPGENERIC_H
#define RJCP_DIAGNOSTICS_CPUID_CPUIDDUMPGENERIC_H

#include "cpuid/ICpuIdDump.h"

namespace rjcp {
namespace diagnostics {
namespace cpuid {

class CpuIdDumpGeneric : public ICpuIdDump {
 public:
  auto CpuIdDump(ICpuId& cpuid) -> std::vector<CpuIdRegister> override;
};

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
#endif
