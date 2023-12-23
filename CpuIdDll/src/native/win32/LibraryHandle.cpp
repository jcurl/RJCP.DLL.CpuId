#include "native/win32/LibraryHandle.h"

namespace rjcp {
namespace native {
namespace win32 {

ProcAddress::ProcAddress(FARPROC ptr) noexcept : ptr_{ptr} { }

LibraryLoad::LibraryLoad(LPCTSTR file_name) noexcept {
  if (file_name) module_ = LoadLibrary(file_name);
}

LibraryLoad::~LibraryLoad() noexcept {
  if (module_) FreeLibrary(module_);
}

auto LibraryLoad::operator[](LPCSTR proc_name) const noexcept -> ProcAddress {
  if (module_) return ProcAddress(GetProcAddress(module_, proc_name));
  return ProcAddress();
}

ModuleGet::ModuleGet(LPCTSTR file_name) noexcept {
  if (file_name) module_ = GetModuleHandle(file_name);
}

auto ModuleGet::operator[](LPCSTR proc_name) const noexcept -> ProcAddress {
  if (module_) return ProcAddress(GetProcAddress(module_, proc_name));
  return ProcAddress();
}

}  // namespace win32
}  // namespace native
}  // namespace rjcp
