////////////////////////////////////////////////////////////////////////////////
//
// (C) 2012-2020, Jason Curl
//
////////////////////////////////////////////////////////////////////////////////
//
// Header: cpuid.h
//
// Description:
//  Routines to dump CPUID restuls
//
////////////////////////////////////////////////////////////////////////////////

#ifndef _PRV_CPUID_H
#define _PRV_CPUID_H

#include "cpuidx.h"
#include "cpuidtypes.h"

int iddump_main(struct cpuidinfo *info, size_t bytes);
int iddump_intel(struct cpuidinfo *info, size_t bytes);
int iddump_amd(struct cpuidinfo *info, size_t bytes);
int iddump_default(struct cpuidinfo *info, size_t bytes);

int iddump_region(int basefunction, struct cpuidinfo *base, struct cpuidinfo *info, size_t bytes);
int iddump_hypervisor(struct cpuidinfo *base, struct cpuidinfo *info, size_t bytes);
#endif