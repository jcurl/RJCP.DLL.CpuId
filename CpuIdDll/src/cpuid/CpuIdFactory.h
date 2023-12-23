#ifndef RJCP_DIAGNOSTICS_CPUID_CPUIDFACTORY_H
#define RJCP_DIAGNOSTICS_CPUID_CPUIDFACTORY_H

#include "cpuid/CpuIdNativeFactory.h"

// To create a new ICpuIdFactory object:
// * Create a new class XXXFactory : public ICpuIdFactory with header and source file
// * Create a new config class XXXConfig : public ICpuIdConfig with header and source file
// * Add here the signature for MakeCpuIdFactory for the newly created config. You'll need to include the config here
//   above.
// * Create the implementation in the source file where XXXFactory is.
//
// A useful factory might be to read a file and extract information from there.
#endif
