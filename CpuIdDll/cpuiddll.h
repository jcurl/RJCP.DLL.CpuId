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

struct cpuidinfo {
	DWORD veax;
	DWORD vecx;
	DWORD peax;
	DWORD pebx;
	DWORD pecx;
	DWORD pedx;
};

CPUIDDLL_API int WINAPI cpuid(DWORD eax, DWORD ecx, LPDWORD peax, LPDWORD pebx, LPDWORD pecx, LPDWORD pedx);
CPUIDDLL_API int WINAPI hascpuid();
CPUIDDLL_API int WINAPI iddump(struct cpuidinfo *info, size_t bytes);
CPUIDDLL_API int WINAPI iddumponcore(struct cpuidinfo *info, size_t bytes, int core);

#ifdef __cplusplus 
}
#endif