using System;
namespace Interpreter.Reserved {
    partial class Functions {
        //you can add 2 variables
        public static object Add_Fu(SymbolTable s) {

            object a = s.GetValue(new Id("~a"));
            object b = s.GetValue(new Id("~b"));
            try {
                
                return (int)a + (int)b;
            }
            catch {
                try {

                    return (double)a + (double)b;
                }
                catch {
                    try {

                        return (string)b + (string)a;
                    }
                    catch {
                        throw new Exception("you cannot add " + a.GetType() + " and " + b.GetType());
                    }
                }
            }
        }
    }
}