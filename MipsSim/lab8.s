
.text
.globl main

main:
	addu $s0, $ra, $0
	li $v0, 4
	la $a0, msg1
	syscall

	# get integer 1
	li $v0, 5
	syscall

	addu $t0, $v0, $0 # move first int into t0

	li $v0, 4
	la $a0, msg1
	syscall

	# get integer 2
	li $v0, 5
	syscall

	addu $t1, $v0, $0 # move second int into t1
	addu $a0, $t0, $0 # move first int into a0
	addu $a1, $t1, $0 # move second into into a1

	jal power

	addu $t5, $v0, $0 # move result into t5

	li $v0, 4
	la $a0, msg2
	syscall # Print First Result Message

	li $v0, 1
	addu $a0, $t5, $0 # move result into a0
	syscall # print result

	li $v0, 4
	la $a0, msg3
	syscall # print line ending
	
	addu $ra, $0, $s0
	jr $ra

power:
	addi $sp, $sp, -4 # allocate stack data
	addi $t0, $0, 1 # set t0 to 1
	addu $t4, $t4, $0  # move 0 into t4
	addu $t1, $a1, $0 # move y into t1
	
	loop:
		slt $t3, $t4, $t1 # 0 < t1?
		beq $t3, $0, finish # if 0 < t1 == false
		mul $t0, $t0, $a0 # temp = temp * x
		addi $t1, $t1, -1 # i--
		j loop

	finish:
		addu $v0, $t0, $0 # move final result into v0
		addi $sp, $sp, 4 # restore stack
		jr $ra
		
		
.data
msg1:	.asciiz "Please enter an integer number: "
msg2:	.asciiz "\tResult: "
msg3:	.asciiz "\n"