////////////////////////////////////////////////////////////////////////////////
//
// (C) 2012-2022, Jason Curl
//
////////////////////////////////////////////////////////////////////////////////
//
// Header: cpuiddump_intel.cpp
//
// Description:
//  Dump registers according to Intel documentation. Reserved registers are
//  still dumped.
//
////////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "cpuid_iter.h"
#include "cpuiddll.h"

#include <stdlib.h>

/// <summary>
/// Get the register, iterate to the next element.
/// </summary>
struct cpuidinfo *get_cpuid(struct cpuidinfo **info, size_t *bytes, DWORD veax, DWORD vecx)
{
	if (*bytes < sizeof(struct cpuidinfo)) return NULL;

	(*info)->veax = veax;
	(*info)->vecx = vecx;
	cpuidget(*info);

	struct cpuidinfo *node = *info;
	(*info)++;
	*bytes -= sizeof(struct cpuidinfo);
	return node;
}

struct cpuidinfo *rev_cpuid(struct cpuidinfo **info, size_t *bytes)
{
	(*info)--;
	*bytes += sizeof(struct cpuidinfo);
	return *info;
}
