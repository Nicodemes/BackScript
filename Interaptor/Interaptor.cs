using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interaptor {
    class Interaptor {

        public Stack<object> pStack;
        SymbolTable activeScope;
        public Stack<object> ProcessStack { get { return pStack; } }

        public SymbolTable ActiveScope { get { return activeScope; } set { this.activeScope = value; } }
        
        //falgs.
        bool exptFuCallFlag = false;
        //this flag is on when you are interpreting op code blocks.
        bool opBlockFlag = false;
        //the new block that will be pushed into the stack after the closer
        Opcodes newblock;


        public Interaptor() {
            pStack = new Stack<object>();
            activeScope = new SymbolTable(null);
        }

        public void Reset() {
            pStack = new Stack<object>();
            exptFuCallFlag = false;
        }
        
        public void Process(LinkedList<object> input) {
            LinkedListNode<object> cur = input.First;
            while (cur != null) {
                this.Process(cur.Value);
                cur = cur.Next;
            }
        }
        private void Process(object obj) { 
            if(obj is Token)
                Process((Token)obj);
            else
                pStack.Push(obj);
        }
        private void Process(Token t) {

            if (exptFuCallFlag) 
                ExcpectFunctionCall(t);

            else if (opBlockFlag) {
                if (t.type == Token.Type.Operator && t.lexema == "}") {
                    opBlockFlag = false;
                    this.pStack.Push(newblock);
                }
                else {
                    newblock.Append(t);
                }
            }
            else
                switch (t.type) {
                    case Token.Type.Indexer:
                        pStack.Push(int.Parse(t.lexema));
                        CallFunction(new Id("~indexer"));
                        break;
                    case Token.Type.Operator:
                        ProcessOperator(t.lexema);
                        break;
                
                    case Token.Type.FunctionCall:
                        CallFunction(new Id(t.lexema));
                        break;
                    case Token.Type.String:
                        pStack.Push(t.lexema);
                        break;
                    case Token.Type.Double:
                        pStack.Push(Double.Parse(t.lexema));
                        break;
                    case Token.Type.Integer:
                        pStack.Push(Double.Parse(t.lexema));
                        break;
                
                    case Token.Type.IdHead:
                        pStack.Push(new Id(t.lexema));
                        break;
                    case Token.Type.IdSingle:
                        pStack.Push(new Id(t.lexema));
                        
                        break;

                    case Token.Type.IdTail:
                        Id before = (Id)pStack.Peek();
                        before.AddPath(t.lexema);
                        break;
                    case Token.Type.EOS:
                        break;
                }
        }
       
        private void ProcessOperator(string op) {
            try {
                switch (op) {
                    case "<-":
                        exptFuCallFlag = true;
                        break;
                    case "+":
                        CallFunction(new Id("Add"));
                        break;
                    case "-":
                        pStack.Push(-(double)pStack.Pop() + (double)pStack.Pop());
                        break;

                    case "*":
                        pStack.Push((double)pStack.Pop() * (double)pStack.Pop());
                        break;
                    case ";":
                        pStack = new Stack<object>();
                        break;
                    case "=":
                        CallFunction(new Id("Be"));
                        break;
                    case "{":
                        opBlockFlag = true;
                        newblock = new Opcodes(this);
                        break;
                    

                    default: 
                        throw new Exception("invalid Operator");
                        break;
                }
            }
            catch (InvalidOperationException e) {
                throw new Exception("Noth enoth openders to continue the operation");
            }
        }

        void ExcpectFunctionCall(Token t) {
            if (t.type == Token.Type.IdSingle) {
                CallFunction(new Id(t.lexema));
                exptFuCallFlag = false;
            }
            else if (t.type == Token.Type.IdHead) {
                pStack.Push(new Id(t.lexema));
            }
            else if (t.type == Token.Type.IdTail) {
                Id before = (Id)pStack.Peek();
                before.AddPath(t.lexema);
            }
            else if (t.type == Token.Type.IdEnd) {
                Id before = (Id)pStack.Pop();
                before.AddPath(t.lexema);
                CallFunction(before);
                exptFuCallFlag = false;
            }
            else
                throw new Exception("invalid function call");
        }

        public void EnterScope(SymbolTable t) {
         
            this.activeScope = t;
        }
        public void ScopeOut() {
           
            this.activeScope = this.activeScope.Father;
            if (activeScope == null)
                throw new Exception(" you are trying to scope out of the last scoup");
        }


        public void CallFunction(Id name) {
            //the id of the function is obj.lexema

            //get the number of aprands that this function resiave
            int limit = this.activeScope.GetFunctionParametersCount(name);

            //the parameters that will be passed to the function call
            List<object> parameters = new List<object>();

            for (int i = 0; i < limit; i++)
                parameters.Add(pStack.Pop());

            //push to the top of the stack the result
            object returned = this.activeScope.CallFunction(name, parameters);
            if (!(returned is Void))
                pStack.Push(returned);

        }
    }
}
