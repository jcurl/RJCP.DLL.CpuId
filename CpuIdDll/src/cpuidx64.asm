		option casemap: none      ; case sensitive

		.code

; See http://msdn.microsoft.com/en-us/library/zthk2dkh.aspx
; R11+48 pedx
; R11+40 pecx
; R11+32                 R9  = pebx
; R11+24                 R8  = peax
; R11+16                 RDX = vecx
; R11+8                  RCX = veax
; R11+0 Callers EIP
cpuidl proc
		mov r11, rsp
		push rbx
		mov rax, rcx
		mov rcx, rdx
		cpuid

		mov dword ptr [r8], eax
		mov dword ptr [r9], ebx
		mov rax, qword ptr [r11+40]
		mov dword ptr [rax], ecx
		mov rax, qword ptr [r11+48]
		mov dword ptr [rax], edx

		pop rbx
		xor eax,eax
		ret
cpuidl	endp

; To be called on a 486 processor or later
; EAX != 0  =>  CPUID instruction exists
cpuidt proc
		mov rax, 1
		ret
cpuidt endp
end