using System;
using System.Collections.Generic;
namespace Interpreter {
    class Token {

        public Token.Type type;
        public string lexema;

        public Token(string lexema, Token.Type type) {
            this.type = type;
            this.lexema = lexema;
        }

        public enum Type {
            IdHead,
            IdTail,
            IdEnd,
            IdSingle,
            FunctionCall,
            Integer,
            String,
            Double,
            Operator,
            EOS,
            Indexer,
        }
    }
}
