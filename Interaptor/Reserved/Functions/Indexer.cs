using System;
using System.Collections.Generic;
using System.Linq;
namespace Interpreter.Reserved {
    partial class Functions {
        public static object Indexer_Fu(Interpreter m) {
            int p = (int)s.GetValue(new Id("~index"));
            Objects.ObjectArray arr = (Objects.ObjectArray)s.GetValue(new Id("~arr"));
            return arr[p];
        }
    }
}