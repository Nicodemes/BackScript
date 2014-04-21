using System;
using System.Collections.Generic;
namespace Interaptor {
    class Token {

        public Token.Type type;
        public string lexema;

        public Token(string lexema, Token.Type type) {
            this.type = type;
            this.lexema = lexema;
        }

        public enum Type {
            IdHEad,
            IdTail,
            FunctionCall,
            Integer,
            String,
            Double,
            Operator,
            EOS,
        }
    }
}
