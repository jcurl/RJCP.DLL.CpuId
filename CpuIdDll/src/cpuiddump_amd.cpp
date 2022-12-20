////////////////////////////////////////////////////////////////////////////////
//
// (C) 2012-2022, Jason Curl
//
////////////////////////////////////////////////////////////////////////////////
//
// Header: cpuiddump_amd.cpp
//
// Description:
//  Dump registers according to AMD documentation. Reserved registers are
//  still dumped.
//
////////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "cpuiddll.h"
#include "cpuid.h"
#include "cpuid_iter.h"

/// <summary>
/// Dump registers according to the AMD specifications.
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
int iddump_amd(struct cpuidinfo *info, size_t bytes)
{
	DWORD leafs = info[0].peax & 0x0FFFFFFF;
	DWORD leaf = 1;

	struct cpuidinfo *iter = info + 2;
	size_t iterb = bytes - sizeof(struct cpuidinfo) * 2;
	struct cpuidinfo *result;

	while (leaf <= leafs && iterb >= sizeof(struct cpuidinfo)) {
		switch(leaf) {
		case 7: {
			// Structured Extended Feature Flags. 
			result = get_cpuid(&iter, &iterb, leaf, 0);
			if (!result) break;

			DWORD subleafs = result->peax;
			for (DWORD subleaf = 1; subleaf <= subleafs; subleaf++) {
				result = get_cpuid(&iter, &iterb, leaf, subleaf);
				if (!result) break;
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
		default:
			result = get_cpuid(&iter, &iterb, leaf, 0);
			if (!result) break;
			break;
		}

		leaf++;
	}

	if (info[1].peax & 0x80000000) {
		leafs = info[1].peax;
		leaf = info[1].veax;

		while (leaf <= leafs && iterb >= sizeof(struct cpuidinfo)) {
			switch(leaf) {
			case 0x8000001D: {
				result = get_cpuid(&iter, &iterb, leaf, 0);
				if (!result) break;

				DWORD subleaf = 1;
				while (result->peax & 0xF) {
					result = get_cpuid(&iter, &iterb, leaf, subleaf);
					if (!result) break;
					subleaf++;
				}
				break;
			}
			case 0x80000026: {
				DWORD subleaf = 0;
				do {
					result = get_cpuid(&iter, &iterb, leaf, subleaf);
					if (!result) break;
					subleaf++;
				} while(result->pecx & 0x0000FF00);
				break;
			}
			default:
				result = get_cpuid(&iter, &iterb, leaf, 0);
				if (!result) break;
				break;
			}
		}
	}

	return (int)(iter - info);
}
