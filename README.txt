Mark Philipp
109941708
CSCI 113

MIPS SIMULATOR
-----------------
How To Run
-----------------
There is an executable file inside the root folder called "MipsSimulator.exe". You should be able to run this file on Windows. There is also a Linux file called MipsSimulator.x86 in the root folder. I do not have a linux machine to test the file on so it may or may not work.

NOTE: a call to JAL at the end of the program that seeks to exit the program will close the program. This can be a problem if you want to run a full execution of some code because the program will execute and JAL will tell the program to quit. This means you may not be able to see the results if the program quits too soon.

-----------------
Bugs
-----------------
You cannot open a new file once a file has already been opened. You will need to close the program and reopen it to open a separate file, unfortunately.

I've had some jump addressing issues in files that do NOT contain the first few lines that QTspim fills in on the assembly files. QTSpim always adds this data to the top of the file:

[00400000] 8fa40000  lw $4, 0($29)            ; 183: lw $a0 0($sp) # argc 
[00400004] 27a50004  addiu $5, $29, 4         ; 184: addiu $a1 $sp 4 # argv 
[00400008] 24a60004  addiu $6, $5, 4          ; 185: addiu $a2 $a1 4 # envp 
[0040000c] 00041080  sll $2, $4, 2            ; 186: sll $v0 $a0 2 
[00400010] 00c23021  addu $6, $6, $2          ; 187: addu $a2 $a2 $v0 
[00400014] 0c100009  jal 0x00400024 [main]    ; 188: jal main 
[00400018] 00000000  nop                      ; 189: nop 
[0040001c] 3402000a  ori $2, $0, 10           ; 191: li $v0 10 
[00400020] 0000000c  syscall                  ; 192: syscall # syscall 10 (exit) 

I was unable to get my jump and branch addressing to work consistently without this code existing in the hex file.

-------------------
Approach
-------------------
I decided to use the Unity Game Engine to develop the project. I chose this because I prefer the ease of use of C# and the tools the language contains. Additionally, I have a lot of experience with Unity and I wanted to get better with using Unity's UI features. This added some extra work due to the code I had to implement for the UI elements.

For the code, I knew from the start I wanted to use hashtables. In C#, hashtables are implement as a class called Dictionary<type, type>. I stored all the registers into a hashtable using a <byte, Register> pair, where Register is a class I created. So I can get a register by its byte value. E.G:
registers[23].value = 12;

All the memory segments are implemented using hashtables with a <uint, int> pair. uint in C# is just an unsigned integer. The unsigned integer key value would be the 32-bit address of the memory. The integer is the data stored in that memory location. This allows easy lookup such as staticData[0x00100004] = 0x00000023;

I made classes to handle Parsing instructions, Writing instructions to screen, Handling operations, reading instructions from a file, and UI handling.

-------------------
The Code
-------------------
To find the code, go into the "Assets" folder. Any file with a .cs extension is a C# file that can be viewed using a text editor or most IDE's.

OperationManager.cs = Handles operations. Add, Sub, BEQ, JAL, etc.

Globals.cs = Specifies global containers such as for static memory, text memory, stack memory. 	Also contains registers for $HI, $LO, and $PC

InstructionReader.cs = Reads instructions and gets opcode, rs, rt, rd, shamt, funct, address from it. Also will print the decoded program to screen.

MemoryInitializer.cs = Initializes stack, static, and text memory on program load.

Parser.cs = Reads the .s file and sends instructions to the InstructionReader. Takes data segment and tells MemoryInitializer to initialize memory.

Register.cs = Class for Registers. contains a name and a value and logic for using signed or unsigned data.

RegisterManager.cs = Class that holds a hashtable of Registers 0 - 31. Used by OperationManager to get registers from byte codes.

All the other .cs files are UI related.