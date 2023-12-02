@echo off
echo Signing...
"c:\Program Files (x86)\Windows Kits\10\Tools\bin\i386\signtool.exe" sign /fd sha1 /sha1 516ee045d8f8b691f16926af7fff0974397e6025 Debug\x86\cpuid.dll
"c:\Program Files (x86)\Windows Kits\10\Tools\bin\i386\signtool.exe" sign /fd sha1 /sha1 516ee045d8f8b691f16926af7fff0974397e6025 Debug\x64\cpuid.dll
"c:\Program Files (x86)\Windows Kits\10\Tools\bin\i386\signtool.exe" sign /fd sha1 /sha1 516ee045d8f8b691f16926af7fff0974397e6025 /tr http://timestamp.digicert.com Release\x86\cpuid.dll
"c:\Program Files (x86)\Windows Kits\10\Tools\bin\i386\signtool.exe" sign /fd sha1 /sha1 516ee045d8f8b691f16926af7fff0974397e6025 /tr http://timestamp.digicert.com Release\x64\cpuid.dll

:done
pause
