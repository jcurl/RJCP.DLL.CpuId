		.686                      ; create 32 bit code, required for "cpuid" instruction
		.model flat, stdcall      ; 32 bit memory model
		option casemap: none      ; case sensitive
		option language: stdcall

		.code

;stdcall calling convention used for 32-bit
; EBP+28 pedx:NEAR PTR DWORD
; EBP+24 pecx:NEAR PTR DWORD
; EBP+20 pebx:NEAR PTR DWORD
; EBP+16 peax:NEAR PTR DWORD
; EBP+12 vecx:DWORD
; EBP+8  veax:DWORD
; EBP+4  callers EIP
; EBP+0  callers EBP
cpuidl proc veax:DWORD, vecx:DWORD, peax:NEAR PTR DWORD, pebx:NEAR PTR DWORD, pecx:NEAR PTR DWORD, pedx:NEAR PTR DWORD
		;push ebp
		;mov ebp,esp
		push ebx
		mov eax, veax
		mov ecx, vecx
		cpuid
		push eax
		mov eax, pedx
		mov dword ptr [eax],edx
		mov eax, pecx
		mov dword ptr [eax],ecx
		mov eax, pebx
		mov dword ptr [eax],ebx
		pop ebx
		mov eax, peax
		mov dword ptr [eax],ebx
		pop ebx
		xor eax,eax
		;mov esp,ebp
		;pop ebp
		;ret 24
		ret
cpuidl	endp

; To be called on a 486 processor or later
; EAX != 0  =>  CPUID instruction exists
cpuidt proc
		pushfd
		pop eax				; Get EFLAGS in EAX
		mov ecx, eax		; ECX is an original copy of EFLAGS
		xor eax, 200000h	; Toggle bit 21 of the EFLAGS register
		push eax			;
		popfd				;
		pushfd				;
		pop eax				; with result in EAX

		xor eax,ecx			; Check if bit can be toggled
							; If it can be toggled, EAX=200000, implies CPU supports CPUID
		ret
cpuidt endp
end
