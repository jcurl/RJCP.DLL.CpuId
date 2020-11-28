////////////////////////////////////////////////////////////////////////////////
//
// (C) 2012-2020, Jason Curl
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

#include "cpuiddll.h"
#include "cpuid.h"

/// <summary>
/// Dump registers according to the Intel specifications.
/// </summary>
/// <param name="info">The memory buffer to write the results to.</param>
/// <param name="bytes">The total number of bytes allocated for <paramref name="info"/></param>
/// <remarks>
/// When dumping, the first two entries are expected to have:
/// <list>
/// <item>0: The CPUID[00h] results.</item>
/// <item>1: The CPUID[80000000h] results.</item>
/// </list>
/// </remarks>
/// <returns>Number of elements put into <paramref name="info"/>.</returns>
int iddump_intel(struct cpuidinfo *info, size_t bytes)
{
	int i = 2;
	int p = 1;
	int q = 0;
	int c = 0;

	int sgx;

	while (p <= (int)(info[0].peax & 0x0FFFFFFF) && bytes >= (i + 1) * sizeof(struct cpuidinfo)) {
		switch (p) {
		case 2:
			// Cache Descriptors
			info[i].veax = p;
			info[i].vecx = 0;
			cpuidget(info + i);

			// Intel documentation AP485, May 2012, says that we should check the lower
			// 8 bits of EAX to know how often to call this function.
			//
			// Newer documentation in Volume 2, May 2020 says that the first byte is
			// always 0x01 and should be ignored. Both documents are correct.
			if (q == 0) c = info[i].peax & 0xFF;
			if (c != 0 && q < c - 1) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 4:
			// Deterministic Cache Parameters
			info[i].veax = p;
			info[i].vecx = q;
			cpuidget(info + i);

			if (info[i].peax & 0x1F) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 7:
			// Structured Extended Feature Flags. 
			info[i].veax = p;
			info[i].vecx = q;
			cpuidget(info + i);

			// EAX contains the number of subleaves when ECX == 0.
			// EAX == 0 as return means no subleaves. We get as necessary
			// some flags on further decoding.
			if (q == 0) {
				c = info[i].peax;
				sgx = info[i].pebx & 0x4;
			}
			if (q < c) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 11:
		case 31:
			// x2APIC features
			info[i].veax = p;
			info[i].vecx = q;
			cpuidget(info + i);

			if (info[i].pebx & 0xFFFF) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 13:
			// Processor Extended State
			info[i].veax = p;
			info[i].vecx = q;
			cpuidget(info + i);

			if (q < 2 || (info[i].peax || info[i].pebx || info[i].pecx || info[i].pedx)) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 15:
			info[i].veax = p;
			info[i].vecx = q;
			cpuidget(info + i);

			if (q < 1) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 16:
			info[i].veax = p;
			info[i].vecx = q;
			cpuidget(info + i);

			if (q == 0) c = info[i].pebx >> 1;
			if (c) {
				c >>= 1;
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 18:
			info[i].veax = p;
			info[i].vecx = q;
			cpuidget(info + i);

			// Enumerate ECX=0, 1, 2, and only 3 or higher if SGX is set and the Type is not invalid
			if (q < 2 || (sgx && (info[i].peax & 0xF))) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 20:
			info[i].veax = p;
			info[i].vecx = q;
			cpuidget(info + i);

			if (q == 0) c = info[i].peax;
			if (q < 1 || q < c) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 23:
			info[i].veax = p;
			info[i].vecx = q;
			cpuidget(info + i);

			if (q == 0) c = info[i].peax;
			if (q < c) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 24:
			info[i].veax = p;
			info[i].vecx = q;
			cpuidget(info + i);

			if (q == 0) c = info[i].peax;
			if (q < c) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		default:
			info[i].veax = p;
			info[i].vecx = 0;
			cpuidget(info + i);

			p++;
			break;
		}
		i++;
	}

	i += iddump_region(0x80000000, info + 1, info + i, bytes - i * sizeof(struct cpuidinfo));
	i += iddump_region(0x20000000, NULL,     info + i, bytes - i * sizeof(struct cpuidinfo));
	return i;
}
