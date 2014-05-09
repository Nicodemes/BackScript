using System;
namespace Interpreter.Reserved {
    partial class Functions {
        //prints a variable to the screan
        static Interpreter raptor = Program.interpreter;
        public static object Peek_Fu(SymbolTable s) {
            Console.Write(Program.interpreter.ProcessStack.Peek());
            return new Void();
        }
    }
}