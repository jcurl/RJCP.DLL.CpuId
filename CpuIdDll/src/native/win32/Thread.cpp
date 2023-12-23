#include "stdafx.h"

#include "native/win32/Thread.h"

#include "native/win32/kernel32/Kernel32.h"

namespace rjcp {
namespace native {
namespace win32 {

#if _WIN32 || _WIN64
#if _WIN64
static constexpr int cores = 64;
#else
static constexpr int cores = 32;
#endif
#else
#error Unknown Windows Variant for DWORD_PTR aliasing
#endif

// We define private state here for Windows, because we don't want to include <Windows.h> in the header file, which is
// likely to include that unwanted header everywhere.
struct Thread::state {
  DWORD_PTR affinity = 0;
  DWORD thread_id = 0;
};

Thread::Thread() noexcept : state_{std::make_unique<Thread::state>()} {
  unsigned int cpunum = kernel32::GetCurrentProcessorNumber();

  HANDLE current_thread = GetCurrentThread();
  DWORD_PTR new_mask = (DWORD_PTR)((DWORD_PTR)1 << cpunum);  // C4334 should explicitly cast
  DWORD_PTR old_affinity = SetThreadAffinityMask(current_thread, new_mask);
  if (!old_affinity) return;

  state_->affinity = old_affinity;
  state_->thread_id = GetCurrentThreadId();
}

Thread::Thread(unsigned int cpunum) noexcept : state_{std::make_unique<Thread::state>()} {
  if (cpunum >= cores) return;

  HANDLE current_thread = GetCurrentThread();
  DWORD_PTR new_mask = (DWORD_PTR)((DWORD_PTR)1 << cpunum);  // C4334 should explicitly cast
  DWORD_PTR old_affinity = SetThreadAffinityMask(current_thread, new_mask);
  if (!old_affinity) return;

  state_->affinity = old_affinity;
  state_->thread_id = GetCurrentThreadId();
}

Thread::~Thread() {
  // Windows doesn't have a `GetThreadAffinityMask` and we're not going to hack with a call to NtQueryInformationThread
  // as in https://stackoverflow.com/a/6624238.
  //
  // So don't use multiple instances of this in the same thread interleaved!
  if (state_ == nullptr || state_->thread_id == 0) return;

  // Don't update the affinity if called from a different thread.
  if (state_->thread_id != GetCurrentThreadId()) return;

  HANDLE current_thread = GetCurrentThread();
  SetThreadAffinityMask(current_thread, state_->affinity);
}

}  // namespace win32
}  // namespace native
}  // namespace rjcp
