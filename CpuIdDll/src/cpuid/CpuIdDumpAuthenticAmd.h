#ifndef RJCP_DIAGNOSTICS_CPUID_CPUIDDUMPAUTHENTICAMD_H
#define RJCP_DIAGNOSTICS_CPUID_CPUIDDUMPAUTHENTICAMD_H

#include "cpuid/ICpuIdDump.h"

namespace rjcp {
namespace diagnostics {
namespace cpuid {

class CpuIdDumpAuthenticAmd : public ICpuIdDump {
 public:
  auto CpuIdDump(ICpuId& cpuid) -> std::vector<CpuIdRegister> override;
};

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
#endif
