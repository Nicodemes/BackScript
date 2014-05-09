using System;
namespace Interpreter.Reserved {
    partial class Functions {
        public static object ReadLine_Fu(SymbolTable s) {
            return Console.ReadLine();
        }
    }
}