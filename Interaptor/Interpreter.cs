//#define _DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter {
    //foo {"a"}<-list {a 1 + <PrintL; } <-function
    public class Interpreter {
        Stack<object> pStack;
        SymbolTable activeScope;
        public Stack<object> ProcessStack { get { return pStack; } set { this.pStack = value; } }

        public SymbolTable ActiveScope { get { return activeScope; } set { this.activeScope = value; } }
        
        //falgs.
        bool exptFuCallFlag = false;
        //this flag is on when you are interpreting op code blocks.
        bool opBlockFlag = false;
        //stop flag
        bool stopFlag = false;
        public object Return(SymbolTable s){
            stopFlag = true;
            return new Reserved.ReturnObject(s.GetValue(new Id("toReturn")));
        }

        //the new block that will be pushed into the stack after the closer
        Opcodes newblock;


        public Interpreter() {
            pStack = new Stack<object>();
            activeScope = new SymbolTable(null);
        }
        public Interpreter(Stack<object> proccesStack, SymbolTable activeScope) {
            pStack = proccesStack;
            this.EnterScope(activeScope);
        }

        public void Reset() {
            pStack = new Stack<object>();
            exptFuCallFlag = false;
        }
        
        public void Process(LinkedList<object> input) {
            LinkedListNode<object> cur = input.First;       
                while (cur != null && !stopFlag) {
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
                        pStack.Push(Int32.Parse(t.lexema));
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
                    case Token.Type.Boolean:
                        if (t.lexema[0] == 't')//==true
                            pStack.Push(true);
                        else
                            pStack.Push(false);
                        break;
                    case Token.Type.EOS:
                        break;
                }
        }
       
        private void ProcessOperator(string op) {
            try {
                switch (op) {
                    //ordered pair operator.
                    case ",":
                        object last=pStack.Pop();
                        pStack.Push(new OrderedPair( pStack.Pop(), last));
                        break;
                    //function call operator.
                    case "<-":
                        exptFuCallFlag = true;
                        break;

                    //math operators.
                    case "+":
                        CallFunction(new Id("Add"));
                        break;
                    case "-":
                        pStack.Push(-(double)pStack.Pop() + (double)pStack.Pop());
                        break;

                    case "*":
                        pStack.Push((double)pStack.Pop() * (double)pStack.Pop());
                        break;
                    
                    //empty stack operator.
                    case ";":
                        pStack = new Stack<object>();
                        break;
                    //Def operator.
                    case "=":
                        CallFunction(new Id("Def"));
                        break;
                    //opcodes operator.
                    case "{":
                        opBlockFlag = true;
                        newblock = new Opcodes();
                        break;
                    
                    //boolean operators.
                    case "==":
                        pStack.Push(pStack.Pop().Equals(pStack.Pop()));
                        break;
                    case "&&":
                        object a =pStack.Pop();
                        object b =pStack.Pop();
                        pStack.Push((bool)a && (bool)b);
                        break;
                    case "||":
                        pStack.Push((bool)pStack.Pop() || (bool)pStack.Pop());
                        break;
                    case ">>":
                        pStack.Push(Compare(pStack.Pop(), pStack.Pop(),false));
                        break;
                    case "<<":
                        pStack.Push(Compare(pStack.Pop(), pStack.Pop(), true));
                        break;
                    case "!":
                        pStack.Push(!(bool)pStack.Pop());
                        break;
                    
                    //AND OR XOR
                    
                   
                    default: 
                        throw new Exception("invalid Operator");
                        break;
                }
            }
            catch (InvalidOperationException e) {
                throw new Exception("Noth enoth openders to continue the operation");
            }
        }
        public bool Compare(object a, object b, bool isAbigger) {
            if (!(a is IComparable && b is IComparable))
                throw new Exception("a or b are not comparable");
            IComparable A = a as IComparable;
            IComparable B = b as IComparable;
            if (isAbigger)
                return (A.CompareTo(B) > 0);
            return (B.CompareTo(A) > 0);

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
#if _DEBUG
            Console.WriteLine("Entering Scope " + t);
#endif
            this.activeScope = t;
        }
        public void ScopeOut() {
#if _DEBUG
            Console.Write("Leaving Scope " + this.ActiveScope);
#endif
            this.activeScope = this.activeScope.Perent;
            if (activeScope == null)
                throw new Exception(" you are trying to scope out of the Last scoup");
#if _DEBUG
            Console.WriteLine(" to scope " + this.ActiveScope);
#endif
        }
        

        public void CallFunction(Id name) {
            
#if _DEBUG
            Console.WriteLine("Program calling function "+name+" from scope "+this.activeScope.GetHashCode());
#endif
            //the id of the function is obj.lexema

            //get the fucnion block
            
            FunctionBLock funBlock =(FunctionBLock)this.activeScope.GetValue(name);

            //get the number of aprands that this function resiave
            int limit = funBlock.ParametersCount;
            //the parameters that will be passed to the function call
            List<object> parameters = new List<object>();

            for (int i = 0; i < limit; i++)
                    parameters.Add(pStack.Pop());
            

            Interpreter t = new Interpreter(this.ProcessStack, funBlock.GetCallSymbolTable(this.ActiveScope, parameters));
            t.activeScope.AddFunction("return", new SystemFunction(new RDelegate(t.Return)), "toReturn");
            funBlock.Executable.ExecuteByhInterpreter(t);
        }
        public override string ToString() {
            return "i@"+this.GetHashCode();
        }

       
    }
    
}
