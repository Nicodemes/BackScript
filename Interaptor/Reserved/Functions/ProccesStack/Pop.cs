using System;
namespace Interpreter.Reserved {
    partial class Functions {
        //pops one item from the pStack
        public static object Pop_Fu(SymbolTable s) {
            raptor.ProcessStack.Pop();
            return new Void();
        }
    }
}