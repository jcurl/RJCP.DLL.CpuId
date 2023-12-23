// dllmain.cpp : Defines the entry point for the DLL application.

#include "stdafx.h"

#include "cpuid/CpuIdFactory.h"
#include "cpuid/CpuIdNativeConfig.h"
#include "globalstate.h"

#include <memory>

using namespace rjcp::diagnostics;

BOOL APIENTRY DllMain([[maybe_unused]] HMODULE hModule, [[maybe_unused]] DWORD ul_reason_for_call,
                      [[maybe_unused]] LPVOID lpReserved) {
  switch (ul_reason_for_call) {
    case DLL_PROCESS_ATTACH: {
      // This library always uses the CPUID instruction.
      cpuid::CpuIdNativeConfig config{};
      auto factory = cpuid::MakeCpuIdFactory(config);
      global = std::make_unique<GlobalState>(std::move(factory));
      break;
    }
    case DLL_PROCESS_DETACH: {
      global.reset();
      break;
    }
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
      break;
  }
  return TRUE;
}
