using System;
using System.Collections.Generic;
namespace Interpreter.Reserved {
    partial class Functions {
        public static object For_Fu(SymbolTable s) {
            Opcodes body;
            int n;
            try {
                body = (Opcodes)s.GetValue(new Id("~body"));
            }
            catch {
                throw new Exception("body expected");
            }
            try {
                n = (int)s.GetValue(new Id("~int"));
            }
            catch {
                throw new Exception("boolean value expected");
            }
            SymbolTable scope = Program.environment.interpreter.ActiveScope.GetNewAnonymicScope();
            Stack<object> stck = new Stack<object>();
           for (int i = 0; i < n; i++){
                body.ExecuteByhInterpreter(new Interpreter(stck, scope));
            }
            return new Void();
        }
    }
}