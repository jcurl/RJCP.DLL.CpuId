#ifndef RJCP_DIAGNOSTICS_NATIVE_WIN32_LIBRARYHANDLE_H
#define RJCP_DIAGNOSTICS_NATIVE_WIN32_LIBRARYHANDLE_H

#include "stdafx.h"

#include <type_traits>

namespace rjcp {
namespace native {
namespace win32 {

class ProcAddress {
 public:
  explicit ProcAddress() = default;
  explicit ProcAddress(FARPROC ptr) noexcept;

  template<typename T, typename = std::enable_if_t<std::is_function_v<T>>>
  operator T *() const noexcept {
    return reinterpret_cast<T *>(ptr_);
  }

 private:
  FARPROC ptr_;
};

class LibraryLoad {
 public:
  explicit LibraryLoad(LPCTSTR file_name) noexcept;
  LibraryLoad(const LibraryLoad &) = delete;
  LibraryLoad(LibraryLoad &&) = delete;
  auto operator=(const LibraryLoad &) -> LibraryLoad & = delete;
  auto operator=(LibraryLoad &&) -> LibraryLoad & = delete;
  ~LibraryLoad() noexcept;
  auto operator[](LPCSTR proc_name) const noexcept -> ProcAddress;

 private:
  HMODULE module_;
};

class ModuleGet {
 public:
  explicit ModuleGet(LPCTSTR file_name) noexcept;
  ModuleGet(const ModuleGet &) = delete;
  ModuleGet(ModuleGet &&) = delete;
  auto operator=(const ModuleGet &) -> ModuleGet & = delete;
  auto operator=(ModuleGet &&) -> ModuleGet & = delete;
  ~ModuleGet() noexcept = default;
  auto operator[](LPCSTR proc_name) const noexcept -> ProcAddress;

 private:
  HMODULE module_;
};

}  // namespace win32
}  // namespace native
}  // namespace rjcp
#endif
