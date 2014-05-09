using System;
namespace Interpreter.Reserved {
    partial class Functions {
        public static object Def_Fu(SymbolTable s) {
            Opcodes body = (Opcodes)s.GetValue(new Id("~body"));
            bool booolean =(bool) s.GetValue(new Id("~bool"));
            if (booolean) { 
                body.ExecuteByhInterpreter(F
            }
        }
    }
}