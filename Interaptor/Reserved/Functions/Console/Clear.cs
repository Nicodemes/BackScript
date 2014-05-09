using System;
namespace Interpreter.Reserved {
    partial class Functions {
        //clears the screan 
        public static object Clear_Fu(SymbolTable s) {
            Console.Clear();
            return new Void();
        }
    }
}