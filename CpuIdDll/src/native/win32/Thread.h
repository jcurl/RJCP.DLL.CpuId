#ifndef RJCP_DIAGNOSTICS_NATIVE_WIN32_THREAD_H
#define RJCP_DIAGNOSTICS_NATIVE_WIN32_THREAD_H

#include <cstdint>
#include <memory>

namespace rjcp {
namespace native {
namespace win32 {

/// <summary>
/// Object for handling threads.
/// </summary>
/// <remarks>
/// We don't allow copying or moving of this object. Instantiate at the time you want to create an affinity for a
/// thread, and when it goes out of scope, the affinity is restored. Do not use this class more than once at a time.
/// </remarks>
class Thread {
 public:
  /// <summary>
  /// Initializes a new instance of the <see cref="Thread"/> class.
  /// </summary>
  /// <remarks>Is equivalent to not setting any thread affinity.</remarks>
  Thread() noexcept;

  /// <summary>
  /// Initializes a new instance of the <see cref="Thread"/> class.
  /// </summary>
  /// <param name="cpunum">The cpu to pin the current thread to.</param>
  Thread(unsigned int cpunum) noexcept;

  Thread(const Thread&) = delete;
  Thread(Thread&&) = delete;
  auto operator=(const Thread&) -> Thread& = delete;
  auto operator=(Thread&&) -> Thread& = delete;

  /// <summary>
  /// Finalizes an instance of the <see cref="Thread"/> class.
  /// </summary>
  /// <remarks>
  /// On destruction, the thread affinity is restored if it is the same as when this class was constructed.
  /// </remarks>
  ~Thread();

 private:
  struct state;
  std::unique_ptr<state> state_;
};

}  // namespace win32
}  // namespace native
}  // namespace rjcp
#endif
