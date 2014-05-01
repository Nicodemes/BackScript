#define _DEBUG
using System;
using System.Collections.Generic;

namespace Interaptor {
    interface IExecutable {
        object ExecuteWithTable(SymbolTable tble);
    }
    class Opcodes : IExecutable {
        Interaptor machine;
        public LinkedList<object> ops;

        public Opcodes() {
            ops = new LinkedList<object>();
        }
        bool flag = false;
        Opcodes newblock;
        public void Append(Token t) {
            if (t.type == Token.Type.Operator && t.lexema == "{") {
                flag = true;
                newblock = new Opcodes();
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
#if _DEBUG
            Console.WriteLine("Executing with table " + tble);
#endif
            this.machine = new Interaptor();
            this.machine.EnterScope(tble);
            machine.Process(ops);
            object toReturn;
            if(machine.ProcessStack.Count>0)
                toReturn=machine.ProcessStack.Peek();
            else
                toReturn = new Void();
#if _DEBUG
            Console.WriteLine(" returning "+toReturn);
#endif
            return toReturn;
        }
    }
    class ReservedFunction: IExecutable {

        public delegate object ReservedFuncDelegate(SymbolTable tble);

        ReservedFuncDelegate toDo;

        public ReservedFunction(ReservedFuncDelegate toDo) {
            this.toDo = toDo;
        }

        public object ExecuteWithTable(SymbolTable tble) {
            //Console.WriteLine("Executing with table " + tble.GetHashCode());
            return toDo(tble);
        }
    }
}
