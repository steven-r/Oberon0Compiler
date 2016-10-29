This generator creates code according to the RISC architecture defined
in the compiler book from N. Wirth

It's simply a 32 bit machine with 16 registers (_R0_ - _R15_ where _R15_ is used as a return instruction holder), a _PC_ and four 
flags 
+-------+--------------------------+
| **N** | Negative                 |
+-------+--------------------------+
| **Z** | Zero                     |
+-------+--------------------------+
| **C** | Carry Out                |
+-------+--------------------------+
| **V** | Overflow                 |
+-------+--------------------------+

More Information can be found in the corresponding book.