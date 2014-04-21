using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interaptor {
    class Interaptor {
        Stack<object> pStack;
        SymbolTable activeScope;

        public Interaptor() {
            pStack = new Stack<object>();
        }
        
        public void Process(Token t) {
            switch (t.type) {
                
                case Token.Type.Operator:
                    break;
                
                case Token.Type.FunctionCall:
                    //the name of the function is t.lexema

                    //get the number of aprands that this function resiave
                    int limit=this.activeScope.GetFunctionParametersCount(t.lexema);

                    //the parameters that will be passed to the function call
                    List<object> parameters = new List<object>();

                    for (int i = 0; i < limit; i++)
                        parameters.Add(pStack.Pop());

                    //push to the top of the stack the result
                    pStack.Push(this.activeScope.CallFunction(t.lexema, parameters));

                    break;
                case Token.Type.String:
                    pStack.Push(t.lexema);
                    break;
                case Token.Type.Double:
                    pStack.Push(Double.Parse(t.lexema));
                    break;
                case Token.Type.Integer:
                    pStack.Push(int.Parse(t.lexema));
                    break;
                
                case Token.Type.IdHEad:
                    pStack.Push(new Id(t.lexema));
                    break;
                case Token.Type.IdTail:
                    Id before = (Id)pStack.Peek();
                    before.AddPath(t.lexema);
                    break;
                case Token.Type.EOS:
                    foreach (object obj in pStack)
                        Console.WriteLine(obj.ToString());
                    break;
            }
        }
        private void ProcessOperator(string op) {
            
            switch (op) { 
                case "+":
                    pStack.Push((double)pStack.Pop() + (double)pStack.Pop());
                    break;
                case "-":
                    pStack.Push(-(double)pStack.Pop() + (double)pStack.Pop());
                    break;

                case "*":
                    pStack.Push((double)pStack.Pop() * (double)pStack.Pop());
                    break;
            }
        }
        
    }
}
