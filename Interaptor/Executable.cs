
using System;
using System.Collections.Generic;

namespace Interaptor {
    interface IExecutable {
        object ExecuteWithTable(SymbolTable tble);
    }
    class Opcodes : IExecutable {
        List<Token> ops;
        public object ExecuteWithTable(SymbolTable tble) {
            return null;
        }
    }
    class ReservedFunction: IExecutable {

        public delegate object ReservedFuncDelegate(SymbolTable tble);

        ReservedFuncDelegate toDo;

        public ReservedFunction(ReservedFuncDelegate toDo) {
            this.toDo = toDo;
        }

        public object ExecuteWithTable(SymbolTable tble) {
            return toDo(tble);
        }
    }
}
