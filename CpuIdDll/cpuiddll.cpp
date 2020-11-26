// cpuiddll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "cpuiddll.h"

extern "C" {
int __stdcall cpuidl(DWORD eax, DWORD ecx, LPDWORD peax, LPDWORD pebx, LPDWORD pecx, LPDWORD pedx);
int __stdcall cpuidt();
}

static int inteliddump(struct cpuidinfo *info, size_t bytes);
static int amdiddump(struct cpuidinfo *info, size_t bytes);
static int defaultiddump(struct cpuidinfo *info, size_t bytes);

/// <summary>
/// The supported vendors in this DLL
/// </summary>
typedef enum vendor_t {
	/// <summary>
	/// Unknown vendor
	/// </summary>
	VENDOR_UNKNOWN,

	/// <summary>
	/// Intel
	/// </summary>
	VENDOR_INTEL,

	/// <summary>
	/// AMD
	/// </summary>
	VENDOR_AMD,
};

/// <summary>
/// Executes the CPUID on the current processor for eax and ecx.
/// </summary>
/// <param name="eax">EAX input</param>
/// <param name="ecx">ECX input</param>
/// <param name="peax">EAX output</param>
/// <param name="pebx">EBX output</param>
/// <param name="pecx">ECX output</param>
/// <param name="pedx">EDX output</param>
/// <returns></returns>
CPUIDDLL_API int WINAPI cpuid(DWORD eax, DWORD ecx, LPDWORD peax, LPDWORD pebx, LPDWORD pecx, LPDWORD pedx)
{
	return cpuidl(eax, ecx, peax, pebx, pecx, pedx);
}

/// <summary>
/// Checks if this processor supports the CPUID instruction
/// </summary>
/// <returns></returns>
CPUIDDLL_API int WINAPI hascpuid()
{
	return cpuidt() != 0;
}

#define cpuidget(info) cpuid((info)->veax, (info)->vecx, &((info)->peax), &((info)->pebx), &((info)->pecx), &((info)->pedx))

static int iddump_internal(struct cpuidinfo *info, size_t bytes)
{
	if (info == NULL || bytes < 2 * sizeof(struct cpuidinfo)) return 0;

	info[0].veax = 0x00000000;
	info[0].vecx = 0x00000000;
	cpuidget(info);

	info[1].veax = 0x80000000;
	info[1].vecx = 0x00000000;
	cpuidget(info+1);

	vendor_t vendor;
	if (info[0].pebx == 0x756E6547 && info[0].pecx == 0x6C65746E && info[0].pedx == 0x49656E69) {
		vendor = VENDOR_INTEL;
	} else if (info[0].pebx == 0x68747541 && info[0].pecx == 0x444d4163 && info[0].pedx == 0x69746e65) {
		vendor = VENDOR_AMD;
	} else {
		vendor = VENDOR_UNKNOWN;
	}

	switch (vendor) {
	case VENDOR_INTEL:
		return inteliddump(info, bytes);
	case VENDOR_AMD:
		return amdiddump(info, bytes);
	default:
		return defaultiddump(info, bytes);
	}
}

/// <summary>
/// Performs a dump of known CPUID values on the current core
/// </summary>
/// <param name="info">cpuidinfo array to dump into</param>
/// <param name="bytes">Number of bytes allocated by <paramref name="info"/>. </param>
/// <returns>Number of elements</returns>
CPUIDDLL_API int WINAPI iddump(struct cpuidinfo *info, size_t bytes)
{
	DWORD currentProcessor = GetCurrentProcessorNumber();
	return iddumponcore(info, bytes, currentProcessor);
}

/// <summary>
/// Performs a dump of known CPUID values using the specified core
/// </summary>
/// <param name="info">cpuidinfo array to dump into</param>
/// <param name="bytes">Number of bytes allocated by <paramref name="info"/>. </param>
/// <returns>Number of elements</returns>
CPUIDDLL_API int WINAPI iddumponcore(struct cpuidinfo *info, size_t bytes, int core)
{
	if (core < 0 || core > 64) {
		SetLastError(ERROR_INVALID_PARAMETER);
		return -1;
	}

	HANDLE currentThread = GetCurrentThread();
	DWORD_PTR newMask = 1 << core;
	DWORD_PTR affinity = SetThreadAffinityMask(currentThread, newMask);
	if (!affinity) return -1;

	int result = iddump_internal(info, bytes);
	SetThreadAffinityMask(currentThread, affinity);
	return result;
}

static int inteliddump(struct cpuidinfo *info, size_t bytes)
{
	int el = 2;
	int p = 1;
	int q = 0;
	int c = 0;

	int sgx = 0;

	while (p <= (int)(info[0].peax & 0x7FFFFFFF) && bytes > (el + 1) * sizeof(struct cpuidinfo)) {
		switch (p) {
		case 2:
			// Cache Descriptors
			info[el].veax = p;
			info[el].vecx = 0;
			cpuidget(info+el);

			// Intel documentation AP485, May 2012, says that we should check the lower
			// 8 bits of EAX to know how often to call this function.
			//
			// Newer documentation in Volume 2, May 2020 says that the first byte is
			// always 0x01 and should be ignored. Both documentats are correct.
			if (q == 0) c = info[el].peax & 0xFF;
			if (c != 0 && q < c - 1) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 4:
			// Deterministic Cache Parameters
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (info[el].peax & 0x1F) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 7:
			// Structured Extended Feature Flags. 
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			// Future leaves depend on this bit
			if (q == 0) sgx = info[el].pebx & 0x04;

			// EAX contains the number of subleaves when ECX == 0.
			// EAX == 0 as return means no subleaves
			if (q == 0) c = info[el].peax;
			if (q < c) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 11:
			// x2APIC features
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (info[el].peax || info[el].pebx) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 13:
			// Processor Extended State
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q < 2 || (info[el].peax || info[el].pebx || info[el].pecx || info[el].pedx)) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 15:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q < 1) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 16:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q < 3) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 18:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q < 2 || (info[el].peax & 0x03)) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 20:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q < 1) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 23:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q < 3) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 24:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q == 0) c = info[el].peax;
			if (q < c) {
				q++;
			} else {
				p++; q = 0;
			}
			break;
		case 31:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (info[el].pebx & 0xFFFF) {
				q++;
			} else {
				p++; q = 0;
			}
			break;
		default:
			info[el].veax = p;
			info[el].vecx = 0;
			cpuidget(info+el);

			p++;
			break;
		}
		el++;
	}

	// Dump the extended values second
	if (info[1].peax & 0x80000000) {
		p = 1; c = (int)(info[1].peax & 0x7FFFFFFF);
		while (p <= c && bytes > (el + 1) * sizeof(struct cpuidinfo)) {
			info[el].veax = 0x80000000 + p;
			info[el].vecx = 0;
			cpuidget(info+el);
			p++;
			el++;
		}
	}

	return el;
}

static int amdiddump(struct cpuidinfo *info, size_t bytes)
{
	int el = 2;
	int p = 1;
	int q = 0;
	while (p <= (int)(info[0].peax & 0x7FFFFFFF) && bytes > (el + 1) * sizeof(struct cpuidinfo)) {
		switch (p) {
		case 13:
			switch(q) {
			case 0:
				info[el].veax = p;
				info[el].vecx = 0;
				cpuidget(info+el);
				q++;
				break;
			case 1:
				info[el].veax = p;
				info[el].vecx = 2;
				cpuidget(info+el);
				q++;
				break;
			case 2:
				info[el].veax = p;
				info[el].vecx = 62;
				cpuidget(info+el);
				q = 0; p++;
				break;
			}
			break;
		default:
			info[el].veax = p;
			info[el].vecx = 0;
			cpuidget(info+el);
			p++;
			break;
		}
		el++;
	}

	// Dump the extended values second
	p = 1;
	if (info[1].peax & 0x80000000) {
		while (p <= (int)(info[1].peax & 0x7FFFFFFF) && bytes > (el + 1) * sizeof(struct cpuidinfo)) {
			info[el].veax = 0x80000000 + p;
			info[el].vecx = 0;
			cpuidget(info+el);
			p++;
			el++;
		}
	}

	return el;
}

static int defaultiddump(struct cpuidinfo *info, size_t bytes)
{
	int el = 2;
	int p = 1;
	while (p <= (int)(info[0].peax & 0x7FFFFFFF) && bytes > (el + 1) * sizeof(struct cpuidinfo)) {
		info[el].veax = p;
		info[el].vecx = 0;
		cpuidget(info+el);
		p++;
		el++;
	}

	// Dump the extended values second
	p = 1;
	while (p <= (int)(info[1].peax & 0x7FFFFFFF) && bytes > (el + 1) * sizeof(struct cpuidinfo)) {
		info[el].veax = 0x80000000 + p;
		info[el].vecx = 0;
		cpuidget(info+el);
		p++;
		el++;
	}
	return el;
}