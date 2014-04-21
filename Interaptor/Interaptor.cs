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
            }
        }
        public void ProcessOperator(string op) { 
            
        }
    }
}
