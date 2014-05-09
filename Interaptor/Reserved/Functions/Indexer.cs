using System;
using System.Collections.Generic;
using System.Linq;
namespace Interpreter.Reserved {
    partial class Functions {
        public static object Indexer_Fu(SymbolTable s) {
            int p = (int)s.GetValue(new Id("~index"));
            IEnumerable<object> ls = (IEnumerable<object>)s.GetValue(new Id("~enums"));
            return ls.ElementAt<object>(p);
        }
    }
}