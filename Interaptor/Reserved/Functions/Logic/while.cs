using System;
using System.Collections.Generic;
using Interpreter;
namespace Interpreter.Reserved {
    partial class Functions {
        public static object While_Fu(SymbolTable s) { 
            Opcodes body;
            bool boolean ;
            try{
                body=(Opcodes)s.GetValue(new Id("~body"));
            }catch{
                throw new Exception("body expected");
            }
            try {
               boolean = (bool)s.GetValue(new Id("~bool"));
            }
            catch {
                throw new Exception("boolean value expected");
            }
            SymbolTable scope = Program.environment.interpreter.ActiveScope.GetNewAnonymicScope();
            Stack<object> stck=new Stack<object>();
            while (boolean) {
                body.ExecuteByhInterpreter(new Interpreter(stck, scope));
            }
            return new Void();
        }
        
    }
}