using System;
using System.Collections.Generic;
namespace Interpreter.Reserved {
    partial class Functions {
        public static object ArrayCast_Fu(SymbolTable s) {
            int size= (int) s.GetValue(new Id("~size"));
            OrderedPair pair = (OrderedPair)(s.GetValue(new Id("~pair")));
            Reserved.Objects.ObjectArray arr = new Objects.ObjectArray(pair,size);
            return arr;
        }
        //casting
        public static object ListCast_Fu(SymbolTable s) {
            Opcodes p = s.GetValue(new Id("~items")) as Opcodes;
            List<object> toReturn = new List<object>();
            foreach (object a in p.ops) {
                if (a is Token) {
                    switch ((a as Token).type) {

                        case Token.Type.String:
                            toReturn.Add((a as Token).lexema);
                            break;
                        case Token.Type.Double:
                            toReturn.Add(Double.Parse((a as Token).lexema));
                            break;
                        case Token.Type.Integer:
                            toReturn.Add(Double.Parse((a as Token).lexema));
                            break;
                    }
                }

            }
            return toReturn;
        }
        public static object IntCast_Fu(SymbolTable s) {
            object toCast = s.GetValue(new Id("~toCast"));
            return Convert.ChangeType(toCast, typeof(int));
        }
        public static object DoubleCast_Fu(SymbolTable s) {
            object toCast = s.GetValue(new Id("~toCast"));
            return Convert.ChangeType(toCast, typeof(double));
        }

    }
}