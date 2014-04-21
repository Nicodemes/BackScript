using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interaptor {
    class Program {
        
        static void Main(string[] args) {
            SymbolTable tbl = new SymbolTable(null);
            tbl.AddVariable("WhatToSayFirst", "Hello ");
            
            ReservedFunction foo = new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(HelloWorld));


            List<string> parameters = new List<string>();
            parameters.Add("WhatToSayNext");
            tbl.AddFunction("fun1", parameters , foo);

            List<object> toPass = new List<object>();
            toPass.Add("World!");
            Console.Write(tbl.CallFunction("fun1",toPass));
            
            Console.ReadLine();
        }
        static object HelloWorld(SymbolTable s) {
            Console.Write(s.GetVariable("WhatToSayFirst"));
            return "World!";
        }
    }
    
}
