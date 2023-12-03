////////////////////////////////////////////////////////////////////////////////
//
// (C) 2012-2020, Jason Curl
//
////////////////////////////////////////////////////////////////////////////////
//
// Header: cpuiddll.h
//
// Description:
//  Public interface exposing CPUID instructions for interop with other
//  languages.
//
////////////////////////////////////////////////////////////////////////////////

#ifndef _CPUIDDLL_H
#define _CPUIDDLL_H

// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the CPUIDDLL_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// CPUIDDLL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef CPUIDDLL_EXPORTS
#define CPUIDDLL_API __declspec(dllexport)
#else
#define CPUIDDLL_API __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C" {
#endif

/// <summary>
/// Results of a CPUID function call.
/// </summary>
struct cpuidinfo {
	/// <summary>
	/// The CPUID leaf called.
	/// </summary>
	DWORD veax;

	/// <summary>
	/// The CPUID sub-leaf called.
	/// </summary>
	DWORD vecx;

	/// <summary>
	/// The result EAX after the CPUID call.
	/// </summary>
	DWORD peax;

	/// <summary>
	/// The result EBX after the CPUID call.
	/// </summary>
	DWORD pebx;

	/// <summary>
	/// The result ECX after the CPUID call.
	/// </summary>
	DWORD pecx;

	/// <summary>
	/// The result EDX after the CPUID call.
	/// </summary>
	DWORD pedx;
};

/// <summary>
/// Executes the CPUID on the current processor for eax and ecx.
/// </summary>
/// <param name="eax">EAX input.</param>
/// <param name="ecx">ECX input.</param>
/// <param name="peax">EAX output.</param>
/// <param name="pebx">EBX output.</param>
/// <param name="pecx">ECX output.</param>
/// <param name="pedx">EDX output.</param>
/// <returns>Returns zero on success.</returns>
CPUIDDLL_API int WINAPI cpuid(DWORD eax, DWORD ecx, LPDWORD peax, LPDWORD pebx, LPDWORD pecx, LPDWORD pedx);

/// <summary>
/// Checks if this processor supports the CPUID instruction.
/// </summary>
/// <returns>Returns non-zero if CPUID is supported.</returns>
CPUIDDLL_API int WINAPI hascpuid();

/// <summary>
/// Performs a dump of known CPUID values on the current core.
/// </summary>
/// <param name="info">cpuidinfo array to dump into.</param>
/// <param name="bytes">Number of bytes allocated by <paramref name="info"/>.</param>
/// <returns>Number of elements put into <paramref name="info"/>.</returns>
/// <remarks>
/// On Windows XP, it's not possible to know on what processor the current thread
/// is running, so it will always execute on the first processor. To specify which
/// processor it should be run on, use the method <see cref="iddumponcore"/>. On
/// Windows Server 2003 and later, the thread affinity is set to the current processor,
/// but this is generally indeterminate. Again, it's advised to use <see cref="iddumponcore"/>.
/// </remarks>
CPUIDDLL_API int WINAPI iddump(struct cpuidinfo *info, size_t bytes);

/// <summary>
/// Performs a dump of known CPUID values using the specified core.
/// </summary>
/// <param name="info">cpuidinfo array to dump into.</param>
/// <param name="bytes">Number of bytes allocated by <paramref name="info"/>.</param>
/// <returns>Number of elements put into <paramref name="info"/>.</returns>
CPUIDDLL_API int WINAPI iddumponcore(struct cpuidinfo *info, size_t bytes, int core);

/// <summary>
/// Performs a dump of known CPUID values for each core.
/// </summary>
/// <param name="info">cpuidinfo array to dump into.</param>
/// <param name="bytes">Number of bytes allocated by <paramref name="info"/>.</param>
/// <returns>Number of elements put into <paramref name="info"/>.</returns>
/// <remarks>
/// The array buffer provided, <paramref name="info"/> is filled for all cores. The first
/// element is a fake CPUID entry, of veax being 0xFFFFFFFF and vecx being the core number.
/// </remarks>
CPUIDDLL_API int WINAPI iddumpall(struct cpuidinfo *info, size_t bytes);

#ifdef __cplusplus
}
#endif

#endif