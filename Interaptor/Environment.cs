using System;
using System.Collections.Generic;

namespace Interpreter {
    class Environment {
        public Interpreter interpreter;
        public  Tokenizer lexicalAnaliser;
        public Registry registry;
        public Environment() {
            Init();
        }
        public void Init() {
            interpreter= new Interpreter();
             registry= new Registry(interpreter.ActiveScope);
        }
    }
}
