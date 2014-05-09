using System;
namespace Interpreter.Reserved {
    partial class Functions {
        //prints a variable to the screan
        public static object Print_Fu(SymbolTable s) {
            Console.Write(s.GetValue(new Id("~toPrint")));
            return new Void();
        }
        //Console.Writeine()
       public static object PrintL_Fu(SymbolTable s) {
            Console.WriteLine(s.GetValue(new Id("~toPrint")));
            return new Void();
        }
    }
}