
using System;
using System.Collections.Generic;

namespace Interaptor {
    interface IExecutable {
        object ExecuteWithTable(SymbolTable tble);
    }
    class Opcodes : IExecutable {
        Interaptor machine;
        public LinkedList<object> ops;

        public Opcodes(Interaptor machine) {
            ops = new LinkedList<object>();
            this.machine = machine;
        }
        bool flag = false;
        Opcodes newblock;
        public void Append(Token t) {
            if (t.type == Token.Type.Operator && t.lexema == "{") {
                flag = true;
                newblock = new Opcodes(this.machine);
            }
            if (flag) {
                if (t.type == Token.Type.Operator && t.lexema == "}") {
                    flag = false;
                    this.ops.AddLast(newblock);
                }
                else 
                    newblock.Append(t);   
            }
            else 
                ops.AddLast(t);
        }
        
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
