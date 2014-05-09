using System;
namespace Interpreter.Reserved {
    partial class Functions {
        public static object WriteFile_Fu(SymbolTable s) {
            System.IO.File.WriteAllText((string)s.GetValue(new Id("~path")), (string)s.GetValue(new Id("~text")));
            return new Void();
        }
    }
}