using System;
using System.Collections.Generic;
using Interpreter;
namespace Interpreter.Reserved {
    partial class Functions {
        public static object If_Fu(SymbolTable s) { 
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
            if (boolean) {
                body.ExecuteByhInterpreter(new Interpreter(new Stack<object>(),Program.environment.interpreter.ActiveScope));
                return true;
            }
            else {
                return false;
            }

        }
        
    }
}