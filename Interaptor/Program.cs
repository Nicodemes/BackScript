using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interaptor {
    class Program {
        
        static void Main(string[] args) {
            SymbolTable tbl = new SymbolTable(null);
            tbl.AddVariable("A", "Hello ");
            
            ReservedFunction foo = new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(HelloWorld));
            


            tbl.AddFunction("fun1", new List<string>(0) , foo);
            
            Console.Write(tbl.CallFunction("fun1",new List<object>(0)));
            
            Console.ReadLine();
        }
        static object HelloWorld(SymbolTable s) {
            Console.Write(s.GetVariable("A"));
            return "World!";
        }
    }
    
}
