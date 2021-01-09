@echo off
echo Signing...
"c:\Program Files (x86)\Windows Kits\10\Tools\bin\i386\signtool.exe" sign /fd sha1 /sha1 2fda16f7adf7153e17d4bf3d36adc514a736cdf4 Debug\x86\cpuid.dll
"c:\Program Files (x86)\Windows Kits\10\Tools\bin\i386\signtool.exe" sign /fd sha1 /sha1 2fda16f7adf7153e17d4bf3d36adc514a736cdf4 Debug\x64\cpuid.dll
"c:\Program Files (x86)\Windows Kits\10\Tools\bin\i386\signtool.exe" sign /fd sha1 /sha1 2fda16f7adf7153e17d4bf3d36adc514a736cdf4 /tr http://timestamp.digicert.com Release\x86\cpuid.dll
"c:\Program Files (x86)\Windows Kits\10\Tools\bin\i386\signtool.exe" sign /fd sha1 /sha1 2fda16f7adf7153e17d4bf3d36adc514a736cdf4 /tr http://timestamp.digicert.com Release\x64\cpuid.dll

:done
pause
