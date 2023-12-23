#ifndef RJCP_DIAGNOSTICS_NATIVE_WIN32_KERNEL32_H
#define RJCP_DIAGNOSTICS_NATIVE_WIN32_KERNEL32_H

#include "stdafx.h"

namespace rjcp {
namespace native {
namespace win32 {
namespace kernel32 {

auto GetCurrentProcessorNumber() -> DWORD;

}  // namespace kernel32
}  // namespace win32
}  // namespace native
}  // namespace rjcp
#endif
