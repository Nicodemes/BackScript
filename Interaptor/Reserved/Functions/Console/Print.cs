using System;
namespace Interpreter.Reserved {
    partial class Functions {
        //prints a variable to the screan
        public static object Print_Fu(Interpreter m) {
            Console.Write(m.ActiveScope.GetValue(new Id("~toPrint")));
            return new Void();
        }
        //Console.Writeine()
        public static object PrintL_Fu(Interpreter m) {
           Console.WriteLine(m.ActiveScope.GetValue(new Id("~toPrint")));
            return new Void();
        }
    }
}