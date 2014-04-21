using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interaptor {
    class Interaptor {
       
        Stack<object> pStack;
        SymbolTable activeScope;
        public Stack<object> ProcessStack { get { return pStack; } }

        public SymbolTable ActiveScope { get { return activeScope; } set { this.activeScope = value; } }

        public Interaptor() {
            pStack = new Stack<object>();
            activeScope = new SymbolTable(null);
        }

        public void Reset() {
            pStack = new Stack<object>();
        }

        public void Process(Token t) {
            switch (t.type) {
                
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
                
                case Token.Type.IdHEad:
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
        public void Process(LinkedList<Token> input){
            LinkedListNode<Token> cur = input.First;
            while (cur != null) {
                this.Process(cur.Value);
                cur = cur.Next;
            }
        }
       
        private void ProcessOperator(string op) {
            try {
                switch (op) {
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
                        this.EnterScope(activeScope.AddNewAnonymicScope());
                        break;
                    case "}":
                        this.ScopeOut();
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

        public void EnterScope(SymbolTable t) {
         
            this.activeScope = t;
        }
        public void ScopeOut() {
           
            this.activeScope = this.activeScope.Father;
            if (activeScope == null)
                throw new Exception(" you are trying to scope out of the last scoup");
        }


        public void CallFunction(Id name) {
            //the id of the function is t.lexema

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
