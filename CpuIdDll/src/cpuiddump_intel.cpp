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

#include "cpuiddll.h"
#include "cpuid.h"
#include "cpuid_iter.h"

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
	DWORD leafs = info[0].peax & 0x0FFFFFFF;
	DWORD leaf = 1;

	struct cpuidinfo *iter = info + 2;
	size_t iterb = bytes - sizeof(struct cpuidinfo) * 2;
	struct cpuidinfo *result;

	int sgx;

	while (leaf <= leafs && iterb >= sizeof(struct cpuidinfo)) {
		switch(leaf) {
		case 2: {
			// Cache Descriptors
			result = get_cpuid(&iter, &iterb, leaf, 0);
			if (!result) break;

			// Intel documentation AP485, May 2012, says that we should check the lower
			// 8 bits of EAX to know how often to call this function.
			//
			// Newer documentation in Volume 2, May 2020 says that the first byte is
			// always 0x01 and should be ignored. Both documents are correct.
			DWORD subleafs = result->peax & 0xFF;
			for (DWORD subleaf = 1; subleaf < subleafs; subleaf++) {
				result = get_cpuid(&iter, &iterb, leaf, subleaf);
				if (!result) break;
			}
			break;
		}
		case 4: {
			// Deterministic Cache Parameters
			result = get_cpuid(&iter, &iterb, leaf, 0);
			if (!result) break;

			DWORD subleaf = 1;
			while(result->peax & 0x0000001F) {
				result = get_cpuid(&iter, &iterb, leaf, subleaf);
				subleaf++;
			}
			break;
		}
		case 7: {
			// Structured Extended Feature Flags. 
			result = get_cpuid(&iter, &iterb, leaf, 0);
			if (!result) break;

			DWORD subleafs = result->peax;
			sgx = result->pebx & 0x04;
			for (DWORD subleaf = 1; subleaf <= subleafs; subleaf++) {
				result = get_cpuid(&iter, &iterb, leaf, subleaf);
				if (!result) break;
			}
			break;
		}
		case 11:
		case 31: {
			// x2APIC features
			result = get_cpuid(&iter, &iterb, leaf, 0);
			if (!result) break;

			DWORD subleaf = 1;
			while (result->pebx & 0xFFFF) {
				result = get_cpuid(&iter, &iterb, leaf, subleaf);
				if (!result) break;
				subleaf++;
			}
			break;
		}
		case 13: {
			// Processor Extended State
			for (DWORD subleaf = 0; subleaf < 64; subleaf++) {
				result = get_cpuid(&iter, &iterb, leaf, subleaf);
				if (!result) break;
				if (subleaf >= 2 && !(result->peax || result->pebx || result->pecx || result->pedx)) {
					// We don't want to store this entry, as it's not interesting.
					rev_cpuid(&iter, &iterb);
				}
			}
			break;
		}
		case 15: {
			for (DWORD subleaf = 0; subleaf < 2; subleaf ++) {
				result = get_cpuid(&iter, &iterb, leaf, subleaf);
				if (!result) break;
			}
			break;
		}
		case 16: {
			result = get_cpuid(&iter, &iterb, leaf, 0);
			if (!result) break;

			DWORD subleaf = 1;
			DWORD residbit = result->pebx >> 1;
			while (residbit) {
				result = get_cpuid(&iter, &iterb, leaf, subleaf);
				residbit >>= 1;
				subleaf++;
			}
			break;
		}
		case 18: {
			result = get_cpuid(&iter, &iterb, leaf, 0);
			if (!result) break;

			result = get_cpuid(&iter, &iterb, leaf, 1);
			if (!result) break;

			if (sgx) {
				DWORD subleaf = 2;
				do {
					result = get_cpuid(&iter, &iterb, leaf, subleaf);
					if (!result) break;
					subleaf++;
					if (subleaf > 0xFF || (result->peax & 0x0000000F) == 0)
						subleaf = 0;
				} while (subleaf > 0);
			}
			break;
		}
		case 20:
		case 23:
		case 24: {
			result = get_cpuid(&iter, &iterb, leaf, 0);
			if (!result) break;

			DWORD subleafs = result->peax;
			for (DWORD subleaf = 1; subleaf <= subleafs; subleaf++) {
				result = get_cpuid(&iter, &iterb, leaf, subleaf);
				if (!result) break;
			}
			break;
		}
		default:
			result = get_cpuid(&iter, &iterb, leaf, 0);
			if (!result) break;
			break;
		}

		leaf++;
	}

	int i = (int)(iter - info);
	i += iddump_region(0x80000000, info + 1, info + i, bytes - i * sizeof(struct cpuidinfo));
	i += iddump_region(0x20000000, NULL,     info + i, bytes - i * sizeof(struct cpuidinfo));
	return i;
}
