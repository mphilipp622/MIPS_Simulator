.data
  msg1: .asciiz "\nPlease enter a number: "
  msg2: .asciiz "n! = "

.text
.globl main

main:
  # Display msg1, read the number, save it to $t0
  li   $v0, 4
  la   $a0, msg1
  syscall
  li   $v0, 5
  syscall
  addu $t0, $v0, $zero

  # Copy $t0 to $a0 and call fact
  addu $a0, $t0, $zero
  jal  fact

  # Copy $v0 to $t0, print msg2, copy $t0 to $a0 to display result
  addu $t0, $v0, $zero
  li   $v0, 4
  la   $a0, msg2
  syscall
  li   $v0, 1
  addu $a0, $t0, $zero
  syscall

  # Exit
  li $v0, 10
  syscall

# parameter:
#   $a0 - num
#
# fact :: Int -> Int
# fact 0 = 1
# fact 1 = 1
# fact n = n * fact (n - 1)
fact:
  # constant 1
  addi $t0, $zero, 1

  # base case (n = 0)
  bne  $a0, $zero, BASE1
  addu $v0, $t0, $zero
  jr   $ra

  # base case (n = 1)
  BASE1:
    bne  $a0, $t0, RECF
    addu $v0, $t0, $zero
    jr   $ra

  # recursive case (n >= 2)
  RECF:
    # store $ra and $a0 to stack
    addi $sp, $sp, -8
    sw   $ra, 0($sp)
    sw   $a0, 4($sp)

    # perform recursion
    subu $a0, $a0, $t0
    jal  fact
    lw   $a0, 4($sp)
    mult $a0, $v0
    mflo $v0

    # load $ra from stack and return
    lw   $ra, 0($sp)
    addi $sp, $sp, 8
    jr   $ra
