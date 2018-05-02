# assumptions
# $t5 (13) = 0x53fc
# $t6 (14) = 0x6405
# $s0 (16) = 0x10010000
# $t8 (24) = 0x0
# $t9 (25) = 0x0

.text
.align 2
.globl main

main:
  and $t0, $t5, $t6
  or  $t1, $t5, $t6
  add $t2, $t5, $t6
  sub $t3, $t5, $t6
  slt $t4, $t5, $t6

  lw $s1, 0($s0)
  lw $s2, 4($s0)
  lw $s3, 8($s0)
  lw $s4, 12($s0)

  sw $t0, 16($s0)
  sw $t1, 20($s0)
  sw $t2, 24($s0)
  sw $t3, 28($s0)
  sw $t4, 32($s0)

  beq $t8, $t9, AAAHHHH

AAAHHHH:
  and $t0, $s1, $s2
  bne $s1, $s2, TEETH

TEETH:
  j FINAL

FINAL:
  add $t1, $t1, $t1

.end main

.data
  list: .word 35, 16, 42, 19