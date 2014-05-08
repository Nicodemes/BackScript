#define _DEBUG
using System;
using System.Collections.Generic;

namespace Interpreter {
    interface IExecutable {
        void ExecuteByhInterpreter(Interpreter machine);
    }
    class Opcodes : IExecutable {
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

        public void ExecuteByhInterpreter(Interpreter machine) {
#if _DEBUG
            Console.WriteLine("Opcodes Executing with  machine "+machine);
#endif
            //remember the old pStack of the machine and replace with a new stack segment.
            ReturnStack s= new ReturnStack(machine);
            machine.Process(ops);
            s.Return();
        }
        class ReturnStack {
            public Stack<object> perent;
            private Interpreter r;
            public ReturnStack(Interpreter r ) {
                this.r = r;
                this.perent = r.ProcessStack;
                r.ProcessStack = new Stack<object>();
            }
            public void Return() {
                
                //check the numberof objects in the cur returnstack
                if (r.ProcessStack.Count > 0) {
                   
                    object toReturn = r.ProcessStack.Peek();
                    //check if the returned object is actually returnd
                    
                    if (!(toReturn is ReturnObject))
                        throw new Exception(" Return expeted");
                    //if it is , push it as reutned. if it is void, fo nothing.
                    if (toReturn is Void)
                         r.ProcessStack = perent;
                    else
                        perent.Push((toReturn as ReturnObject).toReturn);
                }
                else {
                    r.ProcessStack = perent;
                }
                
            }
        }
    }
    class ReservedFunction: IExecutable {

        public delegate object ReservedFuncDelegate(SymbolTable tble);

        ReservedFuncDelegate toDo;

        public ReservedFunction(ReservedFuncDelegate toDo) {
            this.toDo = toDo;
        }

        public void ExecuteByhInterpreter(Interpreter machine){
            //Console.WriteLine("Executing with table " + tble.GetHashCode());
            machine.ProcessStack.Push(toDo(machine.ActiveScope));
        }
    }
}
