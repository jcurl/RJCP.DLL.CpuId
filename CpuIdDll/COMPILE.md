# Compiling CpuId.dll

This project requires Visual Studio 2012. It is compiled with Windows XP
compatibility.

Upgrading it to Visual Studio 2019 will fail due to a missing header in the file
`ResourceXX.rc`.

```cpp
#include "afxres.h"
```
