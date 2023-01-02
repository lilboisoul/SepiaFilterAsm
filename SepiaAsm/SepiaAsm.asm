; rcx/xmm0 - pixels
; rdx/xmm1 - sepia rates
; r8 - start index
; r9 - end index

.data
	;masks for shuffling input bytes
	bluemask_array  db 0, 80h, 80h, 80h,   3, 80h, 80h, 80h,   6, 80h, 80h, 80h,   9, 80h, 80h, 80h
	greenmask_array db 1, 80h, 80h, 80h,   4, 80h, 80h, 80h,   7, 80h, 80h, 80h,  10, 80h, 80h, 80h
	redmask_array   db 2, 80h, 80h, 80h,   5, 80h, 80h, 80h,   8, 80h, 80h, 80h,  11, 80h, 80h, 80h

	;masks for shuffling output bytes
	export_bluemask  db   0, 80h, 80h,   4, 80h, 80h,   8, 80h, 80h, 12, 80h, 80h, 80h, 80h, 80h, 80h
	export_greenmask db 80h,   0, 80h, 80h,   4, 80h, 80h,   8, 80h, 80h, 12, 80h, 80h, 80h, 80h, 80h
	export_redmask   db 80h, 80h,   0, 80h, 80h,   4, 80h, 80h,   8, 80h, 80h, 12, 80h, 80h, 80h, 80h

	;division by 3 = multiplication by 1/3
	variable dd 1051260355	;float 0.330000013
	
.CODE
SepiaAsmAlgorithm proc	
	;pushing registers onto the stack
	;push rbp
	;mov  rbp, rsp
	;push rcx
	;push rdx
	;push r8
	;push r9
	;push r10

	mov			r10, rdx					; sepia coefficient -> r11
	sub			r9, r8						; setting up counter
	mov			rdi, r9						; counter -> rdi	
	add			rcx, r8						; add start offset to rcx

	vbroadcastss	xmm13, variable			; prepare xmm3 register for multiplication by 0.33f

	;preparing masks for shuffling
	movdqa		xmm7,  oword ptr[bluemask_array]
	movdqa		xmm8,  oword ptr[greenmask_array]
	movdqa		xmm9,  oword ptr[redmask_array]
	movdqa		xmm10, oword ptr[export_bluemask]
	movdqa		xmm11, oword ptr[export_greenmask]
	movdqa		xmm12, oword ptr[export_redmask]

	mov			rax, r10					; prepare xmm4 register for adding sepia coefficient
	cvtsi2ss	xmm4, eax
	punpckldq	xmm4, xmm4					
	punpcklqdq	xmm4, xmm4
	cvtps2dq	xmm4, xmm4

sepiaLoop:
	cmp			edi, 0h						; if counter == 0, end loop
	je			endLoop			

	;loading process

	;load 4 pixels
	movlps		xmm5, qword ptr[rcx]		; move 64 bits to lower part of xmm5
	movd		xmm6, dword ptr[rcx + 8]	; move 32 bits to xmm6
	movlhps		xmm5, xmm6					; move 32 bits to higher part of xmm5
	
	;moving particular channels to xmm registers

	movdqa		xmm0, xmm5	;blue channels to xmm7
	pshufb		xmm0, xmm7 

	movdqa		xmm1, xmm5  ;green channels to xmm8
	pshufb		xmm1, xmm8

	movdqa		xmm2, xmm5  ;red channels to xmm9
	pshufb		xmm2, xmm9
	
	;gray-scaling process

	paddd		xmm0, xmm1  ;add all channels together, result is stored in xmm0
	paddd		xmm0, xmm2
	
	mulps		xmm0, xmm13 ;multiply the sum by 0.33f, result is gray-scaled pixel value


	;saving process
	movdqa		xmm1, xmm0  ;blue channels are ready to be saved, move them to xmm1 and shuffle accordingly
	pshufb		xmm1, xmm10

	paddusb		xmm0, xmm4	;add sepia coefficient to blue channel, result is green channel

	movdqa		xmm2, xmm0  ;green channels are ready to be saved, move them to xmm2 and shuffle accordingly
	pshufb		xmm2, xmm11

	paddusb		xmm0, xmm4	;add sepia coefficient to green channel, result is red channel

	movdqa		xmm3, xmm0  ;red channels are ready to be saved, move them to xmm3 and shuffle accordingly
	pshufb		xmm3, xmm12

	paddb		xmm3, xmm2  ;add all channels together so they are placed in the right order
	paddb		xmm3, xmm1

	;save 4 pixels
	movq		qword ptr[rcx], xmm3		;save 64 bits
	movhlps		xmm3, xmm3					;move remaining 32 bits to lower part of xmm3
	movd		dword ptr[rcx + 8], xmm3	;save 32 bits

	add		rcx, 12							;move index forward by 12 bytes
	sub		rdi, 12						    ;subtract 12 bytes from loop counter
	jmp		sepiaLoop						  

endLoop:
	;popping register from stack
	;pop r10
	;pop r9
	;pop r8
	;pop rdx
	;pop rcx
	;mov rsp, rbp
	;pop rbp
    ret
SepiaAsmAlgorithm endp
END